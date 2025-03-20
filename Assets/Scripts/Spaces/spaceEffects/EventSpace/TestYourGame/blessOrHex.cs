using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blessOrHex : eventSpace
{
    private turnManager turnManager;

    private List<itemStats> possibleRelics;
    private itemStats selectedRelic;

    private List<itemStats> possibleOmens;
    private itemStats selectedOmens;

    private int selectedInt;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = Singleton<turnManager>.Instance;
    }

    public override void ActivateEvent()
    {
        int Outcome = Random.Range(1, 3);
        if (Outcome == 1)
        {
            ObtainRelic();
        }
        else if (Outcome == 2)
        {
            ObtainOmen();
        }
        else
        {
            Debug.LogError("Outcome was not sutiable towards it's value");
        }
        StartCoroutine(EndTurn());
    }

    void ObtainRelic()
    {
        Debug.Log("Obtaining a Relic");
        
        //This obtains the character data of the possible relics the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        playerController controller = turnManager.CurrentPlayer.GetComponent<playerController>();
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
        }

        else if (relic == null)
        {
            Debug.Log("Unlucky, You don't have any room for more relics");
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
        playerController controller = turnManager.CurrentPlayer.GetComponent<playerController>();
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
        }

        else if (omen == null)
        {
            Debug.Log("Luckily You don't have any room for more omens");
        }

        //This then clears the list of possible relics and turns the selected relic to null and ends the player's turn
        selectedOmens = null;
        possibleOmens = null;
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(3);
        playerStateManager currentPlayer = turnManager.CurrentPlayer.GetComponent<playerStateManager>();
        currentPlayer.ChangeState(currentPlayer.InactiveState);
    }
}
