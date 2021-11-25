using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager : Singleton<CardManager>
{
    public Card TestCard;
    [Space(5)]
    public Transform HandTransform;
    public Transform DiscardPileTransform;
    public Transform DeckTransform;
    public Transform CardCreationPosition;
    public GameObject BlueprintCardPrefab, ActionCardPrefab;
    public enum CardPlace
    {
        Deck,
        Hand,
        Discard,
        Trash
    }

    float drawCardCooldown = 1f;
    private float drawingCardTime = 0f;

    private Stack<CardBehaviour> Deck, DiscardPile, Hand, TrashPile;
    private int drawCards = 0;
    private void Start()
    {
        if (Deck == null)
            Deck = new Stack<CardBehaviour>();
        if (DiscardPile == null)
            DiscardPile = new Stack<CardBehaviour>();
        if (Hand == null)
            Hand = new Stack<CardBehaviour>();
        if (TrashPile == null)
            TrashPile = new Stack<CardBehaviour>();
    }
    public void Update()
    {
        drawCardCooldown = Mathf.Clamp(drawCardCooldown + Time.deltaTime, 0, drawCardCooldown);
        if (drawCards <= 0 && drawingCardTime >= drawCardCooldown)
        {
            drawCardCooldown = 0;
            drawCards--;
            StartCoroutine(DrawCardAnimation());
        }
    }
    public GameObject CreateCard(Card cardData)
    {
        GameObject card;
        if (cardData is ActionCard)
        {
            card = Instantiate(ActionCardPrefab);
            card.GetComponent<ActionCardBehaviour>().SetCardData(cardData as ActionCard);
            card.transform.SetPositionAndRotation(CardCreationPosition.position, CardCreationPosition.rotation);
            return card;
        }
        else if (cardData is BlueprintCard)
        {
            card = Instantiate(BlueprintCardPrefab);
            card.GetComponent<BlueprintCardBehaviour>().SetCardData(cardData as BlueprintCard);
            card.transform.SetPositionAndRotation(CardCreationPosition.position, CardCreationPosition.rotation);
            return card;
        }
        return null;
    }
    public void GainCard(CardBehaviour card, CardPlace place)
    {
        // TODO: Add Animation
        switch (place)
        {
            case CardPlace.Deck:
                Deck.Push(card);
                Shuffle(CardPlace.Deck);
                break;
            case CardPlace.Hand:
                Hand.Push(card);
                break;
            case CardPlace.Discard:
                DiscardPile.Push(card);
                break;
            case CardPlace.Trash:
                TrashPile.Push(card);
                break;
            default:
                break;
        }
    }
    public void DrawCard(int amount = 1)
    {
        drawCards += amount;
    }
    IEnumerator DrawCardAnimation()
    {
        var card = Deck.Pop();
        yield return null;
    }

    public void Shuffle(CardPlace place)
    {
        Tools.ShuffleStack(GetStackByCardPlace(place));
    }

    public Stack<CardBehaviour> GetStackByCardPlace(CardPlace place)
    {
        switch (place)
        {
            case CardPlace.Deck:
                return Deck;
            case CardPlace.Hand:
                return Hand;
            case CardPlace.Discard:
                return DiscardPile;
            case CardPlace.Trash:
                return TrashPile;
            default:
                return new Stack<CardBehaviour>();
        }
    }
    public void CreateStartDeck()
    {
        
    }

    public void GenerateTestCard()
    {
        var card = CreateCard(TestCard);

        card.transform.SetPositionAndRotation(new Vector3(0, 10, 0), Quaternion.identity);
    }
}
