using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void CreateCard(defenceCardStats newCard)
    {
        DefendCard = newCard;
        defendValue = defendCard.defendValue;
        manaCost = defendCard.manaCost;
        gameObject.name = defendCard.name;
    }
}
