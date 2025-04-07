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
    //this is for the movement cards and provides the possible movement card they can select from the character data
    private movementDeckPool moveDeck;
    [SerializeField] private List<movementCardStats> possibleMovementCards;
    [SerializeField] private movementCardStats selectedMovementCard;

    //This is for the offence cards and provides the possible offence card they can select from the character data
    private offenceDeckPool offenceDeck;
    [SerializeField] private List<offenceCardStats> possibleOffenceCards;
    [SerializeField] private offenceCardStats selectedOffenceCard;

    //This is for the defence cards and provides the possible defence card they can select from the character data
    private defenceDeckPool defenceDeck;
    [SerializeField] private List<defenceCardStats> possibleDefenceCards;
    [SerializeField] private defenceCardStats selectedDefenceCard;

    private statusDeckPool statusDeck;
    [SerializeField] private List<statusCardStats> possibleStatusCards;
    [SerializeField] private statusCardStats selectedStatusCard;

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
        possibleDefenceCards = controller.GetData.possibleDefenceCards;
        possibleOffenceCards = controller.GetData.possibleOffenceCards;
        possibleMovementCards = controller.GetData.possibleMovementCards;
        possibleStatusCards = controller.GetData.possibleStatusCards;

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
        int selectedInt = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
        selectedOffenceCard = possibleOffenceCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedOffenceCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleOffenceCards.Count);
            selectedOffenceCard = possibleOffenceCards[selectedInt];
        }

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        offenceDeck = player.GetComponentInChildren<offenceDeckPool>();
        GameObject attackCard = offenceDeck.GetAvailableOffence();
        if (attackCard != null)
        {
            attackCard.SetActive(true);
            offenceCard attack = attackCard.AddComponent<offenceCard>();
            attack.CreateCard(selectedOffenceCard);
            controller.IncrementDeck(deckTypeEnum.Offence);
        }
    }

    void LegendaryDefence()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
        selectedDefenceCard = possibleDefenceCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedDefenceCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleDefenceCards.Count);
            selectedDefenceCard = possibleDefenceCards[selectedInt];
        }

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        defenceDeck = player.GetComponentInChildren<defenceDeckPool>();
        GameObject defendCard = defenceDeck.GetAvailableDefence();
        if (defendCard != null)
        {
            defendCard.SetActive(true);
            defenceCard defend = defendCard.AddComponent<defenceCard>();
            defend.CreateCard(selectedDefenceCard);
            controller.IncrementDeck(deckTypeEnum.Defence);
        }
    }

    void LegendaryMovement()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleMovementCards.Count);
        selectedMovementCard = possibleMovementCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedMovementCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleMovementCards.Count);
            selectedMovementCard = possibleMovementCards[selectedInt];
        }

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        moveDeck = player.GetComponentInChildren<movementDeckPool>();
        GameObject moveCard = moveDeck.GetAvailableMovement();
        if (moveCard != null)
        {
            moveCard.SetActive(true);
            movementCard move = moveCard.AddComponent<movementCard>();
            move.CreateCard(selectedMovementCard);
            controller.IncrementDeck(deckTypeEnum.Movement);
        }
    }

    void LegendaryStatus()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleStatusCards.Count);
        selectedStatusCard = possibleStatusCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedStatusCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = UnityEngine.Random.Range(0, possibleStatusCards.Count);
            selectedStatusCard = possibleStatusCards[selectedInt];
        }

        //This section checks if the player can obtain the status card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        statusDeck = player.GetComponentInChildren<statusDeckPool>();
        GameObject statusCard = statusDeck.GetAvailableStatus();
        if (statusCard != null)
        {
            statusCard.SetActive(true);
            statusCard status = statusCard.AddComponent<statusCard>();
            status.CreateCard(selectedStatusCard);
            controller.IncrementDeck(deckTypeEnum.Status);
        }
    }
}
