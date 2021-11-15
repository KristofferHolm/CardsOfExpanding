using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "HexGrid/GridDataScriptableObject", order = 1)]
public class GridData : ScriptableObject
{
    public List<Data> GridDatas;

    public void CreateNewListOfData()
    {
        GridDatas = new List<Data>();
        foreach (var item in Enum.GetValues(typeof(GridType)))
        {
            var data = new Data();
            data.Name = item.ToString();
            data.Key = (GridType)item;
            GridDatas.Add(data);
        }
    }

    public enum GridType
    {
        Undecided,
        Sea,
        Plane,
        Trees,
        Stones,
        Berries,
        Fish,
        Building
    }

    [Serializable]
    public class Data
    {
        [HideInInspector]
        public string Name;
        public GridType Key;
        public Properties Properties;
    }

    [Serializable]
    public class Properties
    {
        //graphical properties
        public Material GroundType;
        public GameObject Graphic;

        // game properties
        //public ShopManager.Currency Currency;
    }
}
