using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blessOrHex : eventSpace
{
    private turnManager turnManager;
    private soundManager soundManager;
    private playerController controller;

    private List<itemStats> possibleRelics;
    private itemStats selectedRelic;

    private List<itemStats> possibleOmens;
    private itemStats selectedOmens;

    private int selectedInt;

    [Header("User Interface")]
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] outcomeSounds = new AudioClip[2];



    // Start is called before the first frame update
    void Start()
    {
        turnManager = Singleton<turnManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
    }

    public override void ActivateEvent()
    {
        int Outcome = Random.Range(0, 2);
        if (Outcome == 0)
        {
            ObtainRelic();
        }
        else if (Outcome == 1)
        {
            ObtainOmen();
        }
        else
        {
            Debug.LogError("Outcome was not sutiable towards it's value");
        }
        soundManager.PlaySound(outcomeSounds[Outcome]);
        StartCoroutine(EndTurn());
    }

    void ObtainRelic()
    {
        
        //This obtains the character data of the possible relics the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        controller = turnManager.CurrentPlayer.GetComponent<playerController>();
        possibleRelics = controller.GetData.possibleRelics;
        selectedInt = Random.Range(0, possibleRelics.Count);
        selectedRelic = possibleRelics[selectedInt];

        //This section checks if the player can obtain the relic in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the relic
        //Otherwise they get nothing
        itemDeckPool itemDeck = turnManager.CurrentPlayer.GetComponentInChildren<itemDeckPool>();
        GameObject relic = itemDeck.GetAvailableItem();
        if (relic != null)
        {
            relic.SetActive(true);
            itemBehaviour item = relic.AddComponent<itemBehaviour>();
            item.CreateItem(selectedRelic);
            controller.IncrementDeck(deckTypeEnum.Item);
            eventText.SetText("Congratulations! you recieved a relic. You Obtained: " + item.Item.itemName + " : " + item.Item.itemDescription);
            if (controller.Player == 1)
            {
                playerOneData playerData = GetComponentInChildren<playerOneData>();
                playerData.storedRelics.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = GetComponentInChildren<playerTwoData>();
                playerData.storedDefence.Add(selectedInt);
            }
        }

        else if (relic == null)
        {
            eventText.SetText("Congratulations! you recieved a relic. However, You don't have any room for more relics so unlucky.");
        }

        //This then clears the list of possible relics and turns the selected relic to null and ends the player's turn
        selectedRelic = null;
        possibleRelics = null;
    }

    void ObtainOmen()
    {
        Debug.Log("Obtaining a Omen");


        //This obtains the character data of the possible relics the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        controller = turnManager.CurrentPlayer.GetComponent<playerController>();
        possibleOmens = controller.GetData.possibleOmens;
        selectedInt = Random.Range(0, possibleOmens.Count);
        selectedOmens = possibleOmens[selectedInt];

        //This section checks if the player can obtain the relic in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the relic
        //Otherwise they get nothing
        itemDeckPool itemDeck = turnManager.CurrentPlayer.GetComponentInChildren<itemDeckPool>();
        GameObject omen = itemDeck.GetAvailableItem();
        if (omen != null)
        {
            omen.SetActive(true);
            itemBehaviour item = omen.AddComponent<itemBehaviour>();
            item.CreateItem(selectedOmens);
            controller.IncrementDeck(deckTypeEnum.Item);
            eventText.SetText("How unfortunate! You recieved a omen. You Obtained: " + item.Item.itemName + " : " + item.Item.itemDescription);
            if (controller.Player == 1)
            {
                playerOneData playerData = turnManager.CurrentPlayer.GetComponentInChildren<playerOneData>();
                playerData.storedOmens.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = turnManager.CurrentPlayer.GetComponentInChildren<playerTwoData>();
                playerData.storedOmens.Add(selectedInt);
            }
        }

        else if (omen == null)
        {
            eventText.SetText("How unfortunate! you recieved a omen. However, You don't have any room for more omens so quite lucky I supposed.");
        }

        //This then clears the list of possible relics and turns the selected relic to null and ends the player's turn
        selectedOmens = null;
        possibleOmens = null;
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(4);
        playerStateManager currentPlayer = turnManager.CurrentPlayer.GetComponent<playerStateManager>();
        currentPlayer.ChangeState(currentPlayer.InactiveState);
    }
}
