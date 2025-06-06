using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class godPackEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private itemBehaviour item;
    
    //The decks are reference to identify if there is any avialble slots to create a legendary card
    private movementDeckPool moveDeck;
    private offenceDeckPool offenceDeck;
    private defenceDeckPool defenceDeck;
    private statusDeckPool statusDeck;

    // God Pack gives the player 1 of each type of legendary card (if they have room for it)
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        item = GetComponentInParent<itemBehaviour>();
        item.pickupEvent += UponPickup;
    }
    
    public void UponPickup(object sender, EventArgs e)
    {
        StartCoroutine(ObtainCards());
    }

    IEnumerator ObtainCards()
    {
        yield return new WaitForSeconds(0.5f);
        LegendaryDefence();
        yield return new WaitForSeconds(0.5f);
        LegendaryOffence();
        yield return new WaitForSeconds(0.5f);
        LegendaryMovement();
        yield return new WaitForSeconds(0.5f);
        LegendaryStatus();
    }

    void LegendaryOffence()
    {
        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        offenceDeck = player.GetComponentInChildren<offenceDeckPool>();
        GameObject attackCard = offenceDeck.GetAvailableOffence();
        if (attackCard != null)
        {
            offenceDeck.CreateCard(CardRarity.Legendary);
        }
    }

    void LegendaryDefence()
    {
        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        defenceDeck = player.GetComponentInChildren<defenceDeckPool>();
        GameObject defendCard = defenceDeck.GetAvailableDefence();
        if (defendCard != null)
        {
            defenceDeck.CreateCard(CardRarity.Legendary);
        }
    }

    void LegendaryMovement()
    {

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        moveDeck = player.GetComponentInChildren<movementDeckPool>();
        GameObject moveCard = moveDeck.GetAvailableMovement();
        if (moveCard != null)
        {
            moveDeck.CreateCard(CardRarity.Legendary);
        }
    }

    void LegendaryStatus()
    {

        //This section checks if the player can obtain the status card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        statusDeck = player.GetComponentInChildren<statusDeckPool>();
        GameObject statusCard = statusDeck.GetAvailableStatus();
        if (statusCard != null)
        {
            statusDeck.CreateCard(CardRarity.Legendary);
        }
    }
}
