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



    // Start is called before the first frame update
    void Start()
    {
        turnManager = Singleton<turnManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
    }

    public override void ActivateEvent()
    {
        itemDeckPool itemDeck = turnManager.CurrentPlayer.GetComponentInChildren<itemDeckPool>();
        GameObject item = itemDeck.GetAvailableItem();
        if (item != null)
        {
            int Outcome = Random.Range(0, 2);
            if (Outcome == 0)
            {
                itemDeck.CreateItem(itemEnum.Relic);
            }
            else if (Outcome == 1)
            {
                itemDeck.CreateItem(itemEnum.Omen);
            }
        }
        else
        {
            eventText.SetText("You item bag is full, so you won't obtain a item");
        }
        StartCoroutine(EndTurn());
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(4);
        playerStateManager currentPlayer = turnManager.CurrentPlayer.GetComponent<playerStateManager>();
        currentPlayer.ChangeState(currentPlayer.InactiveState);
    }
}
