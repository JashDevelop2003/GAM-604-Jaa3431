using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blessOrHex : eventSpace
{
    private turnManager turnManager;
    private soundManager soundManager;

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
        soundManager.PlaySound(outcomeSounds[Outcome]);
        StartCoroutine(EndTurn());
    }

    void ObtainRelic()
    {

        //This section checks if the player can obtain the relic in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the relic
        //Otherwise they get nothing
        itemDeckPool itemDeck = turnManager.CurrentPlayer.GetComponentInChildren<itemDeckPool>();
        GameObject relic = itemDeck.GetAvailableItem();
        if (relic != null)
        {
            itemDeck.CreateItem(itemEnum.Relic);
        }

        else if (relic == null)
        {
            eventText.SetText("Congratulations! you recieved a relic. However, You don't have any room for more relics so unlucky.");
        }
    }

    void ObtainOmen()
    {

        //This section checks if the player can obtain the relic in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the relic
        //Otherwise they get nothing
        itemDeckPool itemDeck = turnManager.CurrentPlayer.GetComponentInChildren<itemDeckPool>();
        GameObject omen = itemDeck.GetAvailableItem();
        if (omen != null)
        {
            itemDeck.CreateItem(itemEnum.Omen);
        }

        else if (omen == null)
        {
            eventText.SetText("How unfortunate! you recieved a omen. However, You don't have any room for more omens so quite lucky I supposed.");
        }
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(4);
        playerStateManager currentPlayer = turnManager.CurrentPlayer.GetComponent<playerStateManager>();
        currentPlayer.ChangeState(currentPlayer.InactiveState);
    }
}
