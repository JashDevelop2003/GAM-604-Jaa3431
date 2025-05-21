using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStateManager : MonoBehaviour
{
    public event EventHandler beginEvent;
    
    [SerializeField] private gameStateBase currentState;
    [SerializeField] private gameStateBase minigameState;
    private ruleState ruleState;
    private gameInactiveState inactiveState;

    public gameStateBase MinigameState
    {
        get { return minigameState; }
        set { minigameState = value; }
    }

    public ruleState RuleState
    {
        get { return ruleState; }
    }

    public gameInactiveState InactiveState
    {
        get { return inactiveState; }
    }

    [Header ("Minigame Interface")]
    [SerializeField] private GameObject[] rulesUI = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        minigameState = GetComponent<gameStateBase>();
        ruleState = GetComponent<ruleState>();
        inactiveState = GetComponent<gameInactiveState>();
        currentState = inactiveState;
        currentState.EnterState(this);
    }

    public void StartMinigame(int minigameInt)
    {
        if (minigameInt == (int)minigameEnum.DoubleOrNothing)
        {
            minigameState = GetComponent<doubleOrNothingState>();
        }
        else if (minigameInt == (int)minigameEnum.XIII)
        {
            minigameState = GetComponent<xIIIInactiveState>();
        }
        else if (minigameInt == (int)minigameEnum.TicTacStash) 
        { 
            minigameState = GetComponent<ticTacStashState>();
        }
        ruleState.RulesPanel = rulesUI[minigameInt];
        beginEvent?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(gameStateBase newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
