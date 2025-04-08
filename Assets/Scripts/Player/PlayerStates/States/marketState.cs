using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The market state is when the player access the market and decide to spend their cash onto cards & items
/// The chances and prices of these should include:
/// 40% Chance for a uncommon card of any type (equal odds)
/// 30% Chance for a rare card of any type (equal odds)
/// 20% Chance for a legendary card of any type (equal odds)
/// 10% Chance for a item
/// </summary>

//A struct wiull be use to provide the retail object ()
[System.Serializable]
public struct InMarket
{
    public marketEnum retailObject;
    public deckTypeEnum retailType;
    public int price;
    public bool hasBought;
}

public class marketState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm, ICancel
{
    //the controls are used to select the cards or even ignore collecting
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private playerController controller;

    //this is for the movement cards and provides the possible movement card they can select from the character data
    [SerializeField] private List<movementCardStats> possibleMovementCards;
    private movementCardStats selectedMovementCard;
    private movementDeckPool movementDeck;

    //This is for the offence cards and provides the possible offence card they can select from the character data
    [SerializeField] private List<offenceCardStats> possibleOffenceCards;
    private offenceCardStats selectedOffenceCard;
    private offenceDeckPool offenceDeck;

    //This is for the defence cards and provides the possible defence card they can select from the character data
    [SerializeField] private List<defenceCardStats> possibleDefenceCards;
    private defenceCardStats selectedDefenceCard;
    private defenceDeckPool defenceDeck;

    //this is for the status cards and provide the possible stauts card they can select from the character data
    [SerializeField] private List<statusCardStats> possibleStatusCards;
    private statusCardStats selectedStatusCard;
    private statusDeckPool statusDeck;

    [SerializeField] private List<itemStats> possibleItems;
    private itemStats selectedItem;
    private itemDeckPool itemDeck;

    //this selects the card out of the list and checks if this card is sutiable for the rarity
    private int selectedResource;

    [SerializeField] private GameObject[] checkingAvailability = new GameObject[5];

    //These lists will be use for the retail stocks for the player to obtain either a card or item
    [SerializeField] private int[] outcomeResource = new int[4];
    [SerializeField] private int[] outcomeType = new int[4];
    [SerializeField] private InMarket[] inMarket = new InMarket[4];

    [SerializeField] private InMarket selectedStock;
    private int boughtStock;

    private bool endShopping;
    
    public override void EnterState(playerStateManager player)
    {
        selectedStock.price = 0;
        selectedStock.retailObject = marketEnum.Null;

        //this enables to deciding events towards selecting a type of card
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;
        Controls.cancelPressed += Cancel;

        //the controller is referenced to collect the character data of the possible card to obtain
        controller = GetComponent<playerController>();
        possibleMovementCards = controller.GetData.possibleMovementCards;
        possibleOffenceCards = controller.GetData.possibleOffenceCards;
        possibleDefenceCards = controller.GetData.possibleDefenceCards;
        possibleStatusCards = controller.GetData.possibleStatusCards;
        possibleItems = controller.GetData.possibleRelics;

        //This checks if there is an avaialble slot for the player to create cards and items
        offenceDeck = GetComponentInChildren<offenceDeckPool>();
        checkingAvailability[0] = offenceDeck.GetAvailableOffence();

        defenceDeck = GetComponentInChildren<defenceDeckPool>();
        checkingAvailability[1] = defenceDeck.GetAvailableDefence();

        movementDeck = GetComponentInChildren<movementDeckPool>();
        checkingAvailability[2] = movementDeck.GetAvailableMovement();

        statusDeck = GetComponentInChildren<statusDeckPool>();
        checkingAvailability[3] = statusDeck.GetAvailableStatus();

        itemDeck = GetComponentInChildren<itemDeckPool>();
        checkingAvailability[4] = itemDeck.GetAvailableItem();

        //This applies the new resource in the game
        //The while loop checks if the resource is available
        for (int i = 0; i < outcomeResource.Length; i++)
        {
            outcomeResource[i] = UnityEngine.Random.Range(1, 11);
            outcomeType[i] = UnityEngine.Random.Range(0, 5);
            while (checkingAvailability[outcomeType[i]] == null)
            {
                outcomeResource[i] = UnityEngine.Random.Range(1, 11);
                outcomeType[i] = UnityEngine.Random.Range(0, 5);
            }
        }

        for (int i = 0; i < inMarket.Length; i++)
        {
            if (outcomeResource[i] <= 4)
            {
                inMarket[i].retailObject = marketEnum.UncommonCard;
                inMarket[i].price = 25;
            }
            else if(outcomeResource[i] >= 5 || outcomeResource[i] <= 7)
            {
                inMarket[i].retailObject = marketEnum.RareCard;
                inMarket[i].price = 50;
            }
            else if (outcomeResource[i] == 8 ||  outcomeResource[i] == 9)
            {
                inMarket[i].retailObject = marketEnum.LegendaryCard;
                inMarket[i].price = 75;
            }
            else if (outcomeResource[i] == 10)
            {
                inMarket[i].retailObject = marketEnum.Item;
                inMarket[i].price = 100;
            }
            else
            {
                Debug.LogError("Something with wrong when identifying the outcome resource");
            }

            if (outcomeType[i] == (int)deckTypeEnum.Offence) 
            {
                inMarket[i].retailType = deckTypeEnum.Offence;
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Defence)
            {
                inMarket[i].retailType = deckTypeEnum.Defence;
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Movement)
            {
                inMarket[i].retailType = deckTypeEnum.Movement;
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Status)
            {
                inMarket[i].retailType = deckTypeEnum.Status;
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Item)
            {
                inMarket[i].retailType = deckTypeEnum.Item;
            }

            inMarket[i].hasBought = false;

        }
    }

    public override void UpdateState(playerStateManager player)
    {
        if (endShopping) 
        { 
            player.ChangeState(player.InactiveState);
        }
    }

    public override void ExitState(playerStateManager player) 
    {
        endShopping = false;
        Debug.Log("Finished Shopping");
    }

    public void DecidingUp(object sender, EventArgs e)
    {
        selectedStock = inMarket[0];
        boughtStock = 0;
    }
    
    public void DecidingDown(object sender, EventArgs e)
    {
        selectedStock = inMarket[2];
        boughtStock = 2;
    }
    
    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedStock = inMarket[3];
        boughtStock = 3;
    }
    
    public void DecidingRight(object sender, EventArgs e)
    {
        selectedStock = inMarket[1];
        boughtStock = 1;
    }
    
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedStock.retailObject == marketEnum.Null)
        {
            Debug.LogWarning("You didn't choose an item yet");
        }
        else if (selectedStock.hasBought)
        {
            Debug.Log("Stock has been sold out");
        }
        else if (selectedStock.price > controller.GetModel.CurrentCash)
        {
            Debug.LogWarning("You cannot afford to obtain that resource");
        }
        else
        {
            if (selectedStock.retailObject != marketEnum.Item)
            {
                ObtainItem();
            }
            else
            {
                if (selectedStock.retailObject == marketEnum.LegendaryCard)
                {
                    ObtainCard(CardRarity.Legendary);
                }
                else if (selectedStock.retailObject == marketEnum.RareCard)
                {
                    ObtainCard(CardRarity.Rare);
                }
                else if (selectedStock.retailObject == marketEnum.UncommonCard) 
                { 
                    ObtainCard(CardRarity.Uncommon);
                }
            }

            controller.ChangeCash(-selectedStock.price);
            inMarket[boughtStock].hasBought = true;
        }
    }

    public void Cancel(object sender, EventArgs e)
    {
        StartCoroutine(EndShopping());
    }

    void ObtainItem()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleItems.Count);
        selectedItem = possibleItems[selectedInt];
        GameObject relic = checkingAvailability[4];
        relic.SetActive(true);
        itemBehaviour item = relic.AddComponent<itemBehaviour>();
        item.CreateItem(selectedItem);
        controller.IncrementDeck(deckTypeEnum.Item);
    }

    void ObtainCard(CardRarity rarity)
    {
        //if the type selected is offence then the method needs to check for avaialble offence slots
        if (selectedStock.retailType == deckTypeEnum.Offence)
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            int selectedCard = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
            selectedOffenceCard = possibleOffenceCards[selectedCard];
            while (selectedOffenceCard.cardRarity != rarity)
            {
                selectedCard = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
                selectedOffenceCard = possibleOffenceCards[selectedCard];
            }

            GameObject offenceCard = checkingAvailability[0];
            offenceCard.SetActive(true);
            offenceCard offence = offenceCard.AddComponent<offenceCard>();
            offence.CreateCard(selectedOffenceCard);
            controller.IncrementDeck(deckTypeEnum.Offence);
        }

        else if (selectedStock.retailType == deckTypeEnum.Defence)
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            int selectedCard = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
            selectedDefenceCard = possibleDefenceCards[selectedCard];
            while (selectedDefenceCard.cardRarity != rarity)
            {
                selectedCard = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
                selectedDefenceCard = possibleDefenceCards[selectedCard];
            }

            GameObject defenceCard = checkingAvailability[1];
            defenceCard.SetActive(true);
            defenceCard defence = defenceCard.AddComponent<defenceCard>();
            defence.CreateCard(selectedDefenceCard);
            controller.IncrementDeck(deckTypeEnum.Defence);
        }

        else if (selectedStock.retailType == deckTypeEnum.Movement)
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            int selectedCard = UnityEngine.Random.Range(0, possibleMovementCards.Count);
            selectedMovementCard = possibleMovementCards[selectedCard];
            while (selectedMovementCard.cardRarity != rarity)
            {
                selectedCard = UnityEngine.Random.Range(0, possibleMovementCards.Count);
                selectedMovementCard = possibleMovementCards[selectedCard];
            }

            GameObject moveCard = checkingAvailability[2];
            moveCard.SetActive(true);
            movementCard move = moveCard.AddComponent<movementCard>();
            move.CreateCard(selectedMovementCard);
            controller.IncrementDeck(deckTypeEnum.Movement);
        }

        else if (selectedStock.retailType == deckTypeEnum.Status)
        {
            //this part of the method creates the suitable and provide a while loop to ensure that the player obtains the correct rarity
            int selectedCard = UnityEngine.Random.Range(0, possibleStatusCards.Count);
            selectedStatusCard = possibleStatusCards[selectedCard];
            while (selectedStatusCard.cardRarity != rarity)
            {
                selectedCard = UnityEngine.Random.Range(0, possibleStatusCards.Count);
                selectedStatusCard = possibleStatusCards[selectedCard];
            }

            GameObject statCard = checkingAvailability[3];
            statCard.SetActive(true);
            statusCard stat = statCard.AddComponent<statusCard>();
            stat.CreateCard(selectedStatusCard);
            controller.IncrementDeck(deckTypeEnum.Status);
        }
    }

    IEnumerator EndShopping()
    {
        Debug.Log("Ending Shopping");
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.cancelPressed -= Cancel;
        yield return new WaitForSeconds(2);
        endShopping = true;
    }
}
