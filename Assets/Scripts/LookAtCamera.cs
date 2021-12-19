using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Start()
    {
        transform.rotation = GameManager.Instance.MainCamera.rotation * Quaternion.Euler(180,0,180);
    }
}
