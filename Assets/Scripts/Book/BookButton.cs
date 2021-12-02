using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : MonoBehaviour
{
    Vector3 originPos;

   
    protected virtual void Start()
    {
        originPos = transform.localPosition;
    }
    public void ClickDown()
    {
        StartCoroutine(MouseOver());
        
    }
  
    protected IEnumerator MouseOver()
    {
        transform.localPosition = originPos + Vector3.forward * 0.05f;
        while (MoveCamera.Instance.MouseIsOver(transform))
        {
            yield return null;
        }
        transform.localPosition = originPos;
    }

    protected IEnumerator AnimateButtonClick(bool activate)
    {
        float t = 0;
        float timeToAnimate = 0.25f;
        GetComponent<BoxCollider>().enabled = false;
      
        if (activate)
        {
            while (t < timeToAnimate)
            {
                var d = Mathf.Sin((t / timeToAnimate) * Mathf.PI);
                transform.localPosition= originPos + Vector3.forward * d * 0.2f;
                t += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = originPos;
        }
        else
        {
            while (t < timeToAnimate)
            {
                var d = Mathf.Sin((t*3 / timeToAnimate) * Mathf.PI);
                transform.localPosition = originPos - Vector3.up * d * 0.3f;
                t += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = originPos;
        }
        GetComponent<BoxCollider>().enabled = true;
        yield return null;
    }
}
public interface IBookButton
{
    void Activate();
}