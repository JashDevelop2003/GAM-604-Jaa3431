using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// First Playable: This state is use to roll the dice for the player to move around based on the value of the roll
/// The player can also cancel their roll and move back to the deciding state if they prefer not to roll
/// </summary>

public class rollState : playerStateBase, IConfirm, ICancel
{
    //These are the booleans that will be use to change to a specifc state
    //rollDice is to change the current state to move state
    //rollCancel is to change back the current state to deciding state
    private bool rollDice;
    private bool rollCancel;
    
    //These are the variables that will be use to store the selected card from the deciding state
    private int minimumRoll;
    private int maximumRoll;
    private int manaCost;

    //The player controller is required to provide the roll value and decrease in mana for the player to move around
    //TODO: This also checks if the player can even roll with that dice
    private playerController controller;

    //The controls are use to either confirm rolling the dice or cancelling the roll to choose a different card
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }


    public override void EnterState(playerStateManager player)
    {
        //The booleans must stay false to ensure that the player cannot change instantly when enterting the roll state
        rollCancel = false;
        rollDice = false;

        //the controls enable confirm and cancel and will need the interfaces to provide the events
        controls = GetComponent<boardControls>();
        controller = GetComponent<playerController>();
        Controls.confirmPressed += ConfirmingChoice;
        Controls.cancelPressed += Cancel;
    }

    public override void UpdateState(playerStateManager player)
    {
        //Depending on which boolean is set to true first will change to the specifc state
        //if rollDice turns true before rollCancel then the current state will change to move state
        //otherwise if rollCancel turns true before RollDice then the current state will change to deciding state
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
        //When exiting this state, the control events must disable any method being listened to
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.cancelPressed -= Cancel;
    }

    //This method gathers the data from the selected card from the deciding state in order to provide a min and max roll value
    //This also gathers the mana cost to check if the player has enough mana to use the card and decrease equal to or more than 0
    public void CollectValue(int minValue, int maxValue, int Cost)
    {
        minimumRoll = minValue;
        maximumRoll = maxValue;
        manaCost = Cost;
    }

    //This interface method rolls the dice for the player if their mana is above or equal to the mana cost
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //if the player's current mana is equal to or above the mana cost
        //the roll a dice value between the minimum and maximum roll value
        if(controller.GetModel.CurrentMana >= manaCost)
        {
            controller.Roll(manaCost, UnityEngine.Random.Range(minimumRoll, maximumRoll+1));
            rollDice = true;
        }
        //Otherwise, inform the player they cannot use this card to roll and return them back to the deciding state
        else 
        {
            Debug.LogWarning("You don't have enough mana to roll");
            rollCancel = true;
        }
    }

    //This interface method returns the player back to the deciding state
    public void Cancel(object sender, EventArgs e)
    {
        rollCancel = true;
    }
}
