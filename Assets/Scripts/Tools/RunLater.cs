using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunLater : Singleton<RunLater>
{
    public void EndOfFrame(Action callback)
    {
        StartCoroutine(EndOfFrameCoroutine(callback));
    }
    IEnumerator EndOfFrameCoroutine(Action callback)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("calling end of frame function");
        callback.Invoke();
        yield return null;
    }
}
