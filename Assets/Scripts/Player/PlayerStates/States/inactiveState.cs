using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This state is about ending the player's turn
/// This should provide any negative status that they have and provide damage to them at the end of their turn
/// This should check whether it's their turn or they enter combat and provide the correct state
/// </summary>

public class inactiveState : playerStateBase
{
    //the booleans are used to change to a specfic state
    //beginTurn changes the current state to start state
    //TODO: beginCombat changes the current state to defend state
    private bool beginTurn;
    private bool beginCombat;

    //This is use in order to instant defeat the grim reaper if she's still in her last reapsort form
    playerController controller;

    //the enter state checks whether if the player ends their turn or their combat
    public override void EnterState(playerStateManager player)
    {
        //This is called to check if the player is the reaper and is on their last reapsort form
        //If it turns out the boolean is ture then the player is instantly defeated
        controller = GetComponent<playerController>();
        if(controller.GetModel.Character == characterEnum.Reaper)
        {
            lastReapsort instantDefeat = GetComponentInChildren<lastReapsort>();
            if (instantDefeat.LastReapsortActive)
            {
                controller.GetModel.IsAlive = false;
                Debug.Log("Player cannot defeat opponent successfully");
            }
        }

        //this enables the player turn method to start the player turn once their turn is over
        player.startTurn += PlayerTurn;


        beginCombat = false;

        //this checks if the previous state was an actual state (TODO: besides defend state) then end their turn
        if (player.PreviousState != null && player.PreviousState != player.DefendState)
        {
            controller.ActivateEndEffect();
            player.EndTurn();
            Debug.Log("Turn Ended");
        }
    }

    //this state checks when begin turn is turned to true, which will begin the player's turn
    public override void UpdateState(playerStateManager player)
    {
        if (beginTurn)
        {
            player.ChangeState(player.StartState);
        }
        if (beginCombat) 
        { 
            player.ChangeState(player.DefendState);
        }
        
    }

    //the boolean will become false & player turn will be disabled from the start turn event
    public override void ExitState(playerStateManager player)
    {
        beginTurn = false;
        player.startTurn -= PlayerTurn;
    }

    //This method waits until the start turn event invokes which makes begin turn become true
    public void PlayerTurn(object sender, EventArgs e)
    {
        beginTurn = true;
    }

    public void DefendCombat(object sender, EventArgs e) 
    {
        beginCombat = true;
    }
}
