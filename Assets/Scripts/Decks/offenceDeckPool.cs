using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is alternative solution of the deck pool where instead of 1 Pool Script There's multiple
/// This one being the offence deck where the object pool only creates new offence decks
/// This creates new offence cards in order for the cards to be enable when played and possibly disable if removing is involved
/// </summary>

public class offenceDeckPool : MonoBehaviour
{

    //The deckpool will need to reference itself to identify the which deck is being pooled
    //public static offenceDeckPool instance;
    
    //this enum provides the correct deck capacity to match with the player's character stats
    deckTypeEnum deckType = deckTypeEnum.Offence;

    //The list is used to collect all of the pooled objects from this deck pool manager
    private List<GameObject> emptyOffenceCards = new List<GameObject>();
    
    //this list provides all the starting cards that the player should have
    //this is based upon the character data from the player controller (that has the model for the data)
    private List<offenceCardStats> startingOffenceCards;
    public List<GameObject> OffenceCard
    {
        get { return emptyOffenceCards; }
    }

    //this integer is used to pool a certain amount of empty cards
    private int amountToPool;

    //This is the empty prefab that should provide the offence card prefab
    [SerializeField] private GameObject emptyPrefabs;

    private playerController controller;
    

    // Start is called before the first frame update
    void Start()
    {
        //In order to decide on the amount of objects to pool and the starting cards, the method must collect the player controller from the parent
        controller = GetComponentInParent<playerController>();
        //This collects from the character data on the starting offence cards and deck capacity based upon the type of deck
        startingOffenceCards = controller.GetData.startingOffenceCards;
        amountToPool = controller.GetData.deckCapacity[(int)deckType];

        //Depending on the value of the capacity, create cards of that type into the deck
        //Make sure that the cards are derived classes to the deck
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject card = Instantiate(emptyPrefabs, this.transform);
            card.SetActive(false);
            emptyOffenceCards.Add(card);
        }

        //At the start of the game there needs to be a call for collecting the starting offence cards
        CreateStartingOffenceCards();
    }

    public GameObject GetAvailableOffence()
    {
        //This loop checks if there's any remaining offence cards to be made
        for (int i = 0; i < emptyOffenceCards.Count; i++)
        {
            //If there is an empty card slot then return the value of that empty slot in the list
            if (!emptyOffenceCards[i].activeInHierarchy)
            {
                return emptyOffenceCards[i];
            }
        }
        //otherwise the loop will return as nothing to the code and should inform the player that they have to create another type of card
        return null;
    }


    //this method creates all of the starting cards and will be useful loading back the cards in the game
    private void CreateStartingOffenceCards()
    {
        //This loop creates every starting card until all of the starting cards are made
        for (int i = 0; i < startingOffenceCards.Count; i++)
        {
            //This checks if there is any available offence card to be use
            //If there is then this will set the card to active and enable in the hierarchy
            GameObject card = GetAvailableOffence();
            if (card != null)
            {
                card.SetActive(true);
                //this will then add the offence card component into the deck & add the offence card data in the card object
                offenceCard offence = card.AddComponent<offenceCard>();
                offence.CreateCard(startingOffenceCards[i]);
            }
        }
    }

    public void LoadOffenceCards(int id)
    {
        GameObject card = GetAvailableOffence();
        if (card != null) 
        {
            card.SetActive(true);
            offenceCard offence = card.AddComponent<offenceCard>();
            offence.CreateCard(controller.GetData.possibleOffenceCards[id]);
            controller.IncrementDeck(deckTypeEnum.Offence);
        }
    }
}
