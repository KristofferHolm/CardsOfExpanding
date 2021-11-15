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
    float zoom;
    public void Update()
    {
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
            transform.position += transform.GetChild(0).forward * zoom * Time.deltaTime;
            zoom *= 0.9f / (1 + Time.deltaTime);
        }
        else
            zoom = 0f;
    }
    public Vector3 ClampCameraInsideSpace(Vector3 pos, float minX = -25, float maxX = 25, float minZ = -40, float maxZ = 20)
    {
        pos[0] = Mathf.Clamp(pos[0], minX, maxX);
        pos[2] = Mathf.Clamp(pos[2], minZ, maxZ);
        return pos;
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
    public void OnZoom(InputAction.CallbackContext context)
    {
        zoom += context.ReadValue<float>() * Time.deltaTime;
    }

    public void OnLook(InputAction.CallbackContext context)    {    }

    public void OnFire(InputAction.CallbackContext context)    {    }
}
