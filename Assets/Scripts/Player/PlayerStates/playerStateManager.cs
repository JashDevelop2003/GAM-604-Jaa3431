using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the finite state machine which starts off with the player being inactive
/// during the player's turn or combat the player will change their state that suits the certain situation
/// Each state must provide a enter, update and exit state to start, carry on and end their coding
/// </summary>
/// 
public class playerStateManager : MonoBehaviour
{
    //the state manager collects the controls and logic in the player object in order to call certain events for certain states
    private playerController controller;
    private boardControls controls;
    public boardControls Controls
    { 
        get { return controls; }
        set { controls = value; }
    
    }
    
    //this event occurs when the player turn starts which changes the state and ragain their mana
    public event EventHandler startTurn;


    //These are the states that each state is inherit from the player state base
    //each state must be created inside of the state manager which can be able to inherit a monobehaviour from the state manager
    private playerStateBase currentState;
    public inactiveState inactiveState = new inactiveState();
    public startState startState = new startState();
    public decidingState decidingState = new decidingState();

    
    // Start is called before the first frame update
    void Start()
    {
        controls =  GetComponent<boardControls>();
        controller = GetComponent<playerController>();

        //this begins the player in the inactive state where nothing happens until is the player's turn
        currentState = inactiveState;
        currentState.EnterState(this);
    }

    //This is called from the turn manager
    public void StartPlayerTurn()
    {
        //this checks if the player's current state is inactive.
        //If the current state isn't the inactive state then something went wrong
        if(currentState == inactiveState)
        {
            startTurn?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.LogError("Incorrect State Found");
        }
    }

    // Update is called once per frame and keeps updatubg the current state's update method
    void Update()
    {
        currentState.UpdateState(this);
    }

    //this method changes the states
    //this will occur once the update condition to swtich states is met in the current state
    //This will then call the exit state of the current state before changing to the new state
    public void ChangeState(playerStateBase newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        Debug.Log(newState.ToString());
        currentState.EnterState(this);
    }
}
