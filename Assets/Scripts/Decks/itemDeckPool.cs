using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private soundManager soundManager;

    //These are the variable to create and store the cards
    private int selectedInt;
    private itemStats selectedItem;
    private Transform playerTransform;
    private GameObject player;

    [Header("User Interface")]
    [SerializeField] private TMP_Text eventText;

    [Header("Audio")]
    [SerializeField] private AudioClip[] confirmSound = new AudioClip[2];

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
        soundManager = Singleton<soundManager>.Instance;

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

    public void CreateItem(itemEnum type)
    {
        //The transform and gameobject are used to identify the player to find the data
        playerTransform = this.transform.parent;
        player = playerTransform.gameObject;

        if (type == itemEnum.Relic)
        {
            selectedInt = Random.Range(0, controller.GetData.possibleRelics.Count);
            selectedItem = controller.GetData.possibleRelics[selectedInt];
        }
        else if(type == itemEnum.Omen)
        {
            selectedInt = Random.Range(0, controller.GetData.possibleOmens.Count);
            selectedItem = controller.GetData.possibleOmens[selectedInt];
        }

        GameObject chosenItem = GetAvailableItem();
        chosenItem.SetActive(true);
        itemBehaviour item = chosenItem.AddComponent<itemBehaviour>();
        item.CreateItem(selectedItem);
        controller.IncrementDeck(deckTypeEnum.Item);
        soundManager.PlaySound(confirmSound[(int)type]);
        eventText.SetText("Obtained " + type.ToString() + " : " + item.Item.itemName + " : " + item.Item.itemDescription);
        if (controller.Player == 1)
        {
            playerOneData playerData = player.GetComponentInChildren<playerOneData>();
            if(type == itemEnum.Relic)
            {
                playerData.storedRelics.Add(selectedInt);

            }
            else if(type == itemEnum.Omen)
            {
                playerData.storedOmens.Add(selectedInt);
            }
        }
        else if (controller.Player == 2)
        {
            playerTwoData playerData = player.GetComponentInChildren<playerTwoData>();
            if (type == itemEnum.Relic)
            {
                playerData.storedRelics.Add(selectedInt);

            }
            else if (type == itemEnum.Omen)
            {
                playerData.storedOmens.Add(selectedInt);
            }
        }
    }
}
