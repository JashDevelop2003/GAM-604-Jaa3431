using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// First Playable: This state allows the player to select a type of card they want to use and add a new card to their deck
/// This includes Movement cards for the player to choose from and collect
/// TODO: This also includes Offence & Defence Cards for the player to choose from and collect
/// TODO Next Stage: This also include Status Cards for the player to choose from and collect
/// </summary>

public class pickingState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm, ICancel
{
    //the controls are used to select the cards or even ignore collecting
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    //the controller is used to provide the possible cards the player can obtain based on their character
    private playerController controller;

    //this is for the movement cards and provides the possible movement card they can select
    [SerializeField] private List <movementCardStats> possibleMovementCards;
    private movementCardStats selectedMovementCard;

    //this selects the card out of the list and checks if this card is sutiable for the rarity
    private int selectedCard;

    //The type of rarity indicates the rarity card the player will obtain from this card space
    public CardRarity rarity;

    //The rarity int will provide a random range when entering to provide unique rarities
    private int rarityInt;

    //the card type is the selected type of card the player wants to obtain
    public CardType typeSelected;

    //this boolean is to change the state once the player has confirm or cancel obtaining a card
    private bool cardCollected;

    public override void EnterState(playerStateManager player)
    {
        //the boolean is set to false and card type to null to ensure that the player doesn't instantly change state or confirm the choice immediately
        cardCollected = false;
        typeSelected = CardType.Null;
        
        //the controller is referenced to collect the character data of the possible card to obtain
        controller = GetComponent<playerController>();
        possibleMovementCards = controller.GetData.possibleMovementCards;

        //this enables to deciding events towards selecting a type of card
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;
       
        //this provides a random range to provide a 75% chance of being Uncommon & 25% of being Rare
        //TODO Next Stage: Add Legendary & Change the probability to (60/30/10)
        rarityInt = UnityEngine.Random.Range(1, 5);
        if (rarityInt <= 3) 
        {
            rarity = CardRarity.Uncommon;
        }
        else if(rarityInt == 4)
        {
            rarity = CardRarity.Rare;
        }
    }

    //the state changes in the update state of the picking state once cardCollect becomes true
    public override void UpdateState(playerStateManager player)
    {
        if (cardCollected) 
        { 
            player.ChangeState(player.InactiveState);
        }
    }

    //When exiting this state, all methods should be disabled from listening to the controls subject
    public override void ExitState(playerStateManager player)
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

    //These deciding interface methods are used to provide unique card types to select
    // Up is selecting movement
    // TODO Next Stage: Down is selecting status
    // TODO: Left is selecting offence
    // TODO: Right is selecting defence
    public void DecidingUp(object sender, EventArgs e)
    {
        typeSelected = CardType.Movement;
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        typeSelected = CardType.Status;
        Debug.LogWarning("Needs Status Cards to be Implemented");

    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        typeSelected = CardType.Offence;
        Debug.LogWarning("Needs Offence Cards to be Implemented");
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        typeSelected = CardType.Defence;
        Debug.LogWarning("Needs Defence Cards to be Implemented");

    }

    //TODO: Add Offence, Defence & Status

    //Once selected the card the confirm method must check if there's an availble card slot in the deck to set active
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //If the card type is still null then the player hasn't decided yet
        if(typeSelected == CardType.Null)
        {
            Debug.LogError("You haven't selected a card yet");
        }

        //if the type selected is movement then the method needs to check for avaialble movement slots
        else if(typeSelected == CardType.Movement) 
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            selectedCard = UnityEngine.Random.Range(0, possibleMovementCards.Count);
            selectedMovementCard = possibleMovementCards[selectedCard];
            while (selectedMovementCard.cardRarity != rarity) 
            {
                selectedCard = UnityEngine.Random.Range(0, possibleMovementCards.Count);
                selectedMovementCard = possibleMovementCards[selectedCard];
            }

            //this part of the method collect the movement deck pool to check if there is any objects that are set to false
            movementDeckPool pool = GetComponentInChildren<movementDeckPool>();
            GameObject card = pool.GetAvailableMovement();
            //if there is a object that is set to false then add the selected card into the deck
            if (card != null)
            {
                card.SetActive(true);
                movementCard movement = card.AddComponent<movementCard>();
                movement.CreateCard(selectedMovementCard);
                cardCollected = true;
            }

            //else inform the player that there are no available slots to proivde
            else if (card == null) 
            {
                Debug.LogWarning("No Available Cards, Select a Different Deck");
            }
        }
    }

    //the cancel interface method allows the player to decline obtaining a card
    public void Cancel(object sender, EventArgs e)
    {
        cardCollected = true;
    }

}
