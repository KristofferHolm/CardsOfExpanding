using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridData))]
public class GridDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Create New List"))
        {
            GridData data = target as GridData;
            data.CreateNewListOfData();
        }
    }
}
