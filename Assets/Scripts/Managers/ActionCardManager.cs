using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCardManager : Singleton<ActionCardManager>
{
    public enum ActionCard
    {
        HarvestResource,
        CollectTax,
        CollectTax2,
        CollectTax3,
        PlantTree,
        Concrete
    }


    /// <summary>
    /// if conditions are not met, it returns null
    /// </summary>
    /// <param name="hexGrid"></param>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool GetActionCardAbility(HexGridBehaviour hexGrid, ActionCard card)
    {
        switch (card)
        {
            case ActionCard.HarvestResource:
                if (Resources(hexGrid))
                {
                    HarvestResourceAction(hexGrid, 1);
                    return true;
                }
                break;
            case ActionCard.CollectTax:
                CollectTax();
                return true;
                break;
            case ActionCard.PlantTree:
                if (IsTrees(hexGrid) && IsNotFull(hexGrid))
                {
                    PlantTree(hexGrid);
                    return true;
                }
                break;
            case ActionCard.Concrete:
                break;
            case ActionCard.CollectTax2:
                CollectTax2();
                return true;
                break;
            case ActionCard.CollectTax3:
                CollectTax3();
                return true;
                break;
            default:
                break;
        }
        return false;
    }
    #region Requirements

    private bool IsTrees(HexGridBehaviour hexGrid)
    {
        return hexGrid.Type == GridData.GridType.Trees;
    }
    private bool IsNotFull(HexGridBehaviour hexGrid)
    {
        return hexGrid.GetAmountOfResources < 5;
    }
    private bool Resources(HexGridBehaviour hexGrid)
    {
        switch (hexGrid.Type)
        {
            case GridData.GridType.Trees:
            case GridData.GridType.Stones:
            case GridData.GridType.Berries:
            case GridData.GridType.Fish:
                return true;
            default:
                return false;
        }
    }
#endregion
#region Actions

    private Action HarvestResourceAction(HexGridBehaviour hexGrid, int amount)
    {
        switch (hexGrid.Type)
        {
            case GridData.GridType.Trees:
                InventoryManager.Instance.GainResources(amount, 0, 0, 0);
                break;
            case GridData.GridType.Stones:
                InventoryManager.Instance.GainResources(0, amount, 0, 0);
                break;
            case GridData.GridType.Berries:
            case GridData.GridType.Fish:
                InventoryManager.Instance.GainResources(0, 0, amount, 0);
                break;
            default:
                break;
        }
        hexGrid.HarvestField(amount);
        InventoryManager.Instance.PayTheCost(0, 0, 0, 1, true);
        return null;
    }
    private Action CollectTax()
    {
        InventoryManager.Instance.GainCoin(1);
        return null;
    }
    private Action CollectTax2()
    {
        InventoryManager.Instance.GainCoin(3);
        return null;
    }
    private Action CollectTax3()
    {
        InventoryManager.Instance.GainCoin(6);
        return null;
    }
    private Action PlantTree(HexGridBehaviour hexGrid)
    {
        hexGrid.HarvestField(-1);
        return null;
    }

    #endregion
}
