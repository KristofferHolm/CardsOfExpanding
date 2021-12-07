using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InventoryManager : Singleton<InventoryManager>
{

    public Transform TopbarWithResources;

    private List<TextMeshPro> resourcesText;
    private int pollution;
    private int stone;
    private int wood;
    private int food;
    private int workers;
    private int workersLeft;
    public int Stone
    {
        get => stone; set
        {
            stone = value;
            UpdateText();
        }
    }
    public int Wood
    {
        get => wood; set
        {
            wood = value;
            UpdateText();
        }
    }
    public int Food
    {
        get => food; set
        {
            food = value;
            UpdateText();
        }
    }
    public int Workers
    {
        get => workers; set
        {
            workers = value;
            UpdateText();
        }
    }
    public int WorkersLeft
    {
        get => workersLeft; set
        {
            workersLeft = value;
            UpdateText();
        }
    }
    public int Pollution
    {
        get => pollution; set
        {
            pollution = value;
            UpdateText();
        }
    }

    public List<TextMeshPro> ResourcesText { 
        get
        {
            if (resourcesText == null)
            {
                resourcesText = new List<TextMeshPro>();
                for (int i = 0; i < TopbarWithResources.childCount; i++)
                {
                    resourcesText.Add(TopbarWithResources.GetChild(i).GetComponent<TextMeshPro>());
                }
            }
            return resourcesText; 
        } 
        set => resourcesText = value; 
    }
    private void Start()
    {
        UpdateText();
        GameManager.Instance.OnNewTurn += GainWorkersForNewTurn;
    }

    private void GainWorkersForNewTurn()
    {
        WorkersLeft = Workers;
    }

    private void UpdateText()
    {
        //workersleft out of workers first then:
        //wood, stone, food, pollution
        ResourcesText[0].text = workersLeft.ToString();
        ResourcesText[1].text = workers.ToString();
        ResourcesText[2].text = wood.ToString();
        ResourcesText[3].text = stone.ToString();
        ResourcesText[4].text = food.ToString();
        ResourcesText[5].text = pollution.ToString();
    }
    public void GainResources(int wood = 0, int stone =0, int food=0, int workersleft=0)
    {
        Wood += wood;
        Stone += stone;
        Food += food;
        WorkersLeft += workersleft;
    }

    public bool PayTheCost(int wood, int stone, int food, int workersleft, bool pay)
    {
        if (stone > Stone)
            return false;
        if (wood > Wood)
            return false;
        if (food > Food)
            return false;
        if (workersleft > WorkersLeft)
            return false;

        if (pay)
        {
            Stone -= stone;
            Wood -= wood;
            Food -= food;
            WorkersLeft -= workersleft;
        }
        return true;
    }
}
