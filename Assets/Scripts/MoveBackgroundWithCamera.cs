using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBackgroundWithCamera : MonoBehaviour, PlayerController.IPlayerActions
{
    PlayerController controls;

    float range = 0.50f;
    Vector3 direction;
    Vector3 currentDir;
    Vector3 startPos;
    float zoom;
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
        currentDir = moveCamera.transform.position;
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
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }
 
    public void Update()
    {
        direction = currentDir - moveCamera.transform.position;
        transform.position = startPos + direction;
        currentDir = moveCamera.transform.position;
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

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoom += context.ReadValue<float>() * Time.deltaTime;
    }
    public void OnFire(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnMove(InputAction.CallbackContext context)
    {
        var vect2 = context.ReadValue<Vector2>();
        direction = new Vector3(vect2.x, 0, vect2.y);
    }
}
