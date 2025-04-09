using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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

    private GameObject[] checkingAvailability = new GameObject[5];

    //These lists will be use for the retail stocks for the player to obtain either a card or item
    private int[] outcomeResource = new int[4];
    private int[] outcomeType = new int[4];
    [SerializeField] private InMarket[] inMarket = new InMarket[4];

    [SerializeField] private InMarket selectedStock;
    private int boughtStock;

    private bool endShopping;

    [Header("User Interface")]
    [SerializeField] private GameObject marketUI;
    [SerializeField] private Color[] setColour = new Color[5];
    [SerializeField] private Image[] sectionDisplay = new Image[4];

    [SerializeField] private GameObject[] cardUI = new GameObject[4];
    [SerializeField] private Image[] cardImage = new Image[4];
    [SerializeField] private Sprite[] typeImage = new Sprite[4];
    [SerializeField] private TMP_Text[] cardTypeName = new TMP_Text[4];
    [SerializeField] private TMP_Text[] cardPrice = new TMP_Text[4];

    [SerializeField] private GameObject[] itemUI = new GameObject[4];
    [SerializeField] private TMP_Text[] itemPrice = new TMP_Text[4];
    [SerializeField] private TMP_Text eventText;

    
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
            outcomeType[i] = UnityEngine.Random.Range(0, 4);
            while (checkingAvailability[outcomeType[i]] == null)
            {
                Debug.LogWarning("Resource type is too full, changing resource type");
                outcomeResource[i] = UnityEngine.Random.Range(1, 11);
                outcomeType[i] = UnityEngine.Random.Range(0, 4);
            }
        }

        marketUI.SetActive(true);

        for (int i = 0; i < inMarket.Length; i++)
        {
            if (outcomeResource[i] <= 4)
            {
                inMarket[i].retailObject = marketEnum.UncommonCard;
                inMarket[i].price = 25;
                sectionDisplay[i].color = setColour[0];
                cardUI[i].SetActive(true);
                itemUI[i].SetActive(false);
                cardPrice[i].SetText("Price: 25");
            }
            else if(outcomeResource[i] >= 5 || outcomeResource[i] <= 7)
            {
                inMarket[i].retailObject = marketEnum.RareCard;
                inMarket[i].price = 50;
                sectionDisplay[i].color = setColour[1];
                cardUI[i].SetActive(true);
                itemUI[i].SetActive(false);
                cardPrice[i].SetText("Price: 50");
            }
            else if (outcomeResource[i] == 8 ||  outcomeResource[i] == 9)
            {
                inMarket[i].retailObject = marketEnum.LegendaryCard;
                inMarket[i].price = 75;
                sectionDisplay[i].color = setColour[2];
                cardUI[i].SetActive(true);
                itemUI[i].SetActive(false);
                cardPrice[i].SetText("Price: 75");
            }
            else if (outcomeResource[i] == 10)
            {
                inMarket[i].retailObject = marketEnum.Item;
                inMarket[i].price = 100;
                sectionDisplay[i].color = setColour[3];
                cardUI[i].SetActive(false);
                itemUI[i].SetActive(true);
                itemPrice[i].SetText("Price: 100");
            }
            else
            {
                Debug.LogError("Something with wrong when identifying the outcome resource");
            }

            if (outcomeType[i] == (int)deckTypeEnum.Offence) 
            {
                inMarket[i].retailType = deckTypeEnum.Offence;
                cardTypeName[i].SetText("Offence");
                cardImage[i].sprite = typeImage[0];
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Defence)
            {
                inMarket[i].retailType = deckTypeEnum.Defence;
                cardTypeName[i].SetText("Defence");
                cardImage[i].sprite = typeImage[1];
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Movement)
            {
                inMarket[i].retailType = deckTypeEnum.Movement;
                cardTypeName[i].SetText("Movement");
                cardImage[i].sprite = typeImage[2];
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Status)
            {
                inMarket[i].retailType = deckTypeEnum.Status;
                cardTypeName[i].SetText("Status");
                cardImage[i].sprite = typeImage[3];
            }
            else if (outcomeType[i] == (int)deckTypeEnum.Item)
            {
                inMarket[i].retailType = deckTypeEnum.Item;
            }

            inMarket[i].hasBought = false;
            eventText.SetText("Select a Resource to buy, Confirm with Spacebar & Finish Shopping with Backspace");
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
        marketUI.SetActive(false);
    }

    public void DecidingUp(object sender, EventArgs e)
    {
        selectedStock = inMarket[0];
        boughtStock = 0;
        eventText.SetText(selectedStock.retailObject.ToString() + " " + selectedStock.retailType.ToString() + " :" + selectedStock.price.ToString());
    }
    
    public void DecidingDown(object sender, EventArgs e)
    {
        selectedStock = inMarket[2];
        boughtStock = 2;
        eventText.SetText(selectedStock.retailObject.ToString() + " " + selectedStock.retailType.ToString() + " :" + selectedStock.price.ToString());
    }
    
    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedStock = inMarket[3];
        boughtStock = 3;
        eventText.SetText(selectedStock.retailObject.ToString() + " " + selectedStock.retailType.ToString() + " :" + selectedStock.price.ToString());
    }
    
    public void DecidingRight(object sender, EventArgs e)
    {
        selectedStock = inMarket[1];
        boughtStock = 1;
        eventText.SetText(selectedStock.retailObject.ToString() + " " + selectedStock.retailType.ToString() + " :" + selectedStock.price.ToString());
    }
    
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedStock.retailObject == marketEnum.Null)
        {
            eventText.SetText("You didn't choose an item yet");
        }
        else if (selectedStock.hasBought)
        {
            eventText.SetText("Stock has been sold out");
        }
        else if (selectedStock.price > controller.GetModel.CurrentCash)
        {
            eventText.SetText("You cannot afford to obtain that resource");
        }
        else
        {
            if (selectedStock.retailObject == marketEnum.Item)
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

        checkingAvailability[4] = itemDeck.GetAvailableItem();
        GameObject relic = checkingAvailability[4];
        if (relic != null)
        {
            relic.SetActive(true);
            itemBehaviour item = relic.AddComponent<itemBehaviour>();
            item.CreateItem(selectedItem);
            controller.IncrementDeck(deckTypeEnum.Item);
            controller.ChangeCash(-selectedStock.price);
            selectedStock.hasBought = true;
            inMarket[boughtStock].hasBought = true;
            sectionDisplay[boughtStock].color = setColour[4];
            itemUI[boughtStock].SetActive(false);
            eventText.SetText("item Obtained: " + item.Item.itemName + " : " + item.Item.itemDescription + " Press Backspace once you're done shopping");
        }
        else
        {
            eventText.SetText("There is no Available Slot for Items");
        }

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

            checkingAvailability[0] = offenceDeck.GetAvailableOffence();
            GameObject offenceCard = checkingAvailability[0];
            if (offenceCard != null)
            {
                offenceCard.SetActive(true);
                offenceCard offence = offenceCard.AddComponent<offenceCard>();
                offence.CreateCard(selectedOffenceCard);
                controller.IncrementDeck(deckTypeEnum.Offence);
                controller.ChangeCash(-selectedStock.price);
                selectedStock.hasBought = true;
                inMarket[boughtStock].hasBought = true;
                sectionDisplay[boughtStock].color = setColour[4];
                cardUI[boughtStock].SetActive(false);
                eventText.SetText("Offence Card Obtained: " + offence.AttackCard.cardName + " : " + offence.AttackCard.cardDescription + " Press Backspace once you're done shopping");
            }
            else
            {
                eventText.SetText("There is no Available Slot for Offence Cards");
            }
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

            checkingAvailability[1] = defenceDeck.GetAvailableDefence();
            GameObject defenceCard = checkingAvailability[1];
            if (defenceCard != null)
            {
                defenceCard.SetActive(true);
                defenceCard defence = defenceCard.AddComponent<defenceCard>();
                defence.CreateCard(selectedDefenceCard);
                controller.IncrementDeck(deckTypeEnum.Defence);
                controller.ChangeCash(-selectedStock.price);
                selectedStock.hasBought = true;
                inMarket[boughtStock].hasBought = true;
                sectionDisplay[boughtStock].color = setColour[4];
                cardUI[boughtStock].SetActive(false);
                eventText.SetText("Defence Card Obtained: " + defence.DefendCard.cardName + " : " + defence.DefendCard.cardDescription + " Press Backspace once you're done shopping");
            }
            else
            {
                eventText.SetText("There is no Available Slot for Defence Cards");
            }
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

            checkingAvailability[2] = movementDeck.GetAvailableMovement();
            GameObject moveCard = checkingAvailability[2];
            if (moveCard != null)
            {
                moveCard.SetActive(true);
                movementCard move = moveCard.AddComponent<movementCard>();
                move.CreateCard(selectedMovementCard);
                controller.IncrementDeck(deckTypeEnum.Movement);
                controller.ChangeCash(-selectedStock.price);
                selectedStock.hasBought = true;
                inMarket[boughtStock].hasBought = true;
                sectionDisplay[boughtStock].color = setColour[4];
                cardUI[boughtStock].SetActive(false);
                eventText.SetText("Movement Card Obtained: " + move.MoveCard.cardName + " : " + move.MoveCard.cardDescription + " Press Backspace once you're done shopping");
            }
            else
            {
                eventText.SetText("There is no Available Slot for Movement Cards");
            }

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

            checkingAvailability[3] = statusDeck.GetAvailableStatus();
            GameObject statCard = checkingAvailability[3];
            if (statCard != null)
            {
                statCard.SetActive(true);
                statusCard stat = statCard.AddComponent<statusCard>();
                stat.CreateCard(selectedStatusCard);
                controller.IncrementDeck(deckTypeEnum.Status);
                controller.ChangeCash(-selectedStock.price);
                selectedStock.hasBought = true;
                inMarket[boughtStock].hasBought = true;
                sectionDisplay[boughtStock].color = setColour[4];
                cardUI[boughtStock].SetActive(false);
                eventText.SetText("Status Card Obtained: " + stat.StatusCard.cardName + " : " + stat.StatusCard.cardDescription + " Press Backspace once you're done shopping");
            }
            else
            {
                eventText.SetText("There is no Available Slot for Status Cards");
            }
        }

    }

    IEnumerator EndShopping()
    {
        eventText.SetText("Ending Shopping");
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
