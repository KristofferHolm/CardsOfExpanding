using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuyCardsMenuHandler : MonoBehaviour, PlayerController.IBuyCardsMenuActions
{
    public GameObject CardHighlight;
    public StarterDeck_ScriptableObject ShopCollection;
    public Transform CardHolderTransform;
    public Camera CardCamera;
    public GameObject BuyButton, BackButton;

    Animator animator;
    private PlayerController controls;
    private Vector2 mousePosition;
    private bool buyMenuIsOn;
    private Transform EnlargedCardPos;
    private CardBehaviour markedCard, enlargedCard;
    private CardBehaviour EnlargedCard
    {
        get
        {
            return enlargedCard;
        }
        set
        {
            buyMenuIsOn = value != null;
            BuyButton.SetActive(buyMenuIsOn);
            BackButton.SetActive(buyMenuIsOn);
            enlargedCard = value;
        }
    }
    private float scroll, minScroll, maxScroll;


    private void Update()
    {
        if (CardHolderTransform.gameObject.activeSelf)
        {
            var scrollTo = new Vector3(scroll, 0.1f, 0);
            var dist = Vector3.Distance(CardHolderTransform.localPosition, scrollTo);
            CardHolderTransform.localPosition = Vector3.MoveTowards(CardHolderTransform.localPosition, scrollTo, Time.deltaTime + (dist) * Time.deltaTime);
        }
        //this is the buy menu 
        if (buyMenuIsOn)
        {
            enlargedCard.transform.localPosition = Vector3.MoveTowards(CardHolderTransform.localPosition, EnlargedCardPos.localPosition, Time.deltaTime);
        }

    }


    private void Start()
    {
        if(animator == null)
            animator = GetComponentInChildren<Animator>();
        GameManager.Instance.OpenCardsMenu += OpenAnimation;
        if (controls == null)
        {
            controls = new PlayerController();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.BuyCardsMenu.SetCallbacks(this);
        }
        SetTheCardsInTheirPlace(CreateAllTheCards(ShopCollection.StarterCards));
    }

    private List<GameObject> CreateAllTheCards(List<Card> listOfCards)
    {
        var ListOfGO = new List<GameObject>();
        foreach (var card in listOfCards)
        {
            var cardGo = CardManager.Instance.CreateCard(card, true);
            cardGo.GetComponent<CardBehaviour>().Collider.enabled = true;
            ListOfGO.Add(cardGo);
        }
        return ListOfGO;
    }

    private void SetTheCardsInTheirPlace(List<GameObject> listOfCards)
    {
        int x = 0,
            y = 0;
        Quaternion rot = Quaternion.Euler(0, -180, 90);
        foreach (var card in listOfCards)
        {
            card.transform.parent = CardHolderTransform;
            card.transform.localPosition = new Vector3(y * 2.5f, 0f, x * 1.75f - 3.5f);
            card.transform.localRotation = rot;
            card.transform.localScale = Vector3.one * 0.66f;
            x++;
            if (x == 5)
            {
                x = 0;
                y++;
            }
        }
        SetMinMaxScroll(y);
    }

    private void SetMinMaxScroll(int height)
    {
       
        minScroll = 0;
        // maxScroll = height * 2.5f;
        maxScroll = 4 * 2.5f;
    }

    private void OpenAnimation(bool open)
    {
        animator.SetBool("Enter", open);
        if (open)
            controls.BuyCardsMenu.Enable();
        else
            RunLater.Instance.EndOfFrame(() => controls.BuyCardsMenu.Disable());
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (markedCard != null)
        {
            EnlargeCardToBuy(markedCard);
        }

    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        if (Physics.Raycast(CardCamera.ScreenPointToRay(mousePosition), out var hit, 100, GameManager.Instance.CardMask))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.TryGetComponent(out CardBehaviour newMarkedCard))
            {
                if (markedCard != null && newMarkedCard != markedCard)
                    HighlightCard(markedCard, false);
                markedCard = newMarkedCard;
                HighlightCard(markedCard, true);
            }
            else
            {
                HighlightCard(markedCard, false);
                markedCard = null;
            }
        }
    }
    void EnlargeCardToBuy(CardBehaviour card)
    {
        if (EnlargedCard != null && EnlargedCard != card)
            SetCardBack(EnlargedCard);
        EnlargedCard = card;

        //show buttons, enlarge the card
    }
    void SetCardBack(CardBehaviour card)
    {
        //set card back to pos
    }

    private void HighlightCard(CardBehaviour card, bool on)
    {
        if (markedCard == null)
        {
            return;
        }
        //todo: make it an effect on the card and not an object being moved around
        CardHighlight.SetActive(on);
        if (on)
            CardHighlight.transform.SetPositionAndRotation(card.transform.position, card.transform.rotation);
        // highlight card graphically
    }
        

    public void OnExit(InputAction.CallbackContext context)
    {
        GameManager.Instance.OpenCardsMenu.Invoke(false);
    }

    public void OnMouseScroll(InputAction.CallbackContext context)
    {
        //scroll does a value of 120 per scroll
        scroll = Mathf.Clamp(scroll + Time.deltaTime * 0.5f * context.ReadValue<float>(), minScroll, maxScroll);
    }
}
