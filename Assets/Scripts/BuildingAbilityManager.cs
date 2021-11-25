using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAbilityManager : Singleton<BuildingAbilityManager>
{
    public void ActivateBuildingAbility(HexGridBehaviour hexGrid)
    {
        GetAction(hexGrid);
    }

    private void GetAction(HexGridBehaviour hexGrid)
    {
        //todo> all the actions;
        switch (hexGrid.BuildingId)
        {
            case 0:
            case 1:
            case 2:
                break;
            case 3:
                TownHall(hexGrid);
                break;
            case 4:
                Tent();
                break;
            case 5:
            case 6:
            default:
                break;
        }
    }

    private void Tent()
    {
        
    }

    private void TownHall(HexGridBehaviour hexGrid)
    {
        CardManager.Instance.CreateStartDeck();
        hexGrid.AbilityUsed(true);
    }
}
