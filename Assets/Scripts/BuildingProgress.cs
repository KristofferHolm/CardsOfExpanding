using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProgress : MonoBehaviour
{
    public int NumberOfTurnsBeforeFinished;
    public Transform ScaffoldingTransform;
    private int moveNudges = 0;
    public void MoveScaffoldingUpANudge(int numberOfNudges)
    {
        moveNudges += numberOfNudges;
        StopAllCoroutines();

        StartCoroutine(MoveScaffoldingAnimation());
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
        float x = Random.Range(-0.01f, 0.01f);
        float z = Random.Range(-0.01f, 0.01f);
        return new Vector3(x, 0, z);
    }
}
