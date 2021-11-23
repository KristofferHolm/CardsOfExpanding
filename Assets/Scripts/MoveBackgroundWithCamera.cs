using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBackgroundWithCamera : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField]  MoveCamera moveCamera;

    void OnValidate()
    {
        if (moveCamera == null)
            moveCamera = FindObjectOfType<MoveCamera>();
    }
    void Awake()
    {
        if (moveCamera == null)
            moveCamera = FindObjectOfType<MoveCamera>();
        startPos = transform.position;
        //currentDir = moveCamera.transform.position;
        moveCamera.CameraMovedDistance += UpdatePos;
    }

    private void UpdatePos(Vector3 obj)
    {
        transform.position = startPos - obj;
    }
    public void OnDisable()
    {
        moveCamera.CameraMovedDistance -= UpdatePos;
    }
 
}
