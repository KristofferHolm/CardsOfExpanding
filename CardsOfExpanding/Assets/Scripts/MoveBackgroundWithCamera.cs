using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBackgroundWithCamera : MonoBehaviour, PlayerController.IPlayerActions
{
    PlayerController controls;
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
    public void OnFire(InputAction.CallbackContext context)    {    }

    public void OnLook(InputAction.CallbackContext context)    {    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
    float range = 0.50f;
    float acceleration = 2f;
    Vector2 direction;
    Vector2 currentDir;
    Vector3 startPos;
    void Awake()
    {
        startPos = transform.position;
    }
    public void Update()
    {
        
        /**Try to fix jitter with no luck
        if (currentDir.x < direction.x)
            currentDir.x += acceleration * Time.deltaTime;
        else if (currentDir.x > direction.x)
            currentDir.x -= acceleration * Time.deltaTime;
        if (currentDir.y < direction.y)
            currentDir.y += acceleration * Time.deltaTime;
        else if (currentDir.y > direction.y)
            currentDir.y -= acceleration * Time.deltaTime;
        */

        currentDir = Vector2.Lerp(currentDir, direction, 0.05f);
        transform.position = startPos + new Vector3(-currentDir.x * range , 0, -currentDir.y * range );
    }
}
