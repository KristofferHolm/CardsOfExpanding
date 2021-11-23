using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CardBehaviour))]
public class CardBehaviourInspector : Editor
{
    public Card CardToInsert;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Update Card Data"))
        {
            if (CardToInsert == null) return;
            if (target is ActionCardBehaviour)
            {
                var card = target as ActionCardBehaviour;
                card.SetCardData(CardToInsert as ActionCard);
            }
            if (target is BlueprintCardBehaviour)
            {
                var card = target as BlueprintCardBehaviour;
                card.SetCardData(CardToInsert as BlueprintCard);
            }

        }
    }
}
