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

    
    //this event occurs when the player turn starts which changes the state and ragain their mana
    public event EventHandler startTurn;


    //These are the states that each state is inherit from the player state base
    //each state must be created inside of the state manager which can be able to inherit a monobehaviour from the state manager
    //These are the current and previous state, this check for the current state the previous transition
    [SerializeField] private playerStateBase currentState;
    [SerializeField] private playerStateBase previousState;

    public playerStateBase CurrentState
    {

        get { return currentState; } 
    
    }

    //These are all the states that exist and is use to change around
    private inactiveState inactiveState;
    private startState startState;
    private decidingState decidingState;
    private targetState targetState;
    private rollState rollState;
    private moveState moveState;
    private choosingState choosingState;
    private pickingState pickingState;
    private attackState attackState;
    private defendState defendState;
    private itemState itemState;
    private cursingState cursingState;
    private marketState marketState;
    private spinState spinState;

    //These are to call the states in order for the states to change
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

    public targetState TargetState 
    { 
        get { return targetState; } 
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

    public pickingState PickingState
    {
        get { return pickingState; }
    }

    public attackState AttackState 
    { 
        get { return attackState; } 
    }

    public defendState DefendState 
    { 
        get { return defendState; } 
    }

    public itemState ItemState
    {
        get { return itemState; }
    }

    public cursingState CursingState
    {
        get { return cursingState; }
    }

    public marketState MarketState
    {
        get { return marketState; } 
    }
    
    public spinState SpinState
    {
        get { return spinState; }
    }

    [Header("Animation")]
    private stateAnimation stateAnimation;

    // Start is called before the first frame update
    void Start()
    {
        //This collect the component required to start the turn and each of the states
        inactiveState = GetComponent<inactiveState>();
        startState = GetComponent<startState>();
        decidingState = GetComponent<decidingState>();
        targetState = GetComponent<targetState>();
        rollState = GetComponent<rollState>();
        moveState = GetComponent<moveState>();
        choosingState = GetComponent<choosingState>();
        pickingState = GetComponent<pickingState>();
        attackState = GetComponent<attackState>();
        defendState = GetComponent<defendState>();
        itemState = GetComponent<itemState>();
        cursingState = GetComponent<cursingState>();
        marketState = GetComponent<marketState>();
        spinState = GetComponent<spinState>();

        //this begins the player in the inactive state where nothing happens until is the player's turn
        currentState = inactiveState;
        currentState.EnterState(this);

        stateAnimation = GetComponentInChildren<stateAnimation>();
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

    // Update is called once per frame and keeps updating the current state's update method
    // In this scenario Update is use to call once per frame in the current state's update state
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
        currentState = newState;
        currentState.EnterState(this);
        stateAnimation.ChangeAnimationState(newState);
    }

    //This ends the turn of the player by calling the turn manager instance to change to the next turn
    public void EndTurn()
    {

        turnManager turnManager = Singleton<turnManager>.Instance;
        turnManager.StartTurn();
    }
}
