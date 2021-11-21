using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : Singleton<BookManager>
{
    [SerializeField] Animator bookAnimator;
    private void OnValidate()
    {
        if(!bookAnimator)
            bookAnimator = GetComponent<Animator>();
    }
    bool open = false;
    private void Start()
    {
        
    }
    private void Update()
    {
       
    }
    public void OpenBook()
    {
        bookAnimator.SetBool("Open",true);

    }
    public void CloseBook()
    {
        bookAnimator.SetBool("Open", false);
    }
}
