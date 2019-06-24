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
        update_coords();
    }

    private void OnDestroy()
    {
        RemoveMyself();
    }
    void update_coords()
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
        Box neighbor = (neighborGameObjects == null) ? null : neighborGameObjects[0].GetComponent<Box>();
        if (neighbor != null)
        {
            return weight + neighbor.GetStackWeight();
        }
        else
        {
            return weight;
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

    [ContextMenu("Apply Color")]
    public virtual void ApplyColor()
    {
        Debug.Log("Fired");
        //Get object
        GameObject obj = (GameObject)Resources.Load(boxData.colorPrefabs[(int)groupId]);
        if (obj != null)
        {
            
            Box box = obj.GetComponent<Box>();

            //Check if group is overridden property
            PropertyModification[] propertyModifications = PrefabUtility.GetPropertyModifications(box);
            //Debug.Log(propertyModifications[propertyModifications.Length-1].propertyPath);
            //if (propertyModifications[propertyModifications.Length-1].propertyPath)
            //{
                //Instantiate object
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(obj);

                //Copy transform
                instance.transform.position = transform.position;
                instance.transform.parent = transform.parent;

            Selection.activeGameObject = instance;

                //Destroy old object
                DestroyImmediate(gameObject);
            //}
            //else
            //{
            //    Debug.Log("ApplyColor not necessary.");
            //}

        }
        else
        {
            Debug.Log("'" + boxData.colorPrefabs[(int)groupId] + "' could not be found.");
        }
        
    }
    /*
    MatchThreeMaterialAtlas atlas = GetComponentInParent<MatchThreeMaterialAtlas>();
    MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
    Material[] new_materials = mesh_renderer.sharedMaterials;
    string tail = get_tail();
    if (!atlas.materials.ContainsKey(i_am + tail))
    {
        //Create a new material and store it. Then set it.
        switch (match3_group_id)
        {
            case match3_group_id_names.blue:
                new_materials[1] = create_material_color_variant(new_materials[1], Color.blue, atlas);
                break;
            case match3_group_id_names.red:
                new_materials[1] = create_material_color_variant(new_materials[1], Color.red, atlas);
                break;
            case match3_group_id_names.yellow:
                new_materials[1] = create_material_color_variant(new_materials[1], Color.yellow, atlas);
                break;
            case match3_group_id_names.green:
                new_materials[1] = create_material_color_variant(new_materials[1], Color.green, atlas);
                break;
        }
        atlas.materials[i_am + tail] = new_materials[1];
    }
    else
    {
        //Set new material
        new_materials[1] = atlas.materials[i_am + tail];
    }
    mesh_renderer.sharedMaterials = new_materials;

}

private string get_tail()
{
    return match3_group_id.ToString();
}

private Material create_material_color_variant(Material base_material, Color color, MatchThreeMaterialAtlas atlas)
{
    Material new_color_material = new Material(base_material);
    new_color_material.SetColor("_Color", color);
    return new_color_material;

}
*/
}
