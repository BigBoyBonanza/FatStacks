using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCoordDictionary : MonoBehaviour
{
    Dictionary<Vector3Int, HashSet<GameObject>> _Dictionary = new Dictionary<Vector3Int, HashSet<GameObject>>();

    public void Add(Vector3Int coord, GameObject obj)
    {
        if (_Dictionary.ContainsKey(coord) == true)
        {
            _Dictionary[coord].Add(obj);
        }
        else
        {
            _Dictionary.Add(coord, new HashSet<GameObject>());
            _Dictionary[coord].Add(obj);
        }

    }

    public GameObject[] Get(Vector3Int coord)
    {
        if (_Dictionary.ContainsKey(coord) == true)
        {
            HashSet<GameObject> cell = _Dictionary[coord];
            GameObject[] collection = new GameObject[cell.Count];
            _Dictionary[coord].CopyTo(collection);
            return collection;
        }
        else
        {
            return null;
        }
    }

    public void Remove(Vector3Int coord, GameObject obj)
    {
        if (_Dictionary.ContainsKey(coord) == true)
        {
            HashSet<GameObject> cell = _Dictionary[coord];
            cell.Remove(obj);
            if (cell.Count == 0)
            {
                _Dictionary.Remove(coord);
            }
        }
    }
}