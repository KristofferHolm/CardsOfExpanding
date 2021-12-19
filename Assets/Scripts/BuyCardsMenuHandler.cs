using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCardsMenuHandler : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
        GameManager.Instance.OpenCardsMenu += OpenAnimation;
    }

    public void CloseMenu()
    {
        animator.SetBool("Open", false);
    }

    private void OpenAnimation(bool open)
    {
        animator.SetBool("Open", open);
    }
}
