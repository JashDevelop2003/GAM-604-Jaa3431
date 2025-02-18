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
    //the state manager collects the controls and logic in order for the controller to regain mana at the start of the turn
    private playerController controller;

    
    //this event occurs when the player turn starts which changes the state and ragain their mana
    public event EventHandler startTurn;


    //These are the states that each state is inherit from the player state base
    //each state must be created inside of the state manager which can be able to inherit a monobehaviour from the state manager
   [SerializeField] private playerStateBase currentState;
    private playerStateBase previousState;
    private inactiveState inactiveState;
    private startState startState;
    private decidingState decidingState;
    private rollState rollState;
    private moveState moveState;
    private choosingState choosingState;

    public playerStateBase PreviousState
    {
        get { return previousState; }
    }
    
    public inactiveState InactiveState
    {
        get { return inactiveState; }
    }

    public startState StartState
    {
        get { return startState; }
    }

    public decidingState DecidingState
    {
        get { return decidingState; }
    }

    public rollState RollState
    {
        get { return rollState; }
    }

    public moveState MoveState
    {
        get { return moveState; }
    }

    public choosingState ChoosingState
    {
        get { return choosingState; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //This collect the component required to start the turn and each of the states
        //TODO: Add Choosing, Selecting, Attack & Defend States
        controller = GetComponent<playerController>();
        inactiveState = GetComponent<inactiveState>();
        startState = GetComponent<startState>();
        decidingState = GetComponent<decidingState>();
        rollState = GetComponent<rollState>();
        moveState = GetComponent<moveState>();
        choosingState = GetComponent<choosingState>();

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
            Debug.Log(gameObject.name + " 's turn");
            startTurn?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.LogError("Incorrect State Found");
        }
    }

    // Update is called once per frame and keeps updating the current state's update method
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
        previousState = currentState;
        Debug.Log("Previous State: " + previousState.ToString());
        currentState = newState;
        Debug.Log("Current State: " + currentState.ToString());
        currentState.EnterState(this);
    }

    public void EndTurn()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        turnManager turnManager = manager.GetComponent<turnManager>();

        Debug.Log(turnManager);
        //turnManager.StartTurn();
    }
}
