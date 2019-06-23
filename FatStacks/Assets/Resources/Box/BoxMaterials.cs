using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Box Materials", fileName = "New Box Materials")]
public class BoxMaterials : ScriptableObject
{

    public Material[] baseMaterials;
    public Material[] colorMaterials = new Material[4];
    public GameObject[] destructionPrefab = new GameObject[4];
}
