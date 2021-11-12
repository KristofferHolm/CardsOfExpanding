using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(HexGridBehaviour))]
public class HexGridBehaviourInspector : Editor
{
    HexGridBehaviour type;
    [CanEditMultipleObjects]
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (CheckUnupdatedData())
            if (GUILayout.Button("Update Data"))
            {
                foreach (HexGridBehaviour item in targets)
                {
                    item.UpdateProperties();
                }
            }
        
    }
    bool CheckUnupdatedData()
    {
        foreach (HexGridBehaviour item in targets)
        {
            if (item.UnupdatedData)
                return true;
        }
        return false;
    }

    [CanEditMultipleObjects]
    void ChangeType(GridData.GridType type)
    {
        var targetsList = new List<HexGridBehaviour>();
        foreach (var item in targets)
        {
            if (item.GetType() == typeof(HexGridBehaviour))
                targetsList.Add(item as HexGridBehaviour);
        }
        foreach (var item in targetsList)
        {
            item.UpdateType(type);
        }
    }
}
