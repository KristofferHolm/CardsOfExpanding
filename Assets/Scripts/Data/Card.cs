using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewCard", menuName = "Card/NewCard_ScriptableObject", order = 1)]
public class Card : ScriptableObject
{
    public string Title;
    [TextArea(10, 10)] public string Description;
    public Texture CardIcon;
}
