using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsData", menuName = "HexGrid/BuildingsDataScriptableObject", order = 1)]
public class BuildingsData: ScriptableObject
{
    public List<Building> Database;
    
    [Serializable]
    public class Building
    {
        public string Name;
        public int Id;
        public Properties Properties;
    }

    [Serializable]
    public class Properties
    {
        //graphical properties
        public GameObject Graphic;

        // game properties
        //tbc
    }
}
