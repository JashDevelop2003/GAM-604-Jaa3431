using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// First Playable: The Start State is use to regain mana for the player
/// </summary>
public class startState : playerStateBase

{
    //this is to collect the controller's Regain mana method inside the controller.
    private playerController controller;

    //This gets the effects to see if player is stunned
    private currentEffects effects;

    public event EventHandler startEvent;
    public event EventHandler startItemEvents;

    public override void EnterState(playerStateManager player)
    {
        controller = GetComponent<playerController>();
        effects = GetComponent<currentEffects>();
        startEvent += controller.ResetStats;
        startEvent += controller.ActivateStartEffects;

        startEvent?.Invoke(this, EventArgs.Empty);
        startItemEvents?.Invoke(this, EventArgs.Empty);
    }

    public override void UpdateState(playerStateManager player)
    {
        if (effects.Stunned) 
        {
            player.ChangeState(player.InactiveState);
        }
        else
        {
            //This changes the state immediately to the deciding state
            player.ChangeState(player.DecidingState);
        }
    }

    public override void ExitState(playerStateManager player)
    {
        startEvent -= controller.ResetStats;
        startEvent -= controller.ActivateStartEffects;

        //If the player's character is wielder then decrement the cooldown & see if the character changes state
        if (controller.GetModel.Character == characterEnum.Wielder) 
        {
            controller.ActivatePassive();
        }

    }
}
