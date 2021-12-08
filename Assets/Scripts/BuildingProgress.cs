using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProgress : MonoBehaviour
{
    public int BuildingId;
    public int NumberOfTurnsBeforeFinished;
    public Transform ScaffoldingTransform;
    private int moveNudges;
    public bool AbilityUsed;

    public string GetBookText
    {
        get
        {
            return $"Daily: (cost 1 Worker), finish building ({moveNudges} out of {NumberOfTurnsBeforeFinished})";
        }
    }

    /// <summary>
    /// returns true when building is finnished
    /// </summary>
    /// <param name="numberOfNudges"></param>
    /// <returns></returns>
    public bool MoveScaffoldingUpANudge(int numberOfNudges)
    {
        moveNudges += numberOfNudges;
        StopAllCoroutines();
        if (moveNudges >= NumberOfTurnsBeforeFinished)
        {
            return true;
        }
        StartCoroutine(MoveScaffoldingAnimation());
        return false;
    }
    IEnumerator MoveScaffoldingAnimation()
    {
        //ScaffoldingTransform
        while (ScaffoldingTransform.localPosition.y < 0.04f * moveNudges)
        {
            //shaking,
            var newPos = Shake();
            newPos.y += Time.deltaTime * 0.01f;
            ScaffoldingTransform.localPosition = newPos;
            yield return null;
        }
        ScaffoldingTransform.localPosition = new Vector3(0, moveNudges * 0.04f, 0);
        
        yield return null;
    }
    Vector3 Shake()
    {
        float x = Random.Range(-0.001f, 0.001f);
        float z = Random.Range(-0.001f, 0.001f);
        return new Vector3(x, ScaffoldingTransform.localPosition.y, z);
    }
}
