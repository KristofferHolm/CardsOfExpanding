using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardBehaviour : MonoBehaviour
{
    List<TextMeshPro> texts;
    [SerializeField] private Transform TextParent;
    protected void SetText(string[] textsToSet)
    {
        if (!TextParent)
        {
            Debug.LogError("Transform TextParent is Null");
            return;
        }
        if (texts == null)
        {
            foreach (var tmp in TextParent.GetComponentsInChildren<TextMeshPro>())
            {
                texts.Add(tmp);
            }
        }
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = textsToSet[i]; 
        }
    }
}
