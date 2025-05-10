using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// This script is about providing controls for each input with suitable events
/// This provides each script eventHandler which provides events to send to the observers
/// </summary>

public class boardControls : MonoBehaviour
{
    //These are the variables for the action map
    private PlayerControls playerControls;
    private InputAction selectUp;
    private InputAction selectDown;
    private InputAction selectLeft;
    private InputAction selectRight;
    private InputAction selectConfirm;
    private InputAction selectCancel;
    private InputAction useAbility;
    private InputAction reveal;

    //These are the Event Handlers which will allow other scripts to become observers for specifc events
    public event EventHandler upPressed;
    public event EventHandler downPressed;
    public event EventHandler leftPressed;
    public event EventHandler rightPressed;
    public event EventHandler confirmPressed;
    public event EventHandler cancelPressed;
    public event EventHandler useAbilityPressed;
    public event EventHandler revealPressed;

    
    /// The Awake method creates a new player control and provides suitable inputs from the action map
    void Awake()
    {
        //This create a new player controls for the input actions to reference in this action map
        playerControls = new PlayerControls();

        //Each Input Action reference the actions in the board controls action map in the input action
        selectUp = playerControls.boardControls.SelectUp;
        selectDown = playerControls.boardControls.SelectDown;
        selectRight = playerControls.boardControls.SelectRight;
        selectLeft = playerControls.boardControls.SelectLeft;
        selectConfirm = playerControls.boardControls.Confirm;
        selectCancel = playerControls.boardControls.Cancel;
        useAbility = playerControls.boardControls.UseAbility;
        reveal = playerControls.boardControls.RevealOffence;

        //This calls the method once an input is performed
        selectUp.performed += OnUpPressed;
        selectDown.performed += OnDownPressed;
        selectRight.performed += OnRightPressed;
        selectLeft.performed += OnLeftPressed;
        selectConfirm.performed += OnConfirmPressed;
        selectCancel.performed += OnCancelPressed;
        useAbility.performed += OnUseAbilityPressed;
        reveal.performed += OnRevealOffencePressed;
        
        
        //Each input action has to be enabled in order for the inputs to perform in game
        selectUp.Enable();
        selectDown.Enable();
        selectRight.Enable();
        selectLeft.Enable();
        selectConfirm.Enable();
        selectCancel.Enable();
        useAbility.Enable();
        reveal.Enable();
    }

    //These methods check for the button to be performed in which provides the events to call onPressed
    
    private void OnUpPressed(InputAction.CallbackContext up)
    {
        OnPressed(EventArgs.Empty, upPressed);
    }

    private void OnDownPressed(InputAction.CallbackContext down)
    {
        OnPressed(EventArgs.Empty, downPressed);
    }

    private void OnLeftPressed(InputAction.CallbackContext left)
    {
        OnPressed(EventArgs.Empty, leftPressed);
    }

    private void OnRightPressed(InputAction.CallbackContext right)
    {
        OnPressed(EventArgs.Empty, rightPressed);
    }

    private void OnConfirmPressed(InputAction.CallbackContext confirm)
    {
        OnPressed(EventArgs.Empty, confirmPressed);
    }

    private void OnCancelPressed(InputAction.CallbackContext cancel)
    {
        OnPressed(EventArgs.Empty, cancelPressed);
    }

    private void OnUseAbilityPressed(InputAction.CallbackContext useAbility)
    {
        OnPressed(EventArgs.Empty, useAbilityPressed);
    }

    private void OnRevealOffencePressed(InputAction.CallbackContext revealOffence)
    {
        OnPressed(EventArgs.Empty, revealPressed);
    }

    //OnPressed identifies the events inside of the event handler and invokes all methods that are listening
    private void OnPressed(EventArgs e, EventHandler input)
    {
        //This checks if there is anything to invoke
        //If there is then invoke the events and all oberver methods are called
        input?.Invoke(this, e);
    }
    
    //Disable ensures that all of the inputs are disabled if they're not necessary
    public void OnDisable()
    {
        selectUp.Disable();
        selectDown.Disable();
        selectRight.Disable();
        selectLeft.Disable();
        selectConfirm.Disable();
        selectCancel.Disable();
        useAbility.Disable();
        reveal.Disable();
    }


}
