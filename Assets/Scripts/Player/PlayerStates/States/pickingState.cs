using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickingState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm, ICancel
{
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private playerController controller;
    [SerializeField] private List <movementCardStats> possibleMovementCards;
    private movementCardStats selectedMovementCard;

    private int selectedCard;

    public CardRarity rarity;
    private int rarityInt;

    public CardType typeSelected;

    private bool cardCollected;

    public override void EnterState(playerStateManager player)
    {
        cardCollected = false;
        typeSelected = CardType.Null;
        
        controller = GetComponent<playerController>();
        possibleMovementCards = controller.GetData.possibleMovementCards;

        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        
        
        rarityInt = UnityEngine.Random.Range(1, 5);
        if (rarityInt <= 3) 
        {
            rarity = CardRarity.Uncommon;
        }
        else
        {
            rarity = CardRarity.Rare;
        }
    }

    public override void UpdateState(playerStateManager player)
    {
        if (cardCollected) 
        { 
            player.ChangeState(player.InactiveState);
        }
    }

    public override void ExitState(playerStateManager player)
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

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
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if(typeSelected == CardType.Null)
        {
            Debug.LogError("You haven't selected a card yet");
        }

        else if(typeSelected == CardType.Movement) 
        {
            selectedCard = UnityEngine.Random.Range(0, possibleMovementCards.Count);
            selectedMovementCard = possibleMovementCards[selectedCard];
            while (selectedMovementCard.cardRarity != rarity) 
            {
                selectedCard = UnityEngine.Random.Range(0, possibleMovementCards.Count);
                selectedMovementCard = possibleMovementCards[selectedCard];
            }
            
            GameObject card = movementDeckPool.instance.GetAvailableMovement();
            if (card != null)
            {
                card.SetActive(true);
                //this will then add the offence card component into the deck & add the offence card data in the card object
                movementCard movement = card.AddComponent<movementCard>();
                movement.CreateCard(selectedMovementCard);
                cardCollected = true;
            }

            else if (card == null) 
            {
                Debug.LogWarning("No Available Cards, Select a Different Deck");
            }
        }
    }

    public void Cancel(object sender, EventArgs e)
    {
        cardCollected = true;
    }

}
