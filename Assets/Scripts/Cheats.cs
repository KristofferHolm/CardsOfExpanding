using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cheats : MonoBehaviour, PlayerController.ICheatsActions
{
    PlayerController controls;
    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerController();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.Cheats.SetCallbacks(this);
        }
        controls.Cheats.Enable();
    }
    void OnDisable()
    {
        controls.Cheats.Disable();
    }

    public void OnCheat(InputAction.CallbackContext context)
    {
        if (context.performed)
            InventoryManager.Instance.GainResources(1, 1, 1, 0);
    }

}
