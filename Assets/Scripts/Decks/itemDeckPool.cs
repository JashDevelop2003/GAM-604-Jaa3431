using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDeckPool : MonoBehaviour
{
    //this enum provides the correct deck capacity to match with the player's character stats
    deckTypeEnum deckType = deckTypeEnum.Item;

    //The list is used to collect all of the pooled objects from this deck pool manager
    private List<GameObject> emptyItems = new List<GameObject>();

    //this list provides all the starting items that the player should have
    //this is based upon the character data from the player controller (that has the model for the data)
    private List<itemStats> startingItems;

    //this integer is used to pool a certain amount of empty items
    private int amountToPool;

    //This is the empty prefab that should provide the item prefab
    [SerializeField] private GameObject emptyPrefabs;



    private void Awake()
    {
        //In order to decide on the amount of objects to pool and the starting items, the method must collect the player controller from the parent
        playerController player = GetComponentInParent<playerController>();
        //This collects from the character data on the starting items and deck capacity based upon the type of deck
        startingItems = player.GetData.startingItems;
        amountToPool = player.GetData.deckCapacity[(int)deckType];

    }

    // Start is called before the first frame update
    void Start()
    {
        //Depending on the value of the capacity, create items of that type into the deck
        //Make sure that the caritemsds are derived classes to the deck
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject card = Instantiate(emptyPrefabs, this.transform);
            card.SetActive(false);
            emptyItems.Add(card);
        }

        //At the start of the game there needs to be a call for collecting the starting items
        CreateStartingItems();
    }

    public GameObject GetAvailableItem()
    {
        //This loop checks if there's any remaining items to be made
        for (int i = 0; i < emptyItems.Count; i++)
        {
            //If there is an empty item slot then return the value of that empty slot in the list
            if (!emptyItems[i].activeInHierarchy)
            {
                return emptyItems[i];
            }
        }
        //otherwise the loop will return as nothing to the code and should inform the player that they cannot obtain any more items
        return null;
    }


    //this method creates all of the starting items and will be useful loading back the items in the game
    private void CreateStartingItems()
    {
        //This loop creates every starting item until all of the starting items are made
        for (int i = 0; i < startingItems.Count; i++)
        {
            //This checks if there is any available items to be use
            //If there is then this will set the item to active and enable in the hierarchy
            GameObject card = GetAvailableItem();
            if (card != null)
            {
                card.SetActive(true);
                //this will then add the items component into the deck & add the items data in the items object
                itemBehaviour offence = card.AddComponent<itemBehaviour>();
                offence.CreateItem(startingItems[i]);
            }

        }
    }
}
