using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Offence Cards are when set active for the offence deck pool will provide suitable card data to be use for dealing damage
/// Once this card is created, the card is sent to the deck pile for the card to be used during the attacking state
/// </summary>

public class offenceCard : MonoBehaviour
{
    [SerializeField] private offenceCardStats attackCard;
    public offenceCardStats AttackCard
    {
        get { return attackCard; }
        set { attackCard = value; }
    }
    
    [SerializeField] private int attackValue;
    public int AttackValue
    {
        get { return attackValue; }
        set { attackValue = value; }
    }

    [SerializeField] private int manaCost;
    public int ManaCost
    {
        get { return manaCost; }
        set { manaCost = value; }
    }

    //This is the additional event that will invoke if the card has any additional effects
    public event EventHandler additionalEvent;

    //this method is called from either the picking state or the offence deck pool itself and provides the stats onto the object
    //This will then add the card into the offence deck to be used during the attack state
    public void CreateCard(offenceCardStats newCard)
    {
        AttackCard = newCard;
        attackValue = attackCard.attackValue;
        manaCost = attackCard.manaCost;
        gameObject.name = attackCard.name;
        if (newCard.additionalEffect != null) 
        { 
            Instantiate(newCard.additionalEffect, this.transform);
        }

        offenceDeckPile deck = GetComponentInParent<offenceDeckPile>();
        deck.AddCard(this.gameObject);
    }

    public void ApplyAdditionalEffect()
    {
        additionalEvent?.Invoke(this, EventArgs.Empty);
    }
}
