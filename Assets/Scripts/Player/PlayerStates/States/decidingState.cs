using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// First Playable: The Deciding State is to have the player select a movement card to choose for rolling
/// </summary>

public class decidingState : playerStateBase, IDecideDown, IDecideUp, IDecideRight, IDecideLeft, IConfirm

{
    //This controls will be use for providing inputs of deciding the movement card
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    //This will provide the cards to collect from the deck in the child of the player object
    private movementDeckPile movementDeck;
    public movementDeckPile MovementDeck
    {
        get { return movementDeck; }
    }

    //this will be use to send to the roll state what card was selected to roll
    private GameObject selectedCard;

    //These values will be converted to the roll state to roll the minimum and maximum value along with using suitable amount of mana
    private int minRoll;
    private int maxRoll;
    private int manaCost;

    //This boolean checks when the player has selected and can move onto the roll state
    private bool hasSelected;

    public override void EnterState(playerStateManager player)
    {
        //The hasSelected boolean stays false when entering the state to be capable of returning to the state
        //selectedCard becomes null to prevent a card being chosen despite not being the 1 of 3 cards drawn from the deck
        hasSelected = false;
        selectedCard = null;
        
        //Each controls adds the event for each key to enable the correct input on choosing a specifc card
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        //This reference the movement deck pile inside of the child of the player object
        movementDeck = GetComponentInChildren<movementDeckPile>();
        
        //This conditional statement checks if the previous state was start state to draw cards
        //If the previous state is not the start state then the cards stay the same
        if(player.PreviousState == player.StartState)
        {
            movementDeck.DrawCards();
        }

    }

    public override void UpdateState(playerStateManager player)
    {
        //Once the player selects the card they want the player changes to the roll state & applies the minimum, maximum roll and mana cost from the selected card

        if (hasSelected)
        {
            Debug.Log(selectedCard.name);
            //This is to collect the reference of the card that was selected and provide the data of the roll values and mana cost
            movementCard moveCard = selectedCard.GetComponent<movementCard>();
            minRoll = moveCard.RollMinimumValue;
            maxRoll = moveCard.RollMaximumValue;
            manaCost = moveCard.ManaCost;

            //This is to change the state to roll state
            player.ChangeState(player.RollState);
        }
        
    }

    public override void ExitState(playerStateManager player)
    {
        //Before exiting the deciding state, the state must reference the rolll state to have the roll state collect the suitable values
        rollState Rolling = player.RollState.GetComponent<rollState>();
        Rolling.CollectValue(minRoll, maxRoll, manaCost);

        //the inputs will need to be disabled once the state has been changed
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

    //These are using the interfaces of deciding Up, Down, Left, Right & Confirm in order for the state
    // Up, Left & Right provide movement cards and select unique cards
    // TODO Next Stage - Down provides the status effect
    // Once the player has confirm their choice the player moves onto the roll state (unless the selected card is still empty)
    public void DecidingUp(object sender, EventArgs e)
    {
        selectedCard = movementDeck.SelectedCards[1];
        Debug.Log(selectedCard.name);
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedCard = null;
        Debug.LogWarning("Needs to Implement Movement Card");
    }

    public void DecidingLeft(object sender, EventArgs e)
    {       
        selectedCard = movementDeck.SelectedCards[0];
        Debug.Log(selectedCard.name);
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedCard = movementDeck.SelectedCards[2];
        Debug.Log(selectedCard.name);
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //If there is a GameObject inside of confirm choice then the player has successfully selected
        if (selectedCard != null)
        {
            hasSelected = true;
        }
        //otherwise apply an error to inform that the player hasn't chosen a card yet
        else
        {
            Debug.LogError("The Card has not been selected");
        }
    }
}
