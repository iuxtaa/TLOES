using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputsHandler : MonoBehaviour
{
    private Vector2 move;
    private bool interact;
    private bool continuePressed;

    private static InputsHandler instance;

    private void Awake()
    {
        instance = this;
        interact = false;
        continuePressed = false;
        move = Vector2.zero;
    }
    public static InputsHandler GetInstance()
    {
        return instance;
    }
    public Vector2 GetMove()
    { return move; }

    public void Movement(InputAction.CallbackContext action)
    {
        if(action.performed)
        {
            move = action.ReadValue<Vector2>();
        }
        else if(action.canceled) 
        {
            move = action.ReadValue<Vector2>();
        }
    }

    public void ContinueButtonPressed(InputAction.CallbackContext action)
    {
        if(action.performed)
        {
            continuePressed = true;
        }
        else if(action.canceled)
        {
            continuePressed = false;
        }
    }
    public bool GetContinuePressed()
    {
        bool endresult = continuePressed;
        continuePressed = false;
        return endresult;
    }

    public void InteractPressed(InputAction.CallbackContext action)
    {
        if (action.performed)
        {
            interact = true;
        }
        else if (action.canceled)
        {
            interact = false;
        }
    }
    public bool GetInteract()
    {
        bool endresult = interact;
        interact = false;
        return endresult ;
    }

}