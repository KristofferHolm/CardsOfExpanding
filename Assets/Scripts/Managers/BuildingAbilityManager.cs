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
        //todo> all the actions;
        switch (hexGrid.BuildingId)
        {
            case 0:
            case 1:
            case 2:
                break;
            case 3:
                return TownHall(hexGrid);
            case 4:
                Tent();
                break;
            case 5:
            case 6:
            default:
                return false;
        }
        return false;
    }

    bool CoalMine()
    {
        if (InventoryManager.Instance.PayTheCost(0, 1, 0, 1))
        {
            CardManager.Instance.DrawCard(2);
            InventoryManager.Instance.Pollution++;
            return true;
        }
        return false;
    }
    bool LumberMill()
    {
        return true;
    }
    private void Tent()
    {
        
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
