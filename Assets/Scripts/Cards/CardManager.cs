using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CardManager : Singleton<CardManager>
{
    public Card TestCard;
    [Space(5)]
    public StarterDeck_ScriptableObject StarterDeck;
    public Transform HandTransform;
    public Transform DiscardPileTransform;
    public Transform DeckTransform;
    public Transform CardCreationPosition;
    public Transform TrashTransform;
    public Transform MousePos;
    public GameObject BlueprintCardPrefab, ActionCardPrefab;
    [Space(5)]
    [Header("Animation")]
    public AnimationCurve RotateReveal;
    public Action<CardBehaviour, bool> OnCardBeingSpendable;
    public enum CardPlace
    {
        Deck,
        Hand,
        Discard,
        Trash
    }

    bool drawCardCooldown = false;
    private float drawingCardTime = 1f;

    public List<CardBehaviour> Hand;
    private List<CardBehaviour> Deck, DiscardPile, TrashPile;
    private int drawCards = 0;
    private bool cardBeingDragged;
    Coroutine updatePositionsInHand;



    private void Start()
    {
        if (Deck == null)
            Deck = new List<CardBehaviour>();
        if (DiscardPile == null)
            DiscardPile = new List<CardBehaviour>();
        if (Hand == null)
            Hand = new List<CardBehaviour>();
        if (TrashPile == null)
            TrashPile = new List<CardBehaviour>();
        MoveCamera.Instance.OnCardDraggin += OnCardBeingDragged;
        OnCardBeingSpendable += CardBeingSpendable;
    }

    private void CardBeingSpendable(CardBehaviour card, bool isSpendable)
    {
        if (isSpendable)
            Hand.Remove(card);
        else
            Hand.Add(card);

        RearrangeCardsInHand(out var cardsOrder);
        UpdateCardsInHandPositions(cardsOrder);
    }

    void OnCardBeingDragged(bool dragged)
    {
        UpdateCardsInHandPositions();
        if (cardBeingDragged == dragged) return;
        cardBeingDragged = dragged;
        if (dragged)
        {
            //start dragging, very good
        }
        else
        {
            UpdateCardsInHandPositions();
        }
    }
    public void Update()
    {
        if (drawCards > 0 && drawCardCooldown == false)
        {
            drawCardCooldown = true;
            drawCards--;
            StartCoroutine(DrawCardAnimation(drawCards,()=> drawCardCooldown = false));
        }
        //TODO: optimization
        if (cardBeingDragged)
        {
            if (!RearrangeCardsInHand(out var cardsOrder))
            {
                Hand = cardsOrder;
                UpdateCardsInHandPositions();
            }
        }
    }
    public GameObject CreateCard(Card cardData)
    {
        GameObject card;
        if (cardData is ActionCard)
        {
            card = Instantiate(ActionCardPrefab,transform.parent);
            card.GetComponent<ActionCardBehaviour>().SetCardData(cardData as ActionCard);
            card.transform.SetPositionAndRotation(CardCreationPosition.position, CardCreationPosition.rotation);
            return card;
        }
        else if (cardData is BlueprintCard)
        {
            card = Instantiate(BlueprintCardPrefab, transform.parent);
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
                Deck.Add(card);
                Shuffle(CardPlace.Deck);
                break;
            case CardPlace.Hand:
                Hand.Add(card);
                break;
            case CardPlace.Discard:
                DiscardPile.Add(card);
                break;
            case CardPlace.Trash:
                TrashPile.Add(card);
                break;
            default:
                break;
        }
    }
    public void DrawCard(int amount = 1)
    {
        drawCards += amount;
    }
    IEnumerator DrawCardAnimation(float speedMultiplier = 1, Action callback = null)
    {
        if (Deck.Count <= 0)
        {
            //Shuffle discards into deck
            while (DiscardPile.Count > 0)
            {
                yield return ReturnDiscardToDeck(Pop(DiscardPile));
            }
        }
        var card = Pop(Deck);
        if (card == null)
            yield break;
        float t = 0f;
        if (speedMultiplier <= 1)
            speedMultiplier = 1;
        float timeToGetThere = 0.5f / speedMultiplier;
        float timeToRest = 0.5f / speedMultiplier;
        var originPos = card.transform.position;
        var originRot = card.transform.rotation;
        var posToGoTo = new Vector3(CardCreationPosition.position.x, CardCreationPosition.position.y + 300f, CardCreationPosition.position.z);
        var rotToGoTo = CardCreationPosition.rotation * Quaternion.Euler(0, 180, 0);
        while (t < timeToGetThere)
        {
            card.transform.SetPositionAndRotation(Vector3.Lerp(originPos, posToGoTo, t / timeToGetThere),
                Quaternion.LerpUnclamped(originRot, rotToGoTo, RotateReveal.Evaluate(t / timeToGetThere)));
            t += Time.deltaTime;
            yield return null;
        }
        card.transform.SetPositionAndRotation(posToGoTo, rotToGoTo);
        Hand.Add(card);
        yield return new WaitForSeconds(timeToRest);
        callback?.Invoke();
        UpdateCardsInHandPositions();
    }

    private CardBehaviour Pop(List<CardBehaviour> pile)
    {
        if (pile.Count == 0)
            return null;
        int index = pile.Count - 1;
        var card = pile[index];
        pile.RemoveAt(index);
        return card;
    }

    public void Shuffle(CardPlace place)
    {
        //TODO Shuffle animation
        GetStackByCardPlace(place).ShuffleList();
    }
    public Transform GetTransformByCardPlace(CardPlace place)
    {
        switch (place)
        {
            case CardPlace.Deck:
                return DeckTransform;
            case CardPlace.Hand:
                return HandTransform;
            case CardPlace.Discard:
                return DiscardPileTransform;
            case CardPlace.Trash:
                return TrashTransform;
            default:
                return transform;
        }
    }
    public List<CardBehaviour> GetStackByCardPlace(CardPlace place)
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
                return new List<CardBehaviour>();
        }
    }
    public void CreateStarterDeck()
    {
        CreateCardsWithAnimation(StarterDeck.StarterCards, ()=>
        {
            DrawCard(5);
            GameManager.Instance.OnStartGame?.Invoke();
        }, 3f);
       
    }
    public void CreateCardsWithAnimation(List<Card> Cards, Action callback = null, float timeMultiplier =1)
    {
        StartCoroutine(CreateMultipleCardsAnimation(Cards, callback, timeMultiplier));
    }
    public void CreateCardsWithAnimation(Card card, Action callback = null, float timeMultiplier =1)
    {
        var list = new List<Card>();
        list.Add(card);
        StartCoroutine(CreateMultipleCardsAnimation(list, callback, timeMultiplier));
    }

    IEnumerator CreateMultipleCardsAnimation(List<Card> Cards, Action callback, float timeMultiplier)
    {
        foreach (var item in Cards)
        {
            var card = CreateCard(item);
            yield return CreateAnimation(card, timeMultiplier);
            GainCard(card.GetComponent<CardBehaviour>(), CardPlace.Deck);
            
            StartCoroutine(MoveCardToDestination(card,CardPlace.Deck, timeMultiplier));
        }
        //we wait the amount of seconds MoveCardToDeck to time it all correct
        yield return new WaitForSeconds(0.5f);
        callback.Invoke();
        yield return null;
    }

    class CardFromToPos
    {
        public GameObject obj;
        public Quaternion originRot, rotToGoTo;
        public Vector3 originPos, posToGoTo;
        public void SetInfo(GameObject card, Quaternion rotTo, Vector3 posTo)
        {
            this.obj = card;
            originPos = card.transform.position;
            originRot = card.transform.rotation;
            rotToGoTo = rotTo;
            posToGoTo = posTo;
        }
    }
    /// <summary>
    /// arrange the cards in hand in according to the which is the most right
    /// </summary>
    bool RearrangeCardsInHand(out List<CardBehaviour> newArrange)
    {
        newArrange = Hand.OrderBy(x => -x.TransformX).ToList();
        return AreTheTwoListEqual(newArrange,Hand);
    }
    bool AreTheTwoListEqual(List<CardBehaviour> list1, List<CardBehaviour> list2)
    {
        if (list1.Count != list2.Count) return false;
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i].gameObject.GetInstanceID() != list2[i].gameObject.GetInstanceID())
            {
                return false;
            }
        }
        return true;
    }
    void UpdateCardsInHandPositions(List<CardBehaviour> newHand = null, Action callback = null)
    {
        List<CardFromToPos> positions = new List<CardFromToPos>();
        var hand = newHand;
        if (hand == null)
            hand = Hand;
        var maxCards = hand.Count;
        float HalfCards = maxCards * 0.5f;
        int i = 0;
        foreach (var item in hand)
        {
            if (item.IsReadyToBeSpend)
                continue;
            var animation = new CardFromToPos();
            if (!item.IsbeingDragged)
            {
                var pos = new Vector3((HalfCards - i) * 1.5f, 0, 0);
                animation.SetInfo(item.gameObject, HandTransform.rotation * Quaternion.Euler(4,90,90), HandTransform.position + pos);
            }
            positions.Add(animation);
            i++;
        }
        if(updatePositionsInHand != null)
            StopCoroutine(updatePositionsInHand);
        updatePositionsInHand = StartCoroutine(MoveCardsInHand(positions,callback));
    }

    IEnumerator MoveCardsInHand(List<CardFromToPos> animations, Action callback)
    {
        float t = 0f;
        float timeToGetThere = 0.25f;
      
        while (t < timeToGetThere)
        {
            foreach (var card in animations)
            {
                if (card.obj == null)
                    continue;
                card.obj.transform.SetPositionAndRotation(Vector3.Lerp(card.originPos, card.posToGoTo, t / timeToGetThere),
                    Quaternion.Lerp(card.originRot, card.rotToGoTo, t / timeToGetThere));
            }
            t += Time.deltaTime;
            yield return null;
        }
        foreach (var card in animations)
        {
            if (card.obj == null)
                continue;
            card.obj.transform.SetPositionAndRotation(Vector3.Lerp(card.originPos, card.posToGoTo, t / timeToGetThere),
                Quaternion.Lerp(card.originRot, card.rotToGoTo, t / timeToGetThere));
        }
        callback?.Invoke();
        yield return null;
    }

    IEnumerator CreateAnimation(GameObject card, float speedMultiplier = 1)
    {
        float t = 0f;
        float timeToEnlarge = 0.5f / speedMultiplier;
        float timeToRotate = 0.5f / speedMultiplier;
        float timeToRest = 0.1f / speedMultiplier;
        card.transform.SetPositionAndRotation(CardCreationPosition.position, CardCreationPosition.rotation);
        var posToGoTo = new Vector3(CardCreationPosition.position.x, CardCreationPosition.position.y + 300f, CardCreationPosition.position.z);
        while (t < timeToEnlarge)
        {
            card.transform.position = Vector3.Lerp(CardCreationPosition.position, posToGoTo, t / timeToEnlarge);
            t += Time.deltaTime;
            yield return null;
        }
        card.transform.position = posToGoTo;
        t = 0;
        // we end on 180, but we need to overshoot for animation value
        var rotationToGoTo = card.transform.rotation * Quaternion.Euler(0, 180, 0);
        while (t < timeToRotate)
        {
            card.transform.rotation = Quaternion.LerpUnclamped(CardCreationPosition.rotation, rotationToGoTo, RotateReveal.Evaluate(t / timeToRotate));
            t += Time.deltaTime;
            yield return null;
        }
        card.transform.rotation = rotationToGoTo;
        yield return new WaitForSeconds(timeToRest);
    }

    IEnumerator MoveCardToDestination(GameObject card, CardPlace destination, float timeMultiplier)
    {
        float timeToFlyToDeck = 0.5f / timeMultiplier;
        float t = 0f;
        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;
        Vector3 endPos = GetTransformByCardPlace(destination).position;
        Quaternion endRot = GetTransformByCardPlace(destination).rotation;
        while (t < timeToFlyToDeck)
        {
            card.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, t / timeToFlyToDeck), Quaternion.Lerp(startRot, endRot, t/timeToFlyToDeck));
            t += Time.deltaTime;
            yield return null;
        }
        card.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, 1), Quaternion.Lerp(startRot, endRot, 1));

        yield return null;
    }
    /// <summary>
    /// presumely from the hand
    /// </summary>
    public void DiscardCard(CardBehaviour card)
    {
        Hand.Remove(card);
        StartCoroutine(MoveCardToDestination(card.gameObject, CardPlace.Discard, 1));
        card.ResetCard();
        DiscardPile.Add(card);
    }
    IEnumerator ReturnDiscardToDeck(CardBehaviour card)
    {
        DiscardPile.Remove(card);
        yield return MoveCardToDestination(card.gameObject, CardPlace.Deck, 3f);
        Deck.Add(card);
    }

    public void GenerateTestCard()
    {
        var card = CreateCard(TestCard);

        card.transform.SetPositionAndRotation(new Vector3(0, 10, 0), Quaternion.identity);
    }
}
