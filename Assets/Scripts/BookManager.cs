using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookManager : Singleton<BookManager>
{
    [SerializeField] Animator bookAnimator;


    [SerializeField] TextMeshPro RightPage, LeftPage;
    [SerializeField] GameObject ActivateButton;
    private bool isOpen = false;
    private string rightText, leftText;
    HexGridBehaviour currentHexGrid;
    private void OnValidate()
    {
        if(!bookAnimator)
            bookAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
       
    }

    public void OnBookUpdateText()
    {
        RightPage.text = rightText;
        LeftPage.text = leftText;
    }

    public void OpenBook(HexGridBehaviour hexGrid)
    {
        if (hexGrid.Type == GridData.GridType.Building)
        {
            if (!HexGridPropertiesManager.Instance.TryGetBuildingData(hexGrid.BuildingId, out var buildingData))
            {
                Debug.LogError("building ID of " + hexGrid.BuildingId + " could not be found");
                return;
            }
            currentHexGrid = hexGrid;
            leftText = buildingData.Name;
            //set text to building
            rightText = buildingData.Properties.BookText;
            ActivateButton.SetActive(buildingData.Properties.ActiveAbility);
        }
        else
        {
            currentHexGrid = hexGrid;
            leftText = "Hexgrid Type: " + hexGrid.Type.ToString();
            ActivateButton.SetActive(false);
        }

        if (isOpen)
        {
            bookAnimator.SetTrigger("Flip");
        }
        else
        {
            bookAnimator.SetBool("Open", true);
        }
        isOpen = true;
    }
    public void CloseBook()
    {
        bookAnimator.SetBool("Open", false);
        isOpen = false;
    }
    internal void ActivateAbility()
    {
        BuildingAbilityManager.Instance.ActivateBuildingAbility(currentHexGrid);
    }
}
