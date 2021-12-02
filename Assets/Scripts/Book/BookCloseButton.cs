using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCloseButton : BookButton, IBookButton
{
    public void Activate()
    {
        BookManager.Instance.CloseBook();
    }
}
