using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGridCreator))]
public class HexGridCreatorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Build Grid"))
        {
            HexGridCreator HexGridCreator = (HexGridCreator)target;
            HexGridCreator.InstantiateNewGrid();
        }

    }
   
}
