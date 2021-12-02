using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : BookButton, IBookButton
{
    bool active = false;
    MeshRenderer meshRenderer;

    void OnValidate()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
    }
    public bool Active
    {
        get => active; 
        set
        {
            //TODO: animate it inactive
            StartCoroutine(LightUp(value));
            active = value;
        }
    }

    protected override void Start()
    {
        base.Start();
        Active = false;
        GameManager.Instance.OnStartGame += () => Active = true;
    }
    public void Activate()
    {
        if (!Active) return;
        Active = false;
        GameManager.Instance.EndTurn(()=> Active = true);
    }
    IEnumerator LightUp(bool on)
    {
        float t = on ? 0.5f : 1;
        //turn off
        while (!on && t > 0.5f)
        {
            meshRenderer.material.color = new Color(t, t, t, 1);
            t -= Time.deltaTime;
            yield return null;
        }
        //turn on
        while (on && t < 1)
        {
            meshRenderer.material.color = new Color(t, t, t, 1);
            t += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
