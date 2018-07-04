using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SnapObject))]
public class SnapHelper : Editor
{
    public override void OnInspectorGUI()
    {
        var snapObj = target as SnapObject;

        snapObj.testTarget = EditorGUILayout.ObjectField("Test Target Object", snapObj.testTarget, typeof(SnapTarget), true) as SnapTarget;

        if (GUILayout.Button("Snap"))
            snapObj.SnapToTarget(snapObj.testTarget);

    }
}
