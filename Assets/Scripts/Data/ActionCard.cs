using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewActionCard", menuName = "Card/New Action Card", order = 2)]
public class ActionCard : Card
{
    public int WorkerCost;
    public int Level;
    public int pollutionCost;
    public UnityAction Ability;
}
