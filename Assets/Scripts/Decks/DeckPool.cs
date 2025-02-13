using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This is the object pooling for every deck
/// This will require an enum in order to provide collect the suitable deck capacity & type of card
/// </summary>
public class DeckPool : MonoBehaviour
{
    //The deckpool will need to reference itself to identify the which deck is being pooled
    public static DeckPool instance;

    //The enum is used for a integer value to provide the specific capacity and card array
    public enum DeckType {Offence, Defence, Movement, Status, Curse};
    [SerializeField] private DeckType Type;

    //The list is used to collect all of the pooled objects from this deck pool manager
    private List<GameObject> emptyCards = new List<GameObject>();
    
    //this integer is used to pool a certain amount of empty cards
    private int amountToPool;

    //the game object is in array and will be use to collect a specifc object from the array
    [SerializeField] private GameObject[] emptyCardsPrefabs = new GameObject[5];

    //This is to collect the starting cards for the deck
    private List<object> startingCards = new List<object>();


    private List<offenceCardStats> startingOffenceCards;
    private List<defenceCardStats> startingDefenceCards;
    private List<movementCardStats> startingMovementCards;
    
    //This will need to provide the deck to pool along with the correct amount to pool capacity from the character data
    void Awake()
    {
       //the amount of pool is equal to the chracter data (from the player controller) depending on the integer of that type
       // Offence is 0, Defence is 1, Movement is 2, Status is 3 & Curse is 4
       //amountToPool = GetComponentInParent<playerController>().GetData.deckCapacity[(int)Type];
        playerController player = GetComponentInParent<playerController>();
        amountToPool = player.GetData.deckCapacity[(int)Type];
        startingOffenceCards = player.GetData.startingOffenceCards;
        startingDefenceCards = player.GetData.startingDefenceCards;
        startingMovementCards = player.GetData.startingMovementCards;

        if(Type == DeckType.Offence)
        {
            foreach (var card in startingOffenceCards) 
            {
                startingCards.Add(card);
            }
            
            Debug.Log(startingCards.Count);
            Debug.Log(startingCards[0]);
            Debug.Log(startingCards[1]);
            Debug.Log(startingCards[2]);
            Debug.Log(startingCards[3]);


        }

        if (instance == null)
        {
            instance = this;
        }
    }
    
    //At the start the deck pool has to instantiate the empty cards with the suitable type
    void Start()
    {
        //Depending on the value of the capacity, create cards of that type into the deck
        //Make sure that the cards are derived classes to the deck
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject card = Instantiate(emptyCardsPrefabs[(int)Type], this.transform);
            card.SetActive(false);
            emptyCards.Add(card);
        }

        //CreateStartingCards();
    }

    //This method is to check if there is an empty slot for the requested call from another method to create a new card
    public GameObject GetAvailableSlot()
    {
        //This checks for each empty card to see if there is an available slot to create a new card
        for (int i = 0; i < emptyCards.Count; i++) 
        {
            //If there is an empty card slot then return the value of that empty slot in the list
            if (!emptyCards[i].activeInHierarchy)
            {
                return emptyCards[i];
            }
        } 

        //otherwise return the value to nothing
        return null;   
    }

    //TODO: Add Card Objects Before Creating Starting Cards
    private void CreateStartingCards()
    {
        //for (int i = 0; i < startingCards.Count; i++)
        //{
          //  GameObject card = instance.GetAvailableSlot();

          //  if (card != null)
          //  {
          //      card.SetActive(true);
           //     if(Type == DeckType.Offence)
             //   {
               //     offenceCard offence = FindAnyObjectByType<offenceCard>();
                 //   if (offence.AttackCard == null)
                   // {
                     //   offence.getCard = startingCards[i];
                   // }
               // }

           // }

       // }
    }

}
