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
        PlantTree,
        Concrete
    }



    public bool GetActionCardAbility(HexGridBehaviour hexGrid, ActionCard card, out Action ability)
    {
        ability = null;
        switch (card)
        {
            case ActionCard.HarvestResource:
                ability = HarvestResourceAction(hexGrid);
                return Resources(hexGrid);
            case ActionCard.CollectTax:
                ability = CollectTax();
                return true;
            case ActionCard.PlantTree:
                ability = PlantTree(hexGrid);
                return IsTrees(hexGrid);
            case ActionCard.Concrete:
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

    private Action HarvestResourceAction(HexGridBehaviour hexGrid)
    {
        switch (hexGrid.Type)
        {
            case GridData.GridType.Trees:
                InventoryManager.Instance.GainResources(1, 0, 0, 0);
                break;
            case GridData.GridType.Stones:
                InventoryManager.Instance.GainResources(0, 1, 0, 0);
                break;
            case GridData.GridType.Berries:
            case GridData.GridType.Fish:
                InventoryManager.Instance.GainResources(0, 0, 1, 0);
                break;
            default:
                break;
        }
        InventoryManager.Instance.PayTheCost(0, 0, 0, 1, true);
        return null;
    }
    private Action CollectTax()
    {
        InventoryManager.Instance.GainCoin(1);
        return null;
    }
    private Action PlantTree(HexGridBehaviour hexGrid)
    {
        hexGrid.HarvestField(-1);
        return null;
    }

#endregion
}
