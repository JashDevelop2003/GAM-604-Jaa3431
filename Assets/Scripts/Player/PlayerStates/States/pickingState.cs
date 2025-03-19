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

    //This is to see if the player has a lucky buff on them to change the possibilities
    private currentBuffs buffs;

    //this is for the movement cards and provides the possible movement card they can select from the character data
    [SerializeField] private List <movementCardStats> possibleMovementCards;
    private movementCardStats selectedMovementCard;

    //This is for the offence cards and provides the possible offence card they can select from the character data
    [SerializeField] private List<offenceCardStats> possibleOffenceCards;
    private offenceCardStats selectedOffenceCard;

    //This is for the defence cards and provides the possible defence card they can select from the character data
    [SerializeField] private List<defenceCardStats> possibleDefenceCards;
    private defenceCardStats selectedDefenceCard;

    [SerializeField] private List<statusCardStats> possibleStatusCards;
    private statusCardStats selectedStatusCard;

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

    [SerializeField] private GameObject[] checkingAvailability;
    [SerializeField] private int numberOfNoSlots;

    public override void EnterState(playerStateManager player)
    {
        //the boolean is set to false and card type to null to ensure that the player doesn't instantly change state or confirm the choice immediately
        cardCollected = false;
        typeSelected = CardType.Null;
        checkingAvailability = new GameObject[4];
        numberOfNoSlots = 0;
        
        //the controller is referenced to collect the character data of the possible card to obtain
        controller = GetComponent<playerController>();
        possibleMovementCards = controller.GetData.possibleMovementCards;
        possibleOffenceCards = controller.GetData.possibleOffenceCards;
        possibleDefenceCards = controller.GetData.possibleDefenceCards;
        possibleStatusCards = controller.GetData.possibleStatusCards;

        //the buff is reference to change the likelihood to obtaining a rarer card
        buffs = GetComponent<currentBuffs>();

        //this enables to deciding events towards selecting a type of card
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        //This checks if there is an avaialble slot for the player to create cards
        offenceDeckPool offenceDeck = GetComponentInChildren<offenceDeckPool>();
        checkingAvailability[0] = offenceDeck.GetAvailableOffence();

        defenceDeckPool defenceDeck = GetComponentInChildren<defenceDeckPool>();
        checkingAvailability[1] = defenceDeck.GetAvailableDefence();

        movementDeckPool movementDeck = GetComponentInChildren<movementDeckPool>();
        checkingAvailability[2] = movementDeck.GetAvailableMovement();

        statusDeckPool statusDeck = GetComponentInChildren<statusDeckPool>();
        checkingAvailability[3] = statusDeck.GetAvailableStatus();

        for(int i = 0; i < checkingAvailability.Length; i++)
        {
            if(checkingAvailability[i] == null)
            {
                numberOfNoSlots++;
            }
        }

        if(numberOfNoSlots == 4)
        {
            Debug.Log("Player Cannot Collect any more cards");
            cardCollected = true;
        }

        //this provides a random range to provide a 75% chance of being Uncommon & 25% of being Rare
        //However is 100% for rare card if the player is lucky
        //TODO: Add Legendary & Change the probability to (50/40/10)
        //TODO: Change the lucky probability to (20/50/30)       
        rarityInt = UnityEngine.Random.Range(1, 11);

        if (buffs.IsLucky)
        {
            if (rarityInt <= 2)
            {
                rarity = CardRarity.Uncommon;
            }
            else if (rarityInt >= 3 && rarityInt <= 7)
            {
                rarity = CardRarity.Rare;
                Debug.Log("Lucky Chance");
            }
            else if (rarityInt >= 8)
            {
                rarity = CardRarity.Legendary;
                Debug.Log("Very Lucky Chance");
            }
            else
            {
                Debug.LogError("Something went wrong with the random range");
            }
        }        
        else
        {
            if (rarityInt <= 5)
            {
                rarity = CardRarity.Uncommon;
            }
            else if (rarityInt >= 6 && rarityInt <= 9)
            {
                rarity = CardRarity.Rare;
                Debug.Log("Lucky Chance");
            }
            else if(rarityInt == 10)
            {
                rarity = CardRarity.Legendary;
                Debug.Log("Very Lucky Chance");
            }
            else
            {
                Debug.LogError("Something went wrong with the random range");
            }
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
    // Down is selecting status
    // Left is selecting offence
    // Right is selecting defence
    public void DecidingUp(object sender, EventArgs e)
    {
        if (checkingAvailability[2] != null)
        {
            typeSelected = CardType.Movement;
            Debug.Log(typeSelected);
        }
        else
        {
            typeSelected = CardType.Null;
            Debug.LogWarning(typeSelected + " There isn't any room for a new movement card");
        }
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        if (checkingAvailability[3] != null)
        {
            typeSelected = CardType.Status;
            Debug.Log(typeSelected);
        }
        else
        {
            typeSelected = CardType.Null;
            Debug.LogWarning(typeSelected + " There isn't any room for a new status card");
        }

    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        if (checkingAvailability[0] != null)
        {
            typeSelected = CardType.Offence;
            Debug.Log(typeSelected);
        }
        else
        {
            typeSelected = CardType.Null;
            Debug.LogWarning(typeSelected + " There isn't any room for a new offence card");
        }
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        if (checkingAvailability[1] != null)
        {
            typeSelected = CardType.Defence;
            Debug.Log(typeSelected);
        }
        else
        {
            typeSelected = CardType.Null;
            Debug.LogWarning(typeSelected + " There isn't any room for a new defence card");
        }
    }

    //Once selected the card the confirm method must check if there's an availble card slot in the deck to set active
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //If the card type is still null then the player hasn't decided yet
        if (typeSelected == CardType.Null)
        {
            Debug.LogError("You haven't selected a card yet");
        }

        //if the type selected is movement then the method needs to check for avaialble movement slots
        else if (typeSelected == CardType.Movement)
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
            movementDeckPool movePool = GetComponentInChildren<movementDeckPool>();
            GameObject moveCard = movePool.GetAvailableMovement();
            //if there is a object that is set to false then add the selected card into the deck
            if (moveCard != null)
            {
                moveCard.SetActive(true);
                movementCard movement = moveCard.AddComponent<movementCard>();
                movement.CreateCard(selectedMovementCard);
                cardCollected = true;
            }

            //else inform the player that there are no available slots to proivde
            else if (moveCard == null)
            {
                Debug.LogWarning("No Available Cards, Select a Different Deck");
            }

        }

        //if the type selected is offence then the method needs to check for avaialble offence slots
        else if (typeSelected == CardType.Offence)
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            selectedCard = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
            selectedOffenceCard = possibleOffenceCards[selectedCard];
            while (selectedOffenceCard.cardRarity != rarity)
            {
                selectedCard = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
                selectedOffenceCard = possibleOffenceCards[selectedCard];
            }

            //this part of the method collect the offence deck pool to check if there is any objects that are set to false
            offenceDeckPool offencePool = GetComponentInChildren<offenceDeckPool>();
            GameObject offenceCard = offencePool.GetAvailableOffence();
            //if there is a object that is set to false then add the selected card into the deck
            if (offenceCard != null)
            {
                offenceCard.SetActive(true);
                offenceCard offence = offenceCard.AddComponent<offenceCard>();
                offence.CreateCard(selectedOffenceCard);
                cardCollected = true;
            }

            //else inform the player that there are no available slots to proivde
            else if (offenceCard == null)
            {
                Debug.LogWarning("No Available Cards, Select a Different Deck");
            }
        }

        //if the type selected is defence then the method needs to check for avaialble defence slots
        else if (typeSelected == CardType.Defence)
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            selectedCard = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
            selectedDefenceCard = possibleDefenceCards[selectedCard];
            while (selectedDefenceCard.cardRarity != rarity)
            {
                selectedCard = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
                selectedDefenceCard = possibleDefenceCards[selectedCard];
            }

            //this part of the method collect the defence deck pool to check if there is any objects that are set to false
            defenceDeckPool defencePool = GetComponentInChildren<defenceDeckPool>();
            GameObject defenceCard = defencePool.GetAvailableDefence();
            //if there is a object that is set to false then add the selected card into the deck
            if (defenceCard != null)
            {
                defenceCard.SetActive(true);
                defenceCard defence = defenceCard.AddComponent<defenceCard>();
                defence.CreateCard(selectedDefenceCard);
                cardCollected = true;
            }

            //else inform the player that there are no available slots to proivde
            else if (defenceCard == null)
            {
                Debug.LogWarning("No Available Cards, Select a Different Deck");
            }
        }

        //if the type selected is Status then the method needs to check for avaialble Status slots
        else if (typeSelected == CardType.Status)
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            selectedCard = UnityEngine.Random.Range(0, possibleStatusCards.Count);
            selectedStatusCard = possibleStatusCards[selectedCard];
            while (selectedStatusCard.cardRarity != rarity)
            {
                selectedCard = UnityEngine.Random.Range(0, possibleStatusCards.Count);
                selectedStatusCard = possibleStatusCards[selectedCard];
            }

            //this part of the method collect the status deck pool to check if there is any objects that are set to false
            statusDeckPool statusPool = GetComponentInChildren<statusDeckPool>();
            GameObject statusCard = statusPool.GetAvailableStatus();
            //if there is a object that is set to false then add the selected card into the deck
            if (statusCard != null)
            {
                statusCard.SetActive(true);
                statusCard status = statusCard.AddComponent<statusCard>();
                status.CreateCard(selectedStatusCard);
                cardCollected = true;
            }

            //else inform the player that there are no available slots to proivde
            else if (statusCard == null)
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
