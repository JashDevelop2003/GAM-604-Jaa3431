using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defence Cards are when set active for the defence deck pool will provide suitable card data to be use for reducing damage
/// Once this card is created, the card is sent to the deck pile for the card to be used during the defending state
/// </summary>

public class defenceCard : MonoBehaviour
{
    [SerializeField] private defenceCardStats defendCard;
    public defenceCardStats DefendCard
    {
        get { return defendCard; }
        set { defendCard = value; }
    }

    [SerializeField] private int defendValue;
    public int DefendValue
    {
        get { return defendValue; }
        set { defendValue = value; }
    }

    [SerializeField] private int manaCost;
    public int ManaCost
    {
        get { return manaCost; }
        set { manaCost = value; }
    }

    //This is the additional event that will invoke if the card has any additional effects
    public event EventHandler additionalEvent;

    //this method is called from either the picking state or the defence deck pool itself and provides the stats onto the object
    //This will then add the card into the offence deck to be used during the defend state
    public void CreateCard(defenceCardStats newCard)
    {
        DefendCard = newCard;
        defendValue = defendCard.defendValue;
        manaCost = defendCard.manaCost;
        gameObject.name = defendCard.cardName;
        if (newCard.additionalEffect != null)
        {
            Instantiate(newCard.additionalEffect, this.transform);
        }

        defenceDeckPile deck = GetComponentInParent<defenceDeckPile>();
        deck.AddCard(this.gameObject);
    }

    public void ApplyAdditionalEffect()
    {
        additionalEvent?.Invoke(this, EventArgs.Empty);
    }
}
