using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action OnNewTurn;
    public Action OnEndTurn;
    public Action OnStartGame;

    private void Start()
    {
        //because of the town hall, I'll grant 3 workers from start.
        //but this should be changed later, I think
        InventoryManager.Instance.Workers = 3;
    }

    public void EndTurn(Action callback)
    {
        // go through end step
        // draw card
        // start uptime > update all buildings
        StartCoroutine(EndTurnAnimation(callback));
    }
    IEnumerator EndTurnAnimation(Action callback)
    {
        // TODO; Make some animation of end turn
        OnEndTurn?.Invoke();
        yield return new WaitForSeconds(0.2f);
        CardManager.Instance.DrawCard();
        yield return new WaitForSeconds(0.2f);
        OnNewTurn?.Invoke();
        yield return new WaitForSeconds(0.2f);
        callback?.Invoke();
    }

}
