using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public int Stone, Wood, Food, Workers, WorkersLeft, Pollution;

    public bool PayTheCost(int wood, int stone, int food, int workersleft)
    {
        if (stone > Stone)
            return false;
        if (wood > Wood)
            return false;
        if (food > Food)
            return false;
        if (workersleft > WorkersLeft)
            return false;
        Stone -= stone;
        Wood -= wood;
        Food -= food;
        WorkersLeft -= workersleft;
        return true;
    }
}
