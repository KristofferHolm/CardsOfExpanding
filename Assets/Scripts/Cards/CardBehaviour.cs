using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardBehaviour : MonoBehaviour
{
    List<TextMeshPro> texts;
    [SerializeField] private Transform TextParent;
    [SerializeField] private MeshRenderer Icon;
    private bool _isReadyToBeSpend;
    public bool IsReadyToBeSpend
    {
        get
        {
            return _isReadyToBeSpend;
        }
    }
    public float TransformX
    {
        get
        {
            return transform.position.x;
        }
    }

    bool _isBeingDragged;
    public bool IsbeingDragged
    {
        get
        {
            return _isBeingDragged;
        }
        set
        {
            _isBeingDragged = value;
        }
    }

    private void Update()
    {
        if (!_isBeingDragged)
        {
            ReturnToNormalSize();
            return;
        }
        transform.position = Vector3.Lerp(transform.position, CardManager.Instance.MousePos.position, 0.1f);
        CheckIfCardIsReadyToBeSpendable(CardManager.Instance.MousePos.position.z > -4f);
        var size = Mathf.Clamp01(transform.localScale.z + (_isReadyToBeSpend ? -Time.deltaTime * 4f : Time.deltaTime * 4f));
        transform.localScale = new Vector3(1, 1, size);
    }
    private void ReturnToNormalSize()
    {
        if (transform.localScale.z < 1)
        {
            transform.localScale = new Vector3(1, 1, Mathf.Clamp01(transform.localScale.z + Time.deltaTime * 4f));
        }
    }
    private void CheckIfCardIsReadyToBeSpendable(bool spendable)
    {
        if (_isReadyToBeSpend == spendable) return;
        //TODO: Check if the player has enough resources


        _isReadyToBeSpend = spendable;
        CardManager.Instance.OnCardBeingSpendable?.Invoke(this,spendable);
    }

    protected void SetIcon(Texture icon)
    {
        Icon.sharedMaterial.mainTexture = icon;
    }

    protected void SetText(string[] textsToSet)
    {
        if (!TextParent)
        {
            Debug.LogError("Transform TextParent is Null");
            return;
        }
        if (texts == null)
        {
            texts = new List<TextMeshPro>();
            foreach (var tmp in TextParent.GetComponentsInChildren<TextMeshPro>())
            {
                texts.Add(tmp);
            }
        }
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = textsToSet[i]; 
        }
    }
}
