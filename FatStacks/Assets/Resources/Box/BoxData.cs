using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Box Data", fileName = "New Box Data")]
public class BoxData : ScriptableObject
{

    public string[] colorPrefabs;
    public GameObject[] destructionPrefab = new GameObject[4];
}
