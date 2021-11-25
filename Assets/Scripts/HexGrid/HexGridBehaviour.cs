using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HexGridBehaviour : MonoBehaviour
{
    public GridData.GridType Type = GridData.GridType.Undecided;
    public int SetStartBuildingId = -1;
    private GridData.Properties properties;
    private BuildingsData.Building building;
    [HideInInspector]    public bool UnupdatedData;
    public int BuildingId
    {
        get
        {
            return building.Id;
        }
    }



    #region Handeling the abilities of building

    public void AbilityUsed(bool used)
    {
        building.Properties.AbilityUsed = used;
    }
    public void Build(BuildingsData.Building newBuilding)
    {
        Type = GridData.GridType.Building;
        building = newBuilding;
        if (building.Properties.Daily)
            GameManager.Instance.OnNewTurn += () => AbilityUsed(false);
        UpdateProperties();
    }

    #endregion

    private void Start()
    {
        OnValidate();
    }

    #region Graphic of the grid and management of which building it is
    private void OnValidate()
    {
       

        if (properties == null)
            properties = new GridData.Properties();
        if (building == null)
            building = new BuildingsData.Building();
       
        if (!HexGridPropertiesManager.IsInstantiated) return;
        if (SetStartBuildingId > 0)
        {
            Type = GridData.GridType.Building;
            if(HexGridPropertiesManager.Instance.TryGetBuildingData(SetStartBuildingId, out var newbuilding));
                building = newbuilding;
        }
        var prop = HexGridPropertiesManager.Instance.GetProperty(Type);
        if (prop.Graphic != properties.Graphic)
            UnupdatedData = true;
        else if (prop.GroundType != properties.GroundType)
            UnupdatedData = true;
        else if (Type == GridData.GridType.Building && HexGridPropertiesManager.Instance.GetProperty(BuildingId).Graphic != building.Properties.Graphic)
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
        var prop = HexGridPropertiesManager.Instance.GetProperty(Type);
        UpdateGroundTexture(prop);
        UpdateGraphicObject(prop);
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

        if (HexGridPropertiesManager.Instance.TryGetProperty(BuildingId, out var getprop))
        {
            if (getprop.Graphic == building.Properties.Graphic) return;
            prefabToSpawn = getprop.Graphic;
        }
        else
        {
            //prototype graphic
            prefabToSpawn = prop.Graphic;
            prefabToSpawn.GetComponentInChildren<TMPro.TextMeshPro>().text = "Building ID: \n" + BuildingId;
        }
        InstantiateNewGraphic(prefabToSpawn);
        building.Properties.Graphic = prefabToSpawn;
    }

    private void InstantiateNewGraphic(GameObject go)
    { 
        //TODO: plz pool soon
        if (transform.childCount != 0)
        {
            try
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            catch (Exception)
            {
                try
                {
                    Destroy(transform.GetChild(0).gameObject);
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
