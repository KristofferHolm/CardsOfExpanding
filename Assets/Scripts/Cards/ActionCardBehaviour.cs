using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCardBehaviour : CardBehaviour
{
    ActionCard cardData;

    public void SetCardData(ActionCard data)
    {
        cardData = data;

        string[] texts = {
            cardData.Title,
            cardData.WorkerCost.ToString(),
            cardData.Description,
            cardData.Level.ToString(),
            cardData.pollutionCost.ToString()
        };
        SetText(texts);
        SetIcon(cardData.CardIcon);
    }
}
