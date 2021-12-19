using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class HexGridBehaviour : MonoBehaviour
{
    public GridData.GridType Type = GridData.GridType.Undecided;
    public TextMeshPro HighlightText;
    public int SetStartBuildingId = -1;
    
    private GridData.Properties properties;
    private BuildingsData.Building building;
    private int amountOfResources = 5;
    private bool subscribedToShowResources;
    public int GetAmountOfResources {get => amountOfResources;}

    [HideInInspector]    public bool UnupdatedData;
    public int BuildingId
    {
        get
        {
            return building.Id;
        }
    }

    #region Resource Handeling
    void SetSubscribtionToShowResoures()
    {
        switch (Type)
        {
            case GridData.GridType.Trees:
            case GridData.GridType.Stones:
            case GridData.GridType.Berries:
            case GridData.GridType.Fish:
                if (subscribedToShowResources) break;
                subscribedToShowResources = true;
                GameManager.Instance.ShowResources += ShowResources;
                break;
            default:
                if (!subscribedToShowResources) break;
                subscribedToShowResources = false;
                GameManager.Instance.ShowResources -= ShowResources;
                return;
        }
    }

    public void ShowResources(bool show)
    {
        //secure check
        switch (Type)
        {
            case GridData.GridType.Trees:
            case GridData.GridType.Stones:
            case GridData.GridType.Berries:
            case GridData.GridType.Fish:
                break;
            default:
                return;
        }
        HighlightText.gameObject.SetActive(show);
        HighlightText.text = $"{amountOfResources} / 5 ";

    }

    public int HarvestField(int amount)
    {
        if (amount > amountOfResources)
        {
            amount = amountOfResources;
            amountOfResources = 0;
            EmptiedResources();
        }
        amountOfResources -= amount;
        if(amountOfResources == 0)
            EmptiedResources();
        return amount;
    }

    private void EmptiedResources()
    {
        switch (Type)
        {
            case GridData.GridType.Trees:
            case GridData.GridType.Stones:
            case GridData.GridType.Berries:
            case GridData.GridType.Building:
            case GridData.GridType.Construction:
                Type = GridData.GridType.Plane;
                break;
            case GridData.GridType.Fish:
                Type = GridData.GridType.Sea;
                break;
            default:
                break;
        }
        UpdateProperties();
    }
    #endregion



    #region Handeling the abilities of building

    public bool GetAbilityUsed
    {
        get
        {
            if (building.Properties != null)
                return building.Properties.AbilityUsed;
            else
            {
                Debug.LogError("no properties of this hexagrid", gameObject);
                return false;
            }
        }
    }

    public GameObject GetGraphic {
        get
        {
            if (Type == GridData.GridType.Building)
            {
                return building.Properties.Graphic;
            }
            else
            {
                return properties.Graphic;
            }

        }
    }
    public BuildingProgress GetBuildingProcess
    {
        get
        {
            return transform.GetComponentInChildren<BuildingProgress>();
        }
    }

 

    public void SetAbilityUsed(bool used)
    {
        if (Type == GridData.GridType.Building)
            building.Properties.AbilityUsed = used;
        else if (Type == GridData.GridType.Construction)
            GetBuildingProcess.AbilityUsed = used;
    }

    //TODO: Make a building spawning animation and continue with the progressing animation
    public void BuildingConstruction(int buildingId, int turnsToFinish)
    {
        Type = GridData.GridType.Construction;
        //HexGridPropertiesManager.TryGetBuildingData(4, out var contruction);
        //building = contruction;
        UnupdatedData = true;
        UpdateProperties();
        var bp = GetBuildingProcess;
        bp.BuildingId = buildingId;
        bp.NumberOfTurnsBeforeFinished = turnsToFinish;
        building.Id = buildingId;
        GameManager.Instance.OnNewTurn += () => SetAbilityUsed(false);
        BuildingAbilityManager.Instance.ActivateBuildingAbility(this);
    }

    public void FinishBuilding(int buildingId)
    {
        GameManager.Instance.OnNewTurn -= () => SetAbilityUsed(false);
        Type = GridData.GridType.Building;
        HexGridPropertiesManager.TryGetBuildingData(buildingId,out var newBuilding);
        building = newBuilding;
        UnupdatedData = true;
        UpdateProperties();
        
        if (building.Properties.Daily)
            GameManager.Instance.OnNewTurn += () => SetAbilityUsed(false);
        SetAbilityUsed(false);
    }

    public void Build(BuildingsData.Building newBuilding)
    {
        Type = GridData.GridType.Building;
        building = newBuilding;
        if (building.Properties.Daily)
            GameManager.Instance.OnNewTurn += () => SetAbilityUsed(false);
        UpdateProperties(true);
    }

    #endregion

    private void Start()
    {
        OnValidate();
        if(building.Properties != null && building.Properties.ActiveAbility)
            building.Properties.AbilityUsed = false;

        SetSubscribtionToShowResoures();
    }

    #region Graphic of the grid and management of which building it is
    private void OnValidate()
    {
        if (properties == null)
            properties = new GridData.Properties();
        if (building == null)
            building = new BuildingsData.Building();
       
        //if (!HexGridPropertiesManager.IsInstantiated) return;
        var prop = HexGridPropertiesManager.GetProperty(Type);
        if (Type != GridData.GridType.Building)
            SetStartBuildingId = -1;
        if (HexGridPropertiesManager.TryGetBuildingData(SetStartBuildingId, out var newbuilding))
        {
            building = newbuilding;
        }
        if (prop.Graphic != properties.Graphic)
            UnupdatedData = true;
        else if (prop.GroundType != properties.GroundType)
            UnupdatedData = true;
        else if (Type == GridData.GridType.Building && HexGridPropertiesManager.GetProperty(BuildingId).Graphic != building.Properties.Graphic)
        {
            UnupdatedData = true;
        }
    }
  
    public void UpdateProperties(bool forceUpdate = false)
    {
        if (!UnupdatedData && !forceUpdate) return;
        if (forceUpdate)
        {
            properties = new GridData.Properties();
            building = new BuildingsData.Building();
        }
        var prop = HexGridPropertiesManager.GetProperty(Type);
        UpdateGroundTexture(prop);
        UpdateGraphicObject(prop);
        SetSubscribtionToShowResoures();
        UnupdatedData = false;
    }


    public void UpdateGroundTexture(GridData.Properties prop)
    {
        if (properties.GroundType == prop.GroundType) return;
        GetComponent<MeshRenderer>().material = prop.GroundType;
        properties.GroundType = prop.GroundType;
    }

    void UpdateGraphicObject(GridData.Properties prop)
    {
        if (Type == GridData.GridType.Building)
        {
            UpdateBuildingGraphic(prop);
            return;
        }
        if (properties.Graphic == prop.Graphic) return;
        InstantiateNewGraphic(prop.Graphic);
        properties.Graphic = prop.Graphic;
    }

    private void UpdateBuildingGraphic(GridData.Properties prop)
    {
        GameObject prefabToSpawn;

        HexGridPropertiesManager.TryGetProperty(BuildingId, out var getprop);
        //if (getprop.Graphic == building.Properties.Graphic)
        //        return;
        if (getprop.Graphic == null)
        {
            //prototype graphic
            prefabToSpawn = prop.Graphic;
            prefabToSpawn.GetComponentInChildren<TMPro.TextMeshPro>().text = "Building ID: \n" + BuildingId;
        }
        else 
            prefabToSpawn = getprop.Graphic;
        
        InstantiateNewGraphic(prefabToSpawn);
        building.Properties.Graphic = prefabToSpawn;
    }

    private void InstantiateNewGraphic(GameObject go)
    { 
        //TODO: plz pool soon AND check better than amount of children
        while (transform.childCount > 1)
        {
            try
            {
                DestroyImmediate(transform.GetChild(1).gameObject);
            }
            catch (Exception)
            {
                try
                {
                    Destroy(transform.GetChild(1).gameObject);
                }
                catch (Exception)
                {
                    throw;
                }
                throw;
            }
        }
        if (go == null) return;
        var newGraphic = Instantiate(go, transform);
        newGraphic.transform.position = transform.position;
        //newGraphic.transform.rotation = transform.rotation;
        newGraphic.transform.rotation = transform.rotation * Quaternion.Euler(0, 60f * UnityEngine.Random.Range(0,5),0);
    }
    #endregion
}
