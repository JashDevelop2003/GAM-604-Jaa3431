using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class decidingState : playerStateBase, IDecideDown, IDecideUp, IDecideRight, IDecideLeft, IConfirm

{
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private movementDeckPile movementDeck;
    public movementDeckPile MovementDeck
    {
        get { return movementDeck; }
    }

    private GameObject selectedCard;

    private int minRoll;
    private int maxRoll;
    private int manaCost;

    private bool hasSelected;

    public override void EnterState(playerStateManager player)
    {
        hasSelected = false;
        selectedCard = null;
        
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        movementDeck = GetComponentInChildren<movementDeckPile>();

        if(player.PreviousState == player.StartState)
        {
            movementDeck.DrawCards();
        }

    }

    public override void UpdateState(playerStateManager player)
    {
        if (hasSelected)
        {
            Debug.Log(selectedCard.name);
            movementCard moveCard = selectedCard.GetComponent<movementCard>();
            minRoll = moveCard.RollMinimumValue;
            maxRoll = moveCard.RollMaximumValue;
            manaCost = moveCard.ManaCost;
            player.ChangeState(player.RollState);
        }
        
    }

    public override void ExitState(playerStateManager player)
    {
        rollState Rolling = player.RollState.GetComponent<rollState>();
        Rolling.CollectValue(minRoll, maxRoll, manaCost);

        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

    public void DecidingUp(object sender, EventArgs e)
    {
        selectedCard = movementDeck.SelectedCards[1];
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedCard = null;
        Debug.LogWarning("Needs to Implement Movement Card");
    }

    public void DecidingLeft(object sender, EventArgs e)
    {       
        selectedCard = movementDeck.SelectedCards[0];
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedCard = movementDeck.SelectedCards[2];
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedCard != null)
        {
            hasSelected = true;
        }
        else
        {
            Debug.LogError("The Card has not been selected");
        }
    }
}
