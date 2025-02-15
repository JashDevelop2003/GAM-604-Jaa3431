using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementCard : MonoBehaviour
{
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

    public void CreateCard(movementCardStats newCard)
    {
        MoveCard = newCard;
        rollMinimumValue = MoveCard.minimumMoveValue;
        rollMaximumValue = MoveCard.maximumMoveValue;
        manaCost = moveCard.manaCost;
        gameObject.name = moveCard.name;

        movementDeckPile deck = GetComponentInParent<movementDeckPile>();
        deck.AddCard(this.gameObject);
    }
}
