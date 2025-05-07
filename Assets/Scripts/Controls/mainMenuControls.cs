using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class mainMenuControls : MonoBehaviour
{
    //The controls of the action map and the input actions included
    private MenuControls menuControls;
    private InputAction navigateUp;
    private InputAction navigateDown;
    private InputAction navigateLeft;
    private InputAction navigateRight;
    private InputAction navigateConfirm;

    //events that allow observers to call from these events
    public event EventHandler pressedUp;
    public event EventHandler pressedDown;
    public event EventHandler pressedLeft;
    public event EventHandler pressedRight;
    public event EventHandler pressedConfirm;

    private void Awake()
    {
        menuControls = new MenuControls();
        navigateUp = menuControls.mainMenuControls.NavUp;
        navigateDown = menuControls.mainMenuControls.NavDown;
        navigateLeft = menuControls.mainMenuControls.NavLeft;
        navigateRight = menuControls.mainMenuControls.NavRight;
        navigateConfirm = menuControls.mainMenuControls.Confirm;

        //This calls the method once an input is performed
        navigateUp.performed += OnUpPressed;
        navigateDown.performed += OnDownPressed;
        navigateLeft.performed += OnRightPressed;
        navigateRight.performed += OnLeftPressed;
        navigateConfirm.performed += OnConfirmPressed;

        //Each input action has to be enabled in order for the inputs to perform in game
        navigateUp.Enable();
        navigateDown.Enable();
        navigateLeft.Enable();
        navigateRight.Enable();
        navigateConfirm.Enable();
    }

    private void OnUpPressed(InputAction.CallbackContext up)
    {
        OnPressed(EventArgs.Empty, pressedUp);
    }

    private void OnDownPressed(InputAction.CallbackContext down)
    {
        OnPressed(EventArgs.Empty, pressedDown);
    }

    private void OnLeftPressed(InputAction.CallbackContext left)
    {
        OnPressed(EventArgs.Empty, pressedLeft);
    }

    private void OnRightPressed(InputAction.CallbackContext right)
    {
        OnPressed(EventArgs.Empty, pressedRight);
    }

    private void OnConfirmPressed(InputAction.CallbackContext confirm)
    {
        OnPressed(EventArgs.Empty, pressedConfirm);
    }

    //OnPressed identifies the events inside of the event handler and invokes all methods that are listening
    private void OnPressed(EventArgs e, EventHandler input)
    {
        //This checks if there is anything to invoke
        //If there is then invoke the events and all methods are called
        input?.Invoke(this, e);
    }

    //Disable ensures that all of the inputs are disabled if they're not necessary
    private void OnDisable()
    {
        navigateUp.Disable();
        navigateDown.Disable();
        navigateLeft.Disable();
        navigateRight.Disable();
        navigateConfirm.Disable();
    }
}
