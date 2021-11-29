using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookActivateButton : BookButton, IBookButton
{
    public void Activate()
    {
        StartCoroutine(AnimateButtonClick(BookManager.Instance.ActivateAbility()));
    }
}
