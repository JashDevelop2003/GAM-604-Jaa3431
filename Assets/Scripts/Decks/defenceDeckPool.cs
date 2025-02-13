using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defenceDeckPool : MonoBehaviour
{
    //The deckpool will need to reference itself to identify the which deck is being pooled
    public static defenceDeckPool instance;

    //this enum provides the correct deck capacity to match with the player's character stats
    deckTypeEnum deckType = deckTypeEnum.Defence;

    //The list is used to collect all of the pooled objects from this deck pool manager
    private List<GameObject> emptyDefenceCards = new List<GameObject>();

    //this list provides all the starting cards that the player should have
    //this is based upon the character data from the player controller (that has the model for the data)
    private List<defenceCardStats> startingDefenceCards;

    //this integer is used to pool a certain amount of empty cards
    private int amountToPool;

    //This is the empty prefab that should provide the offence card prefab
    [SerializeField] private GameObject emptyPrefabs;

    private void Awake()
    {
        //In order to decide on the amount of objects to pool and the starting cards, the method must collect the player controller from the parent
        playerController player = GetComponentInParent<playerController>();
        //This collects from the character data on the starting offence cards and deck capacity based upon the type of deck
        startingDefenceCards = player.GetData.startingDefenceCards;
        amountToPool = player.GetData.deckCapacity[(int)deckType];

        //this creates it's own static deck pool in order to pool the objects and be use for referencing
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Depending on the value of the capacity, create cards of that type into the deck
        //Make sure that the cards are derived classes to the deck
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject card = Instantiate(emptyPrefabs, this.transform);
            card.SetActive(false);
            emptyDefenceCards.Add(card);
        }

        CreateStartingDefenceCards();
    }

    public GameObject GetAvailableDefence()
    {
        //This loop checks if there's any remaining defence cards to be made
        for (int i = 0; i < emptyDefenceCards.Count; i++)
        {
            //If there is an empty card slot then return the value of that empty slot in the list
            if (!emptyDefenceCards[i].activeInHierarchy)
            {
                return emptyDefenceCards[i];
            }
        }
        //otherwise the loop will return as nothing to the code and should inform the player that they have to create another type of card
        return null;
    }

    //this method creates all of the starting cards and will be useful loading back the cards in the game
    private void CreateStartingDefenceCards()
    {
        //This loop creates every starting card until all of the starting cards are made
        for (int i = 0; i < startingDefenceCards.Count; i++)
        {
            //This checks if there is any available defence card to be use
            //If there is then this will set the card to active and enable in the hierarchy
            GameObject card = instance.GetAvailableDefence();
            if (card != null)
            {
                card.SetActive(true);
                //this will then add the defence card component into the deck & add the defence card data in the card object
                defenceCard defence = card.AddComponent<defenceCard>();
                defence.CreateCard(startingDefenceCards[i]);
            }
        }
    }
}
