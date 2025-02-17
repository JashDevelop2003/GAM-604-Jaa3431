using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO Tomorrow: Improve the States in order to be tested
public class startState : playerStateBase

{
    //the state manager collects the controls and logic in order for the controller to regain mana at the start of the turn
    private playerController controller;

    //this event occurs when the player turn starts which changes the state and ragain their mana
    public event EventHandler startingTurn;

    public override void EnterState(playerStateManager player)
    {
        controller = GetComponent<playerController>();
        startingTurn += controller.RegainMana;
    }

    public override void UpdateState(playerStateManager player)
    {
        startingTurn?.Invoke(this, EventArgs.Empty);
        player.ChangeState(player.DecidingState);
    }

    public override void ExitState(playerStateManager player)
    {
        startingTurn += controller.RegainMana;
        Debug.Log("Requires Status Effect (TODO: Next Stage)");
    }
}
