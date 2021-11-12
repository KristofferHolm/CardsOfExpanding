using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour, PlayerController.IPlayerActions
{
    PlayerController controls;
    float speed = 32f;
    Vector2 direction;
    Vector2 currentDir;
    public void Update()
    {
        currentDir = Vector2.Lerp(currentDir, direction,0.1f);
        transform.position += new Vector3(currentDir.x * speed * Time.deltaTime, 0, currentDir.y * speed * Time.deltaTime);
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
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }



    public void OnLook(InputAction.CallbackContext context)    {    }

    public void OnFire(InputAction.CallbackContext context)    {    }
}
