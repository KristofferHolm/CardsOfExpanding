using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : MonoBehaviour
{
    public void Activate()
    {
        StartCoroutine(AnimateButtonClick(BookManager.Instance.ActivateAbility()));
    }
    IEnumerator AnimateButtonClick(bool activate)
    {
        if (activate)
        {
            //animate click
        }
        else
        {
            //animate not working like shake
        }
        yield return null;
    }
}
