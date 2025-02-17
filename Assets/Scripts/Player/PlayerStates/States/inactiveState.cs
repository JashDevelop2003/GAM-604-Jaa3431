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

    private bool beginTurn;
    //private bool beginCombat; <--- This will be use during combat change the state to Defend

    public override void EnterState(playerStateManager player)
    {
        player.startTurn += PlayerTurn;
        
        if(player.PreviousState != null)
        {
            player.EndTurn();
            Debug.Log("Turn Ended");
        }
    }

    public override void UpdateState(playerStateManager player)
    {
        if (beginTurn)
        {
            player.ChangeState(player.StartState);
        }
        
        //TODO: Add a method that checks if the player has collided with the enemy and if they have then change the state to defend
    }

    public override void ExitState(playerStateManager player)
    {
        beginTurn = false;
        player.startTurn -= PlayerTurn;
    }

    public void PlayerTurn(object sender, EventArgs e)
    {
        beginTurn = true;
    }
}
