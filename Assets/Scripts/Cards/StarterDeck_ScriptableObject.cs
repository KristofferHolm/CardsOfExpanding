using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStarterDeck", menuName = "StaterDeck/NewStarterDeck", order = 1)]
public class StarterDeck_ScriptableObject : ScriptableObject
{
    public List<Card> StarterCards;
}
