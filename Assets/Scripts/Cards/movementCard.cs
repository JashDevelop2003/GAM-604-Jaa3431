using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement Cards are when set active for the movement deck pool will provide suitable card data to be use for moving around the board
/// Once this card is created, the card is sent to the deck pile for the card to be used during the deciding state
/// </summary>
public class movementCard : MonoBehaviour
{
    //the card collects the card's stats based on roll values and mana cost
    [SerializeField] private movementCardStats moveCard;
    public movementCardStats MoveCard
    {
        get { return moveCard; }
        set { moveCard = value; }
    }

    [SerializeField] private int rollMinimumValue;
    public int RollMinimumValue
    {
        get { return rollMinimumValue; }
        set { rollMinimumValue = value; }
    }

    [SerializeField] private int rollMaximumValue;
    public int RollMaximumValue
    {
        get { return rollMaximumValue; }
        set { rollMaximumValue = value; }
    }

    [SerializeField] private int manaCost;
    public int ManaCost
    {
        get { return manaCost; }
        set { manaCost = value; }
    }

    //This is the additional event that will invoke if the card has any additional effects
    public event EventHandler additionalEvent;

    //this method is called from either the picking state or the movement deck pool itself and provides the stats onto the object
    //This will then add the card into the movement deck to be used during the deciding state
    public void CreateCard(movementCardStats newCard)
    {
        MoveCard = newCard;
        rollMinimumValue = MoveCard.minimumMoveValue;
        rollMaximumValue = MoveCard.maximumMoveValue;
        manaCost = moveCard.manaCost;
        gameObject.name = moveCard.cardName;
        if (newCard.additionalEffect != null)
        {
            Instantiate(newCard.additionalEffect, this.transform);
        }

        movementDeckPile deck = GetComponentInParent<movementDeckPile>();
        deck.AddCard(this.gameObject);
    }

    public void ApplyAdditionalEffect()
    {
        additionalEvent?.Invoke(this, EventArgs.Empty);
    }
}
