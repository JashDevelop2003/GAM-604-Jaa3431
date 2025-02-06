using System.Collections;
using System.Collections.Generic;
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
    
    //This will need to provide the deck to pool along with the correct amount to pool capacity from the character data
    void Awake()
    {
       //the amount of pool is equal to the chracter data (from the player controller) depending on the integer of that type
       // Offence is 0, Defence is 1, Movement is 2, Status is 3 & Curse is 4
       amountToPool = GetComponentInParent<playerController>().GetData.deckCapacity[(int)Type];
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
    }


}
