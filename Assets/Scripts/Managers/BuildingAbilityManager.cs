using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAbilityManager : Singleton<BuildingAbilityManager>
{
    public bool ActivateBuildingAbility(HexGridBehaviour hexGrid)
    {
        return GetAction(hexGrid);
    }

    private bool GetAction(HexGridBehaviour hexGrid)
    {
        if(hexGrid.Type == GridData.GridType.Construction)
            return BuildingProgress(hexGrid);
        //todo> all the actions;
        if (hexGrid.GetAbilityUsed) return false;
        switch (hexGrid.BuildingId)
        {
            case 3:
                return TownHall(hexGrid);
            case 11:
                return DrawingBoard();
            default:
                return false;
        }
        return false;
    }
    bool DrawingBoard()
    {
        GameManager.Instance.OpenCardsMenu?.Invoke(true);
        return true;
    }
    bool CoalMine()
    {
        if (InventoryManager.Instance.PayTheCost(0, 1, 0, 1,true))
        {
            CardManager.Instance.DrawCard(2);
            InventoryManager.Instance.Pollution++;
            return true;
        }
        return false;
    }
    bool LumberMill()
    {
        if (InventoryManager.Instance.PayTheCost(0, 0, 0, 1, true))
        {
            InventoryManager.Instance.GainResources(1, 0, 0, 0);
            InventoryManager.Instance.Pollution++;
            return true;
        }
        return false;
    }
    private bool BuildingProgress(HexGridBehaviour hexGrid)
    {
        var bp = hexGrid.GetComponentInChildren<BuildingProgress>();
        if (bp.AbilityUsed)
            return false;
        if (InventoryManager.Instance.PayTheCost(0, 0, 0, 1, true))
        {
            bp.AbilityUsed = true;
            bp.MoveScaffoldingUpANudge(1, (d) =>
            {
                if (d)
                    hexGrid.FinishBuilding(bp.BuildingId);
            });
            return true;
        }
        return false;
    }

    private bool TownHall(HexGridBehaviour hexGrid)
    {
        if (hexGrid.GetAbilityUsed)
            return false;
        CardManager.Instance.CreateStarterDeck();
        hexGrid.SetAbilityUsed(true);
        return true;
    }
}
