using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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


    public void CreateCard(offenceCardStats newCard)
    {
        AttackCard = newCard;
        attackValue = attackCard.attackValue;
        manaCost = attackCard.manaCost;
        gameObject.name = attackCard.name;
    }
}
