using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintCardBehaviour : CardBehaviour
{
    BlueprintCard cardData;

    public void SetCardData(BlueprintCard data)
    {
        cardData = data;
    
    string[] texts = {
            cardData.Title,
            cardData.Description,
            cardData.WoodCost.ToString(),
            cardData.StoneCost.ToString(),
            cardData.FoodCost.ToString(),
            cardData.TurnsToBuild.ToString(),
        };
        SetText(texts);
    }
}
