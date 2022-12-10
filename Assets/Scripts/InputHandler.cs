using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, DefaultControls.IPlayer1Actions, DefaultControls.IPlayer2Actions
{

    public Vector2 player1CursorDelta;
    public Vector2 player2CursorDelta;

    public Action OnP1Cover;
    public Action OnP1Fire;

    public Action OnP2Cover;
    public Action OnP2Fire;

    DefaultControls controls;

    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new DefaultControls();
        controls.Player1.SetCallbacks(this);
        controls.Player1.Enable();
        controls.Player2.SetCallbacks(this);
        controls.Player2.Enable();
    }

    private void OnDisable()
    {
        controls.Player1.Disable();
        controls.Player2.Disable();
    }

    public void OnP1_ToggleCover(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnP1Cover.Invoke();
    }

    public void OnP1_Aim(InputAction.CallbackContext context)
    {
        player1CursorDelta = context.ReadValue<Vector2>();
    }

    public void OnP1_Fire(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnP1Fire.Invoke();
    }

    public void OnP2_ToggleCover(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnP2Cover.Invoke();
    }

    public void OnP2_Aim(InputAction.CallbackContext context)
    {
        player2CursorDelta = context.ReadValue<Vector2>();
    }

    public void OnP2_Fire(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnP2Fire.Invoke();
    }
}
