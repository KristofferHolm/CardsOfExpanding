using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCardBehaviour : CardBehaviour
{
    ActionCard cardData;
    
    public ActionCardManager.ActionCard GetAbility
    {
        get {
        return cardData.Ability;
        }
    }

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

    protected override void CheckIfCardIsReadyToBeSpendable(bool spendable)
    {
      
        if (_isReadyToBeSpend == spendable) return;
        _isReadyToBeSpend = spendable;
        base.CheckIfCardIsReadyToBeSpendable(spendable);
        var canPay = InventoryManager.Instance.PayTheCost(0, 0, 0, cardData.WorkerCost, false);

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
