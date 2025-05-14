using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameControls : MonoBehaviour
{
    private MinigameControls minigameControls;
    private InputAction chooseUp;
    private InputAction chooseDown;
    private InputAction chooseLeft;
    private InputAction chooseRight;
    private InputAction confirmChoice;
    private InputAction rules;

    //events that allow observers to call from these events
    public event EventHandler pressedUp;
    public event EventHandler pressedDown;
    public event EventHandler pressedLeft;
    public event EventHandler pressedRight;
    public event EventHandler pressedConfirm;
    public event EventHandler pressedRules;

    //This method will create a new minigame controls in order for the input actions to be stored in a script component
    void Awake()
    {
        minigameControls = new MinigameControls();
        chooseUp = minigameControls.GameControls.ChooseUp;
        chooseDown = minigameControls.GameControls.ChooseDown;
        chooseLeft = minigameControls .GameControls.ChooseLeft;
        chooseRight = minigameControls.GameControls.ChooseRight;
        confirmChoice = minigameControls.GameControls.ConfirmChoice;
        rules = minigameControls .GameControls.Rules;

        //This calls the method once an input is performed
        chooseUp.performed += OnUpPressed;
        chooseDown.performed += OnDownPressed;
        chooseLeft.performed += OnLeftPressed;
        chooseRight.performed += OnRightPressed;
        confirmChoice.performed += OnConfirmPressed;
        rules.performed += OnRulesPressed;


        //Each input action has to be enabled in order for the inputs to perform in game
        chooseUp.Enable();
        chooseDown.Enable();
        chooseLeft.Enable();
        chooseRight.Enable();
        confirmChoice.Enable();
        rules.Enable();
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

    private void OnRulesPressed(InputAction.CallbackContext rules) 
    {
        OnPressed(EventArgs.Empty, pressedRules);
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
        chooseUp.Disable();
        chooseDown.Disable();
        chooseLeft.Disable();
        chooseRight.Disable();
        confirmChoice.Disable();
        rules.Disable();
    }
}
