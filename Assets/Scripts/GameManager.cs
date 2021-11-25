using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action OnNewTurn;
    public void NewTurn()
    {

        OnNewTurn.Invoke();
    }

}
