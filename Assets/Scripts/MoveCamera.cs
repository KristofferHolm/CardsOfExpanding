using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 /// <summary>
 /// also controls left and right click interaction, could maybe be moved at some point
 /// But MoveCamera is more like PlayerController god script 😀
 /// </summary>
public class MoveCamera : MonoBehaviour, PlayerController.IPlayerActions
{
    PlayerController controls;
    float speed = 0;
    float _minSpeed = 6f;
    float _maxSpeed = 16f;
    float _acceleration = 8f;
    float _deacceleration = 16f;
    float _zoomSpeed = 0.5f;
    bool startAcceleration = false;
    Vector2 direction;
    Vector2 currentDir;
    float zoom;
    public Action<Vector3> CameraMovedDistance;
    public Camera MainCamera, CardCamera;
    public LayerMask CardMask, Default;
    Vector2 mousePosition;
    bool draggingCard = false;
    Action<bool> OnCardDraggin;

    public void Update()
    {
        if(startAcceleration)
            speed = Mathf.Clamp(speed + _acceleration * Time.deltaTime, _minSpeed, _maxSpeed);
        else
            speed = Mathf.Clamp(speed - _deacceleration * Time.deltaTime, _minSpeed, _maxSpeed);
        Vector3 StartPos = transform.position;
       
        currentDir = Vector2.Lerp(currentDir, direction,0.1f);
        
        transform.position += new Vector3(currentDir.x * speed * Time.deltaTime, 0, currentDir.y * speed * Time.deltaTime);
        transform.position = ClampCameraInsideSpace(transform.position);

        if (transform.position.y < 3f && zoom > 0)
        {
            zoom = 0;
        }
        if (transform.position.y > 25f && zoom < 0)
        {
            zoom = 0;
        }
        if (Mathf.Abs(zoom) > 0.1f)
        {
            //todo: get the camera by class
            transform.position += transform.GetChild(0).forward * _zoomSpeed *zoom * Time.deltaTime;
            zoom *= 0.9f / (1 + Time.deltaTime);
        }
        else
            zoom = 0f;
        CameraMovedDistance.Invoke(transform.position - StartPos);
    }

    public Vector3 ClampCameraInsideSpace(Vector3 pos, float minX = -25, float maxX = 25, float minZ = -40, float maxZ = 20)
    {
        pos[0] = Mathf.Clamp(pos[0], minX, maxX);
        pos[2] = Mathf.Clamp(pos[2], minZ, maxZ);
        return pos;
    }

    private Component CheckMouseHit()
    {
        //we check cardcamera first because it is alway infront of the main camera.
        Ray ray = CardCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, 10000, CardMask))
        {
            return hit.transform.GetComponent<CardBehaviour>();
        }
        else 
        {
            ray = MainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray,out var hexHit, 10000, Default))
            {
                //do something with the hexfields
                return hexHit.transform.GetComponent<HexGridBehaviour>();
            }
        }
        return null;
    }

    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerController();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
        OnCardDraggin += (x) => draggingCard = x;
    }

    public void OnDisable()
    {
        controls.Player.Disable();
        OnCardDraggin -= (x) => draggingCard = x;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
            startAcceleration = true;
        else if (context.canceled)
            startAcceleration = false;

        direction = context.ReadValue<Vector2>();

    }
    public void OnZoom(InputAction.CallbackContext context)
    {
        zoom += context.ReadValue<float>() * Time.deltaTime;
    }

    public void OnLeftClick(InputAction.CallbackContext context)    
    {
        //doto drag mouse click

        if (context.started)
        {
            if (CheckMouseHit() is CardBehaviour)
                OnCardDraggin.Invoke(true);
        }
        else if (context.canceled)
            OnCardDraggin.Invoke(false);



        if (!context.performed) return;
            var hit = CheckMouseHit();
        if (hit is null)
            return;
        if (hit is HexGridBehaviour)
        {
            GetHexGridInfo(hit as HexGridBehaviour);
        }
        else if (hit is CardBehaviour)
        {

        }
    }

    private void GetHexGridInfo(HexGridBehaviour hexGrid)
    {
        BookManager.Instance.OpenBook("Hexgrid Type: " + hexGrid.Type.ToString(), "Building ID: " + hexGrid.BuildingId.ToString());
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        //performed is when the click is down and up, I think.
        if (!context.performed) return;
        BookManager.Instance.CloseBook();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        //todo, move camera around
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        if (draggingCard)
        {
            //move card with mouse
        }
    }
}
