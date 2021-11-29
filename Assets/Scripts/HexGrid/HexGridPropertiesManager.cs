using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridPropertiesManager : Singleton<HexGridPropertiesManager>
{
    private static string buildingDataPath = "BuildingsData";
    private static string gridDataPath = "GridData";
    private static GridData _gridData;
    public static GridData GridData
    {
        get
        {
            if (_gridData == null)
                _gridData = Resources.Load(gridDataPath) as GridData;
            return _gridData;
        }
        set
        {
            _gridData = value;
        }
    }
    private static BuildingsData _buildingsData;
    public static BuildingsData BuildingsData
    {
        get
        {
            if (_buildingsData == null)
                _buildingsData = Resources.Load(buildingDataPath) as BuildingsData;
            return _buildingsData;
        }
        set
        {
            _buildingsData = value;
        }
    }
    

    public static bool TryGetProperty(int id, out BuildingsData.Properties properties)
    {
        properties = null;
        foreach (var item in BuildingsData.Database)
        {
            if (item.Id == id)
            {
                properties = item.Properties;
                return true;
            }
        }
        return false;
    }
    public static bool TryGetBuildingData(int id, out BuildingsData.Building buildingsData)
    {
        buildingsData = null;
        foreach (var item in BuildingsData.Database)
        {
            if (item.Id == id)
            {
                buildingsData = item;
                return true;
            }
        }
        return false;
    }


    public static BuildingsData.Properties GetProperty(int id)
    {
        foreach (var item in BuildingsData.Database)
        {
            if (item.Id == id)
                return item.Properties;
        }
        return null;
    }

    public static bool TryGetProperty(GridData.GridType key, out GridData.Properties properties)
    {
        properties = null;
        foreach (var item in GridData.GridDatas)
        {
            if (item.Key == key)
            {
                properties = item.Properties;
                return true;
            }
        }
        return false;
    }

    public static GridData.Properties GetProperty(GridData.GridType key)
    {
        foreach (var item in GridData.GridDatas)
        {
            if (item.Key == key)
                return item.Properties;
        }
        return null;
    }

}
