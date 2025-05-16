using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class xIIIInactiveState : gameStateBase
{
    //Boolean is used to identify when the player's next turn is
    //There is an encapsulation for the boolean for the XIII Manager to change one of the player's booleans to true.
    private bool startTurn;
    public bool StartingGame
    {
        get { return startTurn; }
        set { startTurn = value; }
    }

    //Game Manager is reference to add an observer to an event
    private xIIIManager xIIIManager;

    public override void EnterState(gameStateManager player)
    {
        //All booleans that change the states are set to false at the enter state
        startTurn = false;

        //The Singleton of the XIII Manager is reference to reference only one of them  in the scene
        //Start Turn observer is added onto the manger's change turn event
        xIIIManager = Singleton<xIIIManager>.Instance;
        xIIIManager.changeTurn += StartTurn;
    }

    public override void UpdateState(gameStateManager player)
    {
        //When changing states, the minigame state will change to the active state
        if (startTurn)
        {
            player.MinigameState = GetComponent<xIIIActiveState>();
            player.ChangeState(player.MinigameState);
        }
    }

    public override void ExitState(gameStateManager player)
    {
        xIIIManager.changeTurn -= StartTurn;
    }

    //The start turn obersver method will set the start turn boolean to true to change the state to the active state
    public void StartTurn(object sender, EventArgs e)
    {
        startTurn = true;
    }
}
