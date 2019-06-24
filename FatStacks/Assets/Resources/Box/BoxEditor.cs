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
            script.ApplyColor();
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

    private void ApplyColorToEveryTarget(Box.GroupIdNames groupId)
    {
        for (int i = 0; i < targets.Length; ++i)
        {
            Box script = (Box)targets[i];
            script.groupId = groupId;
            script.ApplyColor();
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
