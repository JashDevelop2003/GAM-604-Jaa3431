using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class itemDeckPool : MonoBehaviour
{
    //this enum provides the correct deck capacity to match with the player's character stats
    deckTypeEnum deckType = deckTypeEnum.Item;

    //The list is used to collect all of the pooled objects from this deck pool manager
    private List<GameObject> emptyItems = new List<GameObject>();
    public List<GameObject> Items
    {
        get { return emptyItems; }
    }

    //this list provides all the starting items that the player should have
    //this is based upon the character data from the player controller (that has the model for the data)
    private List<itemStats> startingItems;

    //this integer is used to pool a certain amount of empty items
    private int amountToPool;

    //This is the empty prefab that should provide the item prefab
    [SerializeField] private GameObject emptyPrefabs;

    private playerController controller;



    private void Awake()
    {
        //In order to decide on the amount of objects to pool and the starting items, the method must collect the player controller from the parent
        controller = GetComponentInParent<playerController>();
        //This collects from the character data on the starting items and deck capacity based upon the type of deck
        startingItems = controller.GetData.startingItems;
        amountToPool = controller.GetData.deckCapacity[(int)deckType];

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


    public void LoadRelics(int id)
    {
        GameObject item = GetAvailableItem();
        if (item != null)
        {
            item.SetActive(true);
            itemBehaviour relic = item.AddComponent<itemBehaviour>();
            relic.LoadItem(controller.GetData.possibleRelics[id]);
            controller.IncrementDeck(deckTypeEnum.Item);
        }
    }

    public void LoadOmens(int id)
    {
        GameObject item = GetAvailableItem();
        if (item != null)
        {
            item.SetActive(true);
            itemBehaviour omen = item.AddComponent<itemBehaviour>();
            omen.LoadItem(controller.GetData.possibleOmens[id]);
            controller.IncrementDeck(deckTypeEnum.Item);
        }
    }
}
