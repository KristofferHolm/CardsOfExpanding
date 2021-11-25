using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
[CustomEditor(typeof(CardManager))]
public class CardManager_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate Test Card"))
        {
            var cardman = target as CardManager;
            cardman.GenerateTestCard();
        }

        DrawDefaultInspector();
    }
}
