using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Box))]
[CanEditMultipleObjects]
public class BoxEditor : Editor
{
    public Box.GroupIdNames _groupId;
    public Box.GroupIdNames groupId {
        get { return _groupId; }
        set
        {
            Box script = (Box)target;
            script.groupId = value;
            ApplyColor(script);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.BeginHorizontal();
        
        foreach (Box.GroupIdNames groupName in System.Enum.GetValues(typeof(Box.GroupIdNames)))
        {
            if (GUILayout.Button(groupName.ToString()))
            {
                ApplyColorToEveryTarget(groupName);
            }
        }
        
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Snap To Whole Coordinate"))
        {
            SnapEveryTargetToWholeCoordinate();
        }
        
    }

    public void ApplyColor(Box box)
    {
        //Get object
        GameObject varient = (GameObject)Resources.Load(box.boxData.colorPrefabs[(int)box.groupId]);
        if (varient != null)
        {
            Box boxVarient = varient.GetComponent<Box>();

            //Check if group is overridden property
            PropertyModification[] propertyModifications = PrefabUtility.GetPropertyModifications(boxVarient);

            //Instantiate object
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(varient);

            //Copy transform
            instance.transform.position = box.transform.position;
            instance.transform.parent = box.transform.parent;

            Selection.activeGameObject = instance;

            //Destroy old object
            DestroyImmediate(box.gameObject,true);
        }
        else
        {
            Debug.Log("'" + box.boxData.colorPrefabs[(int)groupId] + "' could not be found.");
        }
    }

    private void ApplyColorToEveryTarget(Box.GroupIdNames groupId)
    {
        for (int i = 0; i < targets.Length; ++i)
        {
            Box script = (Box)targets[i];
            script.groupId = groupId;
            ApplyColor(script);
        }
    }

    private void SnapEveryTargetToWholeCoordinate()
    {
        for (int i = 0; i < targets.Length; ++i)
        {
            Box script = (Box)targets[i];
            script.SnapToWholeCoordinate();
        }
    }
}
