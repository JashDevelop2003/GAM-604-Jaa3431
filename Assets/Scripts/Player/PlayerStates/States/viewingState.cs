using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System;
/// <summary>
/// The viewing state is when the player can choose to view their cards and items in their inventory
/// The state will include looking at one of the cards that includes
/// </summary>

[System.Serializable]
public struct resources
{
    public List<GameObject> resourceList;
}

public class viewingState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, ICancel
{
    [SerializeField] private int type;
    [SerializeField] private int resource;
    private bool endViewing;

    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    //The array list is based on the order shown in the deck pool
    private offenceDeckPool offenceDeck;
    private defenceDeckPool defenceDeck;
    private movementDeckPool movementDeck;
    private statusDeckPool statusDeck;
    private itemDeckPool itemDeck;

    [SerializeField] public resources[] resourceType = new resources[5];

    [Header("User Interface")]
    //Display the UI
    [SerializeField] private GameObject viewingUI;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text eventText;
    
    //Displaying the cards
    [SerializeField] private GameObject cardUI;
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite[] cardSprite = new Sprite[4];
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardMana;
    [SerializeField] private TMP_Text cardDetail;

    //Displaying the items
    [SerializeField] private GameObject itemUI;
    [SerializeField] private Image itemImage;
    [SerializeField] private Color[] typeColour = new Color[2];
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemType;

    public override void EnterState(playerStateManager player)
    {
        type = 0;
        resource = 0;
        endViewing = false;
        //resourceType = new resources[5];

        offenceDeck = GetComponentInChildren<offenceDeckPool>();
        defenceDeck = GetComponentInChildren<defenceDeckPool>();
        movementDeck = GetComponentInChildren<movementDeckPool>();
        statusDeck = GetComponentInChildren<statusDeckPool>();
        itemDeck = GetComponentInChildren<itemDeckPool>();

        for (int i = 0; i < offenceDeck.OffenceCard.Count; i++) 
        { 
            if(offenceDeck.OffenceCard[i].activeInHierarchy)
            {
                resourceType[0].resourceList.Add(offenceDeck.OffenceCard[i]);
            }
        }

        for (int i = 0; i < defenceDeck.DefenceCard.Count; i++)
        {
            if (defenceDeck.DefenceCard[i].activeInHierarchy)
            {
                resourceType[1].resourceList.Add(defenceDeck.DefenceCard[i]);
            }
        }

        for (int i = 0; i < movementDeck.MovementCard.Count; i++)
        {
            if (movementDeck.MovementCard[i].activeInHierarchy)
            {
                resourceType[2].resourceList.Add(movementDeck.MovementCard[i]);
            }
        }

        for (int i = 0; i < statusDeck.StatusCard.Count; i++)
        {
            if (statusDeck.StatusCard[i].activeInHierarchy)
            {
                resourceType[3].resourceList.Add(statusDeck.StatusCard[i]);
            }
        }

        for (int i = 0; i < itemDeck.Items.Count; i++)
        {
            if (itemDeck.Items[i].activeInHierarchy)
            {
                resourceType[4].resourceList.Add(itemDeck.Items[i]);
            }
        }


        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.cancelPressed += Cancel;

        viewingUI.SetActive(true);
        cardUI.SetActive(true);
        itemUI.SetActive(false);
        cardImage.sprite = cardSprite[type];
        offenceCard viewCard = resourceType[type].resourceList[resource].GetComponent<offenceCard>();
        cardName.SetText(viewCard.AttackCard.cardName);
        cardMana.SetText(viewCard.AttackCard.manaCost.ToString());
        cardDetail.SetText(viewCard.AttackCard.attackValue.ToString());
        descriptionText.SetText(viewCard.AttackCard.cardDescription);
        eventText.SetText("Press Backsapce when you want to stop viewing your deck");

    }
    public override void UpdateState(playerStateManager player)
    {
        if (endViewing)
        {
            player.ChangeState(player.DecidingState);
        }
    }
    public override void ExitState(playerStateManager player) 
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.cancelPressed -= Cancel;

        viewingUI.SetActive(false);


        for (int i = 0; i < resourceType.Length; i++) 
        {
            resourceType[i].resourceList.Clear();
        }
    }

    //These change the current viewing card.
    public void DecidingUp(object sender, EventArgs e)
    {
        ChangeType(-1);
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        ChangeType(1);
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        ChangeResource(-1);
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        ChangeResource(1);
    }

    //When the player is done viewing, the player can press backspace to go back to deciding
    public void Cancel(object sender, EventArgs e)
    {
        endViewing = true;
    }

    void ChangeType(int change)
    {
        type += change;

        if (type < 0)
        {
            type = resourceType.Length - 1;
        }
        if (type >= resourceType.Length) 
        {
            type = 0;
        }
        if(type == (int)deckTypeEnum.Item && resourceType[type].resourceList.Count == 0)
        {
            if(change > 0)
            {
                type = 0;
            }
            else
            {
               type += change;
            }
        }
        resource = 0;
        DisplayType();
    }

    void ChangeResource(int change)
    {
        resource += change;
        if(resource < 0)
        {
            resource = resourceType[type].resourceList.Count - 1;
        }
        if(resource >= resourceType[type].resourceList.Count)
        {
            resource = 0;
        }
        DisplayResource();
    }

    void DisplayType()
    {
        //Checks if the type is a card or resource
        if(type == (int)deckTypeEnum.Item)
        {
            cardUI.SetActive(false);
            itemUI.SetActive(true);
        }
        else
        {
            cardUI.SetActive(true);
            itemUI.SetActive(false);
            cardImage.sprite = cardSprite[type];
        }
        resource = 0;
        DisplayResource();
    }

    void DisplayResource()
    {
        if (type == (int)deckTypeEnum.Offence)
        {
            offenceCard viewCard = resourceType[type].resourceList[resource].GetComponent<offenceCard>();
            cardName.SetText(viewCard.AttackCard.cardName);
            cardMana.SetText(viewCard.AttackCard.manaCost.ToString());
            cardDetail.SetText(viewCard.AttackCard.attackValue.ToString());
            descriptionText.SetText(viewCard.AttackCard.cardDescription);
        }

        else if (type == (int)deckTypeEnum.Defence)
        {
            defenceCard viewCard = resourceType[type].resourceList[resource].GetComponent<defenceCard>();
            cardName.SetText(viewCard.DefendCard.cardName);
            cardMana.SetText(viewCard.DefendCard.manaCost.ToString());
            cardDetail.SetText(viewCard.DefendCard.defendValue.ToString());
            descriptionText.SetText(viewCard.DefendCard.cardDescription);
        }

        else if (type == (int)deckTypeEnum.Movement)
        {
            movementCard viewCard = resourceType[type].resourceList[resource].GetComponent<movementCard>();
            cardName.SetText(viewCard.MoveCard.cardName);
            cardMana.SetText(viewCard.MoveCard.manaCost.ToString());
            if (viewCard.MoveCard.minimumMoveValue != viewCard.MoveCard.maximumMoveValue) 
            {
                cardDetail.SetText("Rolls between" + viewCard.MoveCard.minimumMoveValue.ToString() + " to " + viewCard.MoveCard.maximumMoveValue.ToString());
            }
            else
            {
                cardDetail.SetText("Rolls between" + viewCard.MoveCard.minimumMoveValue.ToString());
            }
            descriptionText.SetText(viewCard.MoveCard.cardDescription);
        }

        else if (type == (int)deckTypeEnum.Status)
        {
            statusCard viewCard = resourceType[type].resourceList[resource].GetComponent<statusCard>();
            cardName.SetText(viewCard.StatusCard.cardName);
            cardMana.SetText(viewCard.StatusCard.manaCost.ToString());
            cardDetail.SetText(viewCard.StatusCard.target.ToString());
            descriptionText.SetText(viewCard.StatusCard.cardDescription);
        }

        else if (type == (int)deckTypeEnum.Item)
        {
            itemBehaviour viewItem = resourceType[type].resourceList[resource].GetComponent<itemBehaviour>();
            itemName.SetText(viewItem.Item.itemName);
            itemType.SetText(viewItem.Item.itemType.ToString());
            itemImage.color = typeColour[(int)viewItem.Item.itemType];
            descriptionText.SetText(viewItem.Item.itemDescription);
        }
    }
}
