using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintCardBehaviour : CardBehaviour
{
    BlueprintCard cardData;
    public int GetTurnsToBuild
    {
        get
        {
            return cardData.TurnsToBuild;
        }
    }
    public int GetBuildingId
    {
        get
        {
            return cardData.BuildingId;
        }
    }
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
        SetIcon(cardData.CardIcon);
    }

    public void PayThePrice()
    {
        InventoryManager.Instance.PayTheCost(cardData.WoodCost, cardData.StoneCost, cardData.FoodCost, 0, true);
    }

    protected override void CheckIfCardIsReadyToBeSpendable(bool spendable)
    {
        if (_isReadyToBeSpend == spendable) return;
        _isReadyToBeSpend = spendable;
        var canPay = InventoryManager.Instance.PayTheCost(cardData.WoodCost, cardData.StoneCost, cardData.FoodCost,0,false);

        if (canPay)
        {
            CardManager.Instance.OnCardBeingSpendable?.Invoke(this, spendable);
        }
        else
        {
            _isReadyToBeSpend = false;
            IsbeingDragged = false;
            MoveCamera.Instance.OnCardDraggin?.Invoke(false);
        }
    }
}
