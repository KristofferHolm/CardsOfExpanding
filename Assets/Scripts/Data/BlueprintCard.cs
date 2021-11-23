using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlueprintCard", menuName = "Card/New Blueprint Card", order = 1)]
public class BlueprintCard : Card
{
    public int BuildingId;
    public int WoodCost, StoneCost, FoodCost, CoinCost, TurnsToBuild;
    public Card[] GivesCard;
}
