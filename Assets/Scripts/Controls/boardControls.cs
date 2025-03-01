using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private InputAction defendUp;
    private InputAction defendDown;
    private InputAction defendLeft;
    private InputAction defendRight;
    private InputAction defendConfirm;
    private InputAction useAbility;

    //These are the Event Handlers which will allow other scripts to become observers for specifc events
    public event EventHandler upPressed;
    public event EventHandler downPressed;
    public event EventHandler leftPressed;
    public event EventHandler rightPressed;
    public event EventHandler confirmPressed;
    public event EventHandler cancelPressed;
    public event EventHandler defendUpPressed;
    public event EventHandler defendDownPressed;
    public event EventHandler defendLeftPressed;
    public event EventHandler defendRightPressed;
    public event EventHandler defendConfirmPressed;
    public event EventHandler useAbilityPressed;

    
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
        defendUp = playerControls.boardControls.DefendUp;
        defendDown = playerControls.boardControls.DefendDown;
        defendLeft = playerControls.boardControls.DefendLeft;
        defendRight = playerControls.boardControls.DefendRight;
        defendConfirm = playerControls.boardControls.DefendConfirm;
        useAbility = playerControls.boardControls.UseAbility;

        //This calls the method once an input is performed
        selectUp.performed += OnUpPressed;
        selectDown.performed += OnDownPressed;
        selectRight.performed += OnRightPressed;
        selectLeft.performed += OnLeftPressed;
        selectConfirm.performed += OnConfirmPressed;
        selectCancel.performed += OnCancelPressed;
        defendUp.performed += OnDefendUpPressed;
        defendDown.performed += OnDefendDownPressed;
        defendLeft.performed += OnDefendLeftPressed;
        defendRight.performed += OnDefendRightPressed;
        defendConfirm.performed += OnDefendConfirmPressed;
        useAbility.performed += OnUseAbilityPressed;
        
        
        //Each input action has to be enabled in order for the inputs to perform in game
        selectUp.Enable();
        selectDown.Enable();
        selectRight.Enable();
        selectLeft.Enable();
        selectConfirm.Enable();
        selectCancel.Enable();
        defendUp.Enable();
        defendDown.Enable();
        defendLeft.Enable();
        defendRight.Enable();
        defendConfirm.Enable();
        useAbility.Enable();
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

    private void OnDefendUpPressed(InputAction.CallbackContext defendUp) 
    { 
        OnPressed(EventArgs.Empty, defendUpPressed);
    }

    private void OnDefendDownPressed(InputAction.CallbackContext defendDown)
    {
        OnPressed(EventArgs.Empty, defendDownPressed);
    }

    private void OnDefendLeftPressed(InputAction.CallbackContext defendLeft)
    {
        OnPressed(EventArgs.Empty, defendLeftPressed);
    }

    private void OnDefendRightPressed(InputAction.CallbackContext defendRight)
    {
        OnPressed(EventArgs.Empty, defendRightPressed);
    }

    private void OnDefendConfirmPressed(InputAction.CallbackContext defendConfirm)
    {
        OnPressed(EventArgs.Empty, defendConfirmPressed);
    }

    private void OnUseAbilityPressed(InputAction.CallbackContext useAbility)
    {
        OnPressed(EventArgs.Empty, useAbilityPressed);
    }

    //OnPressed identifies the events inside of the event handler and invokes all methods that are listening
    private void OnPressed(EventArgs e, EventHandler input)
    {
        //This checks if there is anything to invoke
        //If there is then invoke the events
        //Otherwise provide an error mentioning: There is nothing to invoke
        if (input != null)
        {
            input.Invoke(this, e); 
        }
        else
        {
            Debug.LogError("There is Nothing to Invoke");
        }
    }
    
    //Disable ensures that all of the inputs are disabled if they're not necessary
    public void Disable()
    {
        selectUp.Disable();
        selectDown.Disable();
        selectRight.Disable();
        selectLeft.Disable();
        selectConfirm.Disable();
        selectCancel.Disable();
        defendUp.Disable();
        defendDown.Disable();
        defendLeft.Disable();
        defendRight.Disable();
        defendConfirm.Disable();
        useAbility.Disable();
    }


}
