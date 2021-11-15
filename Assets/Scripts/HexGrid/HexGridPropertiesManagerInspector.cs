using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGridPropertiesManager))]
public class HexGridPropertiesManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("RefreshAllGrid"))
        {
            foreach (var item in FindObjectsOfType<HexGridBehaviour>())
            {
                item.UpdateProperties(true);
            }
        }
    }
}
