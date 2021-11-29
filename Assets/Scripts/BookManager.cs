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
    private bool buttonIsActive = false;
    HexGridBehaviour currentHexGrid;
    private void OnValidate()
    {
        if(!bookAnimator)
            bookAnimator = GetComponent<Animator>();
    }

    public void OnBookUpdateText()
    {
        RightPage.text = rightText;
        LeftPage.text = leftText;
        ActivateButton.SetActive(buttonIsActive);
    }

    public void OpenBook(HexGridBehaviour hexGrid)
    {
        if (currentHexGrid != null && hexGrid.gameObject.GetInstanceID() == currentHexGrid.gameObject.GetInstanceID()) return;
        if (hexGrid.Type == GridData.GridType.Building)
        {
            if (!HexGridPropertiesManager.TryGetBuildingData(hexGrid.BuildingId, out var buildingData))
            {
                Debug.LogError("building ID of " + hexGrid.BuildingId + " could not be found");
                return;
            }
            currentHexGrid = hexGrid;
            leftText = buildingData.Name;
            //set text to building
            rightText = buildingData.Properties.BookText;
            buttonIsActive = buildingData.Properties.ActiveAbility;
        }
        else
        {
            currentHexGrid = hexGrid;
            rightText = "";
            leftText = "Hexgrid Type: " + hexGrid.Type.ToString();
            buttonIsActive = false;
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
    internal bool ActivateAbility()
    {
        return BuildingAbilityManager.Instance.ActivateBuildingAbility(currentHexGrid);
    }
}
