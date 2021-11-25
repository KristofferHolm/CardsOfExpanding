using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : MonoBehaviour
{
    public void Activate()
    {
        BookManager.Instance.ActivateAbility();
    }
}
