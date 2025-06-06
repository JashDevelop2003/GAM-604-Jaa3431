using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// This is alternative solution of the deck pool where instead of 1 Pool Script There's multiple
/// This one being the status deck where the object pool only creates new status cards
/// This creates new status cards in order for the cards to be enable when played and possibly disable if removing is involved
/// </summary>

public class statusDeckPool : MonoBehaviour
{
    //this enum provides the correct deck capacity to match with the player's character stats
    private deckTypeEnum deckType = deckTypeEnum.Status;

    //The list is used to collect all of the pooled objects from this deck pool manager
    private List<GameObject> emptyStatusCards = new List<GameObject>();
    public List<GameObject> StatusCard
    {
        get { return emptyStatusCards; }
    }

    //this list provides all the starting cards that the player should have
    //this is based upon the character data from the player controller (that has the model for the data)
    private List<statusCardStats> startingStatusCards;

    //this integer is used to pool a certain amount of empty cards
    private int amountToPool;

    //This is the empty prefab that should provide the status card prefab
    [SerializeField] private GameObject emptyPrefabs;

    private playerController controller;

    private soundManager soundManager;

    //These are the variable to create and store the cards
    private int selectedInt;
    private statusCardStats selectedCard;
    private Transform playerTransform;
    private GameObject player;

    [Header("User Interface")]
    [SerializeField] private TMP_Text eventText;

    [Header("Audio")]
    [SerializeField] private AudioClip confirmSound;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = Singleton<soundManager>.Instance;

        //In order to decide on the amount of objects to pool and the starting cards, the method must collect the player controller from the parent
        controller = GetComponentInParent<playerController>();
        //This collects from the character data on the starting status cards and deck capacity based upon the type of deck
        startingStatusCards = controller.GetData.startingStatusCards;
        amountToPool = controller.GetData.deckCapacity[(int)deckType];

        //Depending on the value of the capacity, create cards of that type into the deck
        //Make sure that the cards are derived classes to the deck
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject card = Instantiate(emptyPrefabs, this.transform);
            card.SetActive(false);
            emptyStatusCards.Add(card);
        }

        CreateStartingStatusCards();
    }

    public GameObject GetAvailableStatus()
    {
        //This loop checks if there's any remaining status cards to be made
        for (int i = 0; i < emptyStatusCards.Count; i++)
        {
            //If there is an empty card slot then return the value of that empty slot in the list
            if (!emptyStatusCards[i].activeInHierarchy)
            {
                return emptyStatusCards[i];
            }
        }
        //otherwise the loop will return as nothing to the code and should inform the player that they have to create another type of card
        return null;
    }

    //this method creates all of the starting cards and will be useful loading back the cards in the game
    private void CreateStartingStatusCards()
    {
        //This loop creates every starting card until all of the starting cards are made
        for (int i = 0; i < startingStatusCards.Count; i++)
        {
            //This checks if there is any available status card to be use
            //If there is then this will set the card to active and enable in the hierarchy
            GameObject card = GetAvailableStatus();
            if (card != null)
            {
                card.SetActive(true);
                //this will then add the status card component into the deck & add the offence card data in the card object
                statusCard status = card.AddComponent<statusCard>();
                status.CreateCard(startingStatusCards[i]);
            }
        }
    }

    public void LoadStatusCards(int id)
    {
        GameObject card = GetAvailableStatus();
        if (card != null)
        {
            card.SetActive(true);
            statusCard status = card.AddComponent<statusCard>();
            status.CreateCard(controller.GetData.possibleStatusCards[id]);
            controller.IncrementDeck(deckTypeEnum.Status);
        }
    }

    //Create card is a public method that when called will create a new card
    public void CreateCard(CardRarity rarity)
    {
        //The transform and gameobject are used to identify the player to find the data
        playerTransform = this.transform.parent;
        player = playerTransform.gameObject;

        //Do while loop is used to find a card at first
        do
        {
            selectedInt = UnityEngine.Random.Range(0, controller.GetData.possibleStatusCards.Count);
            selectedCard = controller.GetData.possibleStatusCards[selectedInt];
        }
        while (selectedCard.cardRarity != rarity);

        GameObject statusCard = GetAvailableStatus();

        statusCard.SetActive(true);
        statusCard stat = statusCard.AddComponent<statusCard>();

        stat.CreateCard(selectedCard);
        controller.IncrementDeck(deckTypeEnum.Status);

        eventText.SetText(CardType.Status + " Card Obtained: " + stat.StatusCard.cardName);
        soundManager.PlaySound(confirmSound);
        if (controller.Player == 1)
        {
            playerOneData playerData = player.GetComponentInChildren<playerOneData>();
            playerData.storedStatus.Add(selectedInt);
        }
        else if (controller.Player == 2)
        {
            playerTwoData playerData = player.GetComponentInChildren<playerTwoData>();
            playerData.storedStatus.Add(selectedInt);
        }

    }
}
