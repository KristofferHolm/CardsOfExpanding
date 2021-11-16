using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBackgroundWithCamera : MonoBehaviour
{
    PlayerController controls;
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
 
    public void Update()
    {
        //direction = currentDir - moveCamera.transform.position;
        //transform.position = startPos + direction;
        //currentDir = moveCamera.transform.position;

        //direction.y = -zoom;
        //currentDir = Vector3.Lerp(currentDir, direction, 0.05f);
        //transform.position = startPos - currentDir * range;
        //if (Mathf.Abs(zoom) > 0.1f)
        //{
        //    zoom *= 0.9f / (1 + Time.deltaTime);
        //}
        //else
        //    zoom = 0f;
    }

  
}
