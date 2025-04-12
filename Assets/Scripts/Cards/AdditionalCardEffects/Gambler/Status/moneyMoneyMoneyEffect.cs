using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyMoneyMoneyEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;
    private int amountOfCards;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += MoneyMoneyMoney;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Money, Money, Monet Increase Cash by the amount of cards obtained
    public void MoneyMoneyMoney(object sender, EventArgs e)
    {
        amountOfCards = 0;

        playerController controller = player.GetComponent<playerController>();
        offenceDeckPool offenceDeck = player.GetComponentInChildren<offenceDeckPool>();
        defenceDeckPool defenceDeck = player.GetComponentInChildren<defenceDeckPool>();
        movementDeckPool movementDeck = player.GetComponentInChildren<movementDeckPool>();
        statusDeckPool statusDeck = player.gameObject.GetComponentInChildren<statusDeckPool>();

        for (int i = 0; i < offenceDeck.OffenceCard.Count; i++)
        {
            if (offenceDeck.OffenceCard[i].activeSelf)
            {
                amountOfCards++;
            }
        }

        for (int i = 0; i < defenceDeck.DefenceCard.Count; i++)
        {
            if(defenceDeck.DefenceCard[i].activeSelf)
            {
                amountOfCards++;
            }
        }

        for (int i = 0; i < movementDeck.MovementCard.Count; i++)
        {
            if (movementDeck.MovementCard[i].activeSelf)
            {
                amountOfCards++;
            }
        }

        for (int i = 0; i < statusDeck.StatusCard.Count; i++)
        {
            if (statusDeck.StatusCard[i].activeSelf)
            {
                amountOfCards++;
            }
        }

        controller.ChangeHealth(amountOfCards);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= MoneyMoneyMoney;
    }
}
