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

    //Decks are required to identify if there is any space on making rare cards
    private movementDeckPool moveDeck;
    private offenceDeckPool offenceDeck;
    private defenceDeckPool defenceDeck;
    private statusDeckPool statusDeck;

    // upon pickup does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();

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

        }
    }

    void ObtainOffence()
    {
        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        offenceDeck = player.GetComponentInChildren<offenceDeckPool>();
        GameObject attackCard = offenceDeck.GetAvailableOffence();
        if (attackCard != null)
        {
            offenceDeck.CreateCard(rarity);
        }
    }

    void ObtainDefence()
    {
        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        defenceDeck = player.GetComponentInChildren<defenceDeckPool>();
        GameObject defendCard = defenceDeck.GetAvailableDefence();
        if (defendCard != null)
        {
            defenceDeck.CreateCard(rarity);
        }
    }

    void ObtainMovement()
    {
        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        moveDeck = player.GetComponentInChildren<movementDeckPool>();
        GameObject moveCard = moveDeck.GetAvailableMovement();
        if (moveCard != null)
        {
            moveDeck.CreateCard(rarity);
        }
    }

    void ObtainStatus()
    {

        //This section checks if the player can obtain the status card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        statusDeck = player.GetComponentInChildren<statusDeckPool>();
        GameObject statusCard = statusDeck.GetAvailableStatus();
        if (statusCard != null)
        {
            statusDeck.CreateCard(rarity);
        }
    }

    private void OnDestroy()
    {
        state.startItemEvents -= CardPrinter;

    }
}
