using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookManager : Singleton<BookManager>
{
    [SerializeField] Animator bookAnimator;
    public TextMeshPro RightPage, LeftPage;
    private bool isOpen = false;
    private string rightText, leftText;
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

    public void OpenBook(string leftPage, string rightPage)
    {
        rightText = rightPage;
        leftText = leftPage;
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
}
