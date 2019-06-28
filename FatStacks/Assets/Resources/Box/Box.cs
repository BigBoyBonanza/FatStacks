using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Box : MonoBehaviour
{
    public BoxData boxData;

    [HideInInspector]
    public Grid _Grid;
    [HideInInspector]
    public BoxCoordDictionary _BoxCoordDictionary;

    [SerializeField]
    protected string i_am = "m3Object";
    public string resourcePath;
    public int weight;

    private bool is_being_checked = false;
    protected bool[] was_neighbor_checked = new bool[6];
    private static Vector3 center_local_transform = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField]
    private Vector3[] match3_coord_evaluator_local_transforms = new Vector3[]
    {
        new Vector3(0.5f,0.3f,0.5f),
        new Vector3(0.5f,0.7f,0.5f)
    };
    private static Vector3Int[] neighbor_local_coords = new Vector3Int[] {
        Vector3Int.up,
        Vector3Int.right,
        new Vector3Int(0,0,1),
        new Vector3Int(0,0,-1),
        Vector3Int.left,
        Vector3Int.down
    };

    [HideInInspector]
    public Vector3Int[] coord;
    [HideInInspector]
    public Vector3Int[] prev_coord;

    public GroupIdNames groupId;

    public enum GroupIdNames
    {
        Blue,
        Red,
        Green,
        Yellow
    }

    private bool _frozen;
    public bool Frozen
    {
        get { return _frozen; }
        set
        {
            if (value)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            }
            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
            _frozen = value;
        }
    }

    void Awake()
    {
        _Grid = GetComponentInParent<Grid>();
        _BoxCoordDictionary = GetComponentInParent<BoxCoordDictionary>();
    }
    void Start()
    {
        coord = new Vector3Int[match3_coord_evaluator_local_transforms.Length];
        prev_coord = new Vector3Int[match3_coord_evaluator_local_transforms.Length];
        //Initialize coord
        for (int i = 0; i < match3_coord_evaluator_local_transforms.Length; ++i)
        {
            coord[i] = _Grid.WorldToCell(transform.position + match3_coord_evaluator_local_transforms[i]);
            _BoxCoordDictionary.Add(coord[i], gameObject);
        }
        coord.CopyTo(prev_coord, 0);
    }
    public virtual void FixedUpdate()
    {
        UpdateCoords();
    }

    private void OnDestroy()
    {
        RemoveMyself();
    }
    void UpdateCoords()
    {
        AddMyself();
        for (int j = 0; j < prev_coord.Length; ++j)
        {
            bool found = false;
            for (int k = 0; k < coord.Length; ++k)
            {
                if (prev_coord[j] == coord[k])
                {
                    found = true;
                }
            }
            if (!found)
            {
                //This coord
                //Debug.Log("Cell removed");
                _BoxCoordDictionary.Remove(prev_coord[j], transform.gameObject);
            }
        }
        coord.CopyTo(prev_coord, 0);

    }

    //Returns a list of matching neighbors
    public List<Box> GetMatchingNeighbors()
    {
        List<Box> matching_neigbors = new List<Box>();
        is_being_checked = true;
        for (int i = 0; i < coord.Length; ++i)
        {
            for (int j = 0; j < neighbor_local_coords.Length; ++j)
            {
                //Iterate through all neighbors
                if (was_neighbor_checked[j] == true)
                {
                    //Skip this neighbor (because this neighbor made the recursive call)
                    continue;
                }
                //Get neighbor from match3_grid
                GameObject[] neighbor_game_objects = _BoxCoordDictionary.Get(coord[i] + neighbor_local_coords[j]);

                if (neighbor_game_objects != null)
                {
                    for (int k = 0; k < neighbor_game_objects.Length; ++k)
                    {
                        //Neighbor exists
                        Box neighbor = neighbor_game_objects[k].GetComponent<Box>();
                        if (!neighbor.is_being_checked && neighbor.groupId == groupId)
                        {
                            //Neighbor is of the same group
                            //Check if neighbor is close enough
                            if (Vector3.Distance(neighbor.transform.position + center_local_transform, transform.position + center_local_transform) < 1.3f)
                            {
                                neighbor.was_neighbor_checked[neighbor_local_coords.Length - (j + 1)] = true;
                                was_neighbor_checked[j] = true;
                                neighbor.GetMatchingNeighborsHelper(matching_neigbors);
                            }
                        }
                    }
                }

            }
        }
        matching_neigbors.Add(this);
        foreach (Box obj in matching_neigbors)
        {
            obj.ResetChecked();
        }

        return matching_neigbors;
    }

    //Helper function for finding matching neighbors
    private void GetMatchingNeighborsHelper(List<Box> matching_neigbors)
    {
        is_being_checked = true;
        for (int i = 0; i < coord.Length; ++i)
        {
            for (int j = 0; j < neighbor_local_coords.Length; ++j)
            {
                //Iterate through all neighbors
                if (was_neighbor_checked[j] == true)
                {
                    //Skip this neighbor (because this neighbor made the recursive call)
                    continue;
                }
                //Get neighbor from match3_grid
                GameObject[] neighbor_game_objects = _BoxCoordDictionary.Get(coord[i] + neighbor_local_coords[j]);

                if (neighbor_game_objects != null)
                {
                    for (int k = 0; k < neighbor_game_objects.Length; ++k)
                    {
                        //Neighbor exists
                        Box neighbor = neighbor_game_objects[k].GetComponent<Box>();
                        if (!neighbor.is_being_checked && neighbor.groupId == groupId)
                        {
                            //Neighbor is of the same group
                            //Check if neighbor is close enough
                            if (Vector3.Distance(neighbor.transform.position + center_local_transform, transform.position + center_local_transform) < 1.3f)
                            {
                                neighbor.was_neighbor_checked[neighbor_local_coords.Length - (j + 1)] = true;
                                was_neighbor_checked[j] = true;
                                neighbor.GetMatchingNeighborsHelper(matching_neigbors);
                            }
                        }
                    }
                }

            }
        }
        matching_neigbors.Add(this);
    }

    public int GetStackWeight()
    {
        //Box neighbor = _BoxCoordDictionary?.Get(coord[0] + Vector3Int.up)?[0]?.GetComponent<Box>(); up-to-date C#
        GameObject[] neighborGameObjects = _BoxCoordDictionary.Get(coord[0] + Vector3Int.up);
        Box neighbor = neighborGameObjects?[0].GetComponent<Box>();
        if (neighbor != null)
        {
            return weight + neighbor.GetStackWeight();
        }
        else
        {
            return weight;
        }

    }

    public Box GetBoxOnTopOfMe()
    {
        return _BoxCoordDictionary.Get(coord[0] + Vector3Int.up)?[0].GetComponent<Box>();
    }

    public Box GetBoxOnTopOfMyStack()
    {
        Box neighbor = GetBoxOnTopOfMe();
        if (neighbor == null)
        {
            return this;
        }
        else
        {
            return neighbor.GetBoxOnTopOfMyStack();
        }
        
    }
    public void ResetChecked()
    {
        is_being_checked = false;
        for (int i = 0; i < was_neighbor_checked.Length; ++i)
        {
            was_neighbor_checked[i] = false;
        }
    }
    public void RemoveMyself()
    {
        for (int i = 0; i < coord.Length; ++i)
        {
            _BoxCoordDictionary.Remove(coord[i], transform.gameObject);
        }
    }

    public void AddMyself(bool forceAdd = false)
    {
        for (int i = 0; i < match3_coord_evaluator_local_transforms.Length; ++i)
        {
            coord[i] = _Grid.WorldToCell(transform.position + match3_coord_evaluator_local_transforms[i]);
            if (forceAdd == true || coord[i] != prev_coord[i])
            {
                //Debug.Log("Cell added");
                _BoxCoordDictionary.Add(coord[i], gameObject);
            }
        }
    }

    [ContextMenu("Snap To Whole Coordinate")]
    public void SnapToWholeCoordinate()
    {
        Grid _grid = GetComponentInParent<Grid>();
        transform.position = _grid.CellToWorld(_grid.WorldToCell(transform.position));
    }

    
}
