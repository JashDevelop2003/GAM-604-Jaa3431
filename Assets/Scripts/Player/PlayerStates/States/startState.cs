using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO Tomorrow: Improve the States in order to be tested
public class startState : playerStateBase

{
    //this is to collect the controller's Regain mana method inside the controller.
    private playerController controller;

    public override void EnterState(playerStateManager player)
    {
        controller = GetComponent<playerController>();
    }

    public override void UpdateState(playerStateManager player)
    {
        player.ChangeState(player.DecidingState);
    }

    public override void ExitState(playerStateManager player)
    {
        controller.RegainMana();
        Debug.Log("Requires Status Effect (TODO: Next Stage)");
    }
}
