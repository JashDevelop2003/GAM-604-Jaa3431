using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollState : playerStateBase, IConfirm, ICancel
{
    private bool rollDice;
    private bool rollCancel;
    
    private int minimumRoll;
    private int maximumRoll;
    private int manaCost;

    private playerController controller;

    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }


    public override void EnterState(playerStateManager player)
    {
        rollCancel = false;
        rollDice = false;
        controls = GetComponent<boardControls>();
        controller = GetComponent<playerController>();
        Controls.confirmPressed += ConfirmingChoice;
        Controls.cancelPressed += Cancel;
    }

    public override void UpdateState(playerStateManager player)
    {
        if (rollDice) 
        {
            player.ChangeState(player.MoveState);
        }
        
        if (rollCancel)
        {
            player.ChangeState(player.DecidingState);
        }
    }

    public override void ExitState(playerStateManager player)
    {
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.cancelPressed -= Cancel;
    }

    public void CollectValue(int minValue, int maxValue, int Cost)
    {
        minimumRoll = minValue;
        maximumRoll = maxValue;
        manaCost = Cost;
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if(controller.GetModel.CurrentMana >= manaCost)
        {
            controller.Roll(manaCost, UnityEngine.Random.Range(minimumRoll, maximumRoll+1));
            rollDice = true;
        }
        else 
        {
            Debug.LogWarning("You don't have enough mana to roll");
            rollCancel = true;
        }
    }

    public void Cancel(object sender, EventArgs e)
    {
        rollCancel = true;
    }
}
