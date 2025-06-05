using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardPrinterEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private startState state;

    private CardRarity rarity;
    private int randomChance;
    private int randomType;

    //this is for the movement cards and provides the possible movement card they can select from the character data
    private movementDeckPool moveDeck;
    [SerializeField] private List<movementCardStats> possibleMovementCards;
    [SerializeField] private movementCardStats selectedMovementCard;

    //This is for the offence cards and provides the possible offence card they can select from the character data
    private offenceDeckPool offenceDeck;
    [SerializeField] private List<offenceCardStats> possibleOffenceCards;
    [SerializeField] private offenceCardStats selectedOffenceCard;

    //This is for the defence cards and provides the possible defence card they can select from the character data
    private defenceDeckPool defenceDeck;
    [SerializeField] private List<defenceCardStats> possibleDefenceCards;
    [SerializeField] private defenceCardStats selectedDefenceCard;

    private statusDeckPool statusDeck;
    [SerializeField] private List<statusCardStats> possibleStatusCards;
    [SerializeField] private statusCardStats selectedStatusCard;

    // upon pickup does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();

        possibleDefenceCards = controller.GetData.possibleDefenceCards;
        possibleOffenceCards = controller.GetData.possibleOffenceCards;
        possibleMovementCards = controller.GetData.possibleMovementCards;
        possibleStatusCards = controller.GetData.possibleStatusCards;

        state.startItemEvents += CardPrinter;
    }

    // During the start of the turn the player has a 20% chance of obtaining a random rare card type & 5% chance of obtaining a random legendary card type
    public void CardPrinter(object sender, EventArgs e)
    {
        randomChance = UnityEngine.Random.Range(0, 21);
        randomType = UnityEngine.Random.Range(0, 4);

        if (randomChance == 0) 
        { 
            rarity = CardRarity.Legendary;
            if (randomType == (int)deckTypeEnum.Offence)
            {
                ObtainOffence();
            }
            else if (randomType == (int)deckTypeEnum.Defence)
            {
                ObtainDefence();
            }
            else if (randomType == (int)deckTypeEnum.Movement)
            {
                ObtainMovement();
            }
            else if (randomType == (int)deckTypeEnum.Status) 
            { 
                ObtainStatus();
            }

        }

        else if (randomChance > 0 && randomChance <= 5)
        {
            rarity = CardRarity.Rare;
            if (randomType == (int)deckTypeEnum.Offence)
            {
                ObtainOffence();
            }
            else if (randomType == (int)deckTypeEnum.Defence)
            {
                ObtainDefence();
            }
            else if (randomType == (int)deckTypeEnum.Movement)
            {
                ObtainMovement();
            }
            else if (randomType == (int)deckTypeEnum.Status)
            {
                ObtainStatus();
            }
            else
            {
                Debug.LogError("Something went wrong with the int");
            }

        }
    }

    void ObtainOffence()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
        selectedOffenceCard = possibleOffenceCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedOffenceCard.cardRarity != rarity)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
            selectedOffenceCard = possibleOffenceCards[selectedInt];
        }

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        offenceDeck = player.GetComponentInChildren<offenceDeckPool>();
        GameObject attackCard = offenceDeck.GetAvailableOffence();
        if (attackCard != null)
        {
            attackCard.SetActive(true);
            offenceCard attack = attackCard.AddComponent<offenceCard>();
            attack.CreateCard(selectedOffenceCard);
            controller.IncrementDeck(deckTypeEnum.Offence);
            if (controller.Player == 1)
            {
                playerOneData playerData = GetComponentInChildren<playerOneData>();
                playerData.storedOffence.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = GetComponentInChildren<playerTwoData>();
                playerData.storedOffence.Add(selectedInt);
            }
        }
    }

    void ObtainDefence()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
        selectedDefenceCard = possibleDefenceCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedDefenceCard.cardRarity != rarity)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
            selectedDefenceCard = possibleDefenceCards[selectedInt];
        }

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        defenceDeck = player.GetComponentInChildren<defenceDeckPool>();
        GameObject defendCard = defenceDeck.GetAvailableDefence();
        if (defendCard != null)
        {
            defendCard.SetActive(true);
            defenceCard defend = defendCard.AddComponent<defenceCard>();
            defend.CreateCard(selectedDefenceCard);
            controller.IncrementDeck(deckTypeEnum.Defence);
            if (controller.Player == 1)
            {
                playerOneData playerData = GetComponentInChildren<playerOneData>();
                playerData.storedDefence.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = GetComponentInChildren<playerTwoData>();
                playerData.storedDefence.Add(selectedInt);
            }
        }
    }

    void ObtainMovement()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleMovementCards.Count);
        selectedMovementCard = possibleMovementCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedMovementCard.cardRarity != rarity)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleMovementCards.Count);
            selectedMovementCard = possibleMovementCards[selectedInt];
        }

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        moveDeck = player.GetComponentInChildren<movementDeckPool>();
        GameObject moveCard = moveDeck.GetAvailableMovement();
        if (moveCard != null)
        {
            moveCard.SetActive(true);
            movementCard move = moveCard.AddComponent<movementCard>();
            move.CreateCard(selectedMovementCard);
            controller.IncrementDeck(deckTypeEnum.Movement);
            if (controller.Player == 1)
            {
                playerOneData playerData = GetComponentInChildren<playerOneData>();
                playerData.storedMovement.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = GetComponentInChildren<playerTwoData>();
                playerData.storedMovement.Add(selectedInt);
            }
        }
    }

    void ObtainStatus()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleStatusCards.Count);
        selectedStatusCard = possibleStatusCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedStatusCard.cardRarity != rarity)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleStatusCards.Count);
            selectedStatusCard = possibleStatusCards[selectedInt];
        }

        //This section checks if the player can obtain the status card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        statusDeck = player.GetComponentInChildren<statusDeckPool>();
        GameObject statusCard = statusDeck.GetAvailableStatus();
        if (statusCard != null)
        {
            statusCard.SetActive(true);
            statusCard status = statusCard.AddComponent<statusCard>();
            status.CreateCard(selectedStatusCard);
            controller.IncrementDeck(deckTypeEnum.Status);
            if (controller.Player == 1)
            {
                playerOneData playerData = GetComponentInChildren<playerOneData>();
                playerData.storedStatus.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = GetComponentInChildren<playerTwoData>();
                playerData.storedStatus.Add(selectedInt);
            }
        }
    }

    private void OnDestroy()
    {
        state.startItemEvents -= CardPrinter;

    }
}
