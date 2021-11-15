using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridPropertiesManager : Singleton<HexGridPropertiesManager>
{
    public GridData GridData;
    public BuildingsData BuildingsData;

    public bool TryGetProperty(int id, out BuildingsData.Properties properties)
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

    public BuildingsData.Properties GetProperty(int id)
    {
        foreach (var item in BuildingsData.Database)
        {
            if (item.Id == id)
                return item.Properties;
        }
        return null;
    }

    public bool TryGetProperty(GridData.GridType key, out GridData.Properties properties)
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

    public GridData.Properties GetProperty(GridData.GridType key)
    {
        foreach (var item in GridData.GridDatas)
        {
            if (item.Key == key)
                return item.Properties;
        }
        return null;
    }

}
