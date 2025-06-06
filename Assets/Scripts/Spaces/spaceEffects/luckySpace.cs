using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// This is the lucky space component that once called
/// </summary>

public class luckySpace : MonoBehaviour
{
    //this is to obtain the player and their state
    [SerializeField] private GameObject luckyPlayer;
    private playerStateManager playerState;
    private playerController playerController;

    [SerializeField] private TMP_Text eventText;

    public void beginLucky(GameObject player, int outcome)
    {
        //lucky player turns to player in order to have the variable use for obtaining specific resources
        luckyPlayer = player;
        playerState = player.GetComponent<playerStateManager>();
        playerController = player.GetComponent<playerController>();

        //If the outcome is 1 then obtain a relic
        if (outcome == 1)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Item));
            eventText.SetText("Outcome: Obtain Relic");
        }

        //if the outcome is 2 then obtain a legendary offence card
        else if (outcome == 2)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Offence));
            eventText.SetText("Outcome: Obtain Legendary Offence Card");
        }

        //if the outcome is 3 then obtain a legendary defence card
        else if (outcome == 3) 
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Defence));
            eventText.SetText("Outcome: Obtain Legendary Defence Card");
        }

        //if the outcome is 4 then obtain a legendary movement card
        else if(outcome == 4)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Movement));
            eventText.SetText("Outcome: Obtain Legendary Movement Card");
        }

        //if the outcome is 5 then obtain a legendary status card
        else if (outcome == 5)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Status));
            eventText.SetText("Outcome: Obtain Legendary Status Card");
        }

        //if the outcome is 6 then gain 100 cash
        else if (outcome == 6)
        {
            playerController.ChangeCash(100);
            eventText.SetText("Outcome: Gain 100 Cash");
            StartCoroutine(EndLucky());
        }

        //if the outcome is 7 then gain 25% Health
        else if(outcome == 7)
        {
            playerController.ChangeHealth((int)(playerController.GetModel.MaxHealth * 0.25f));
            eventText.SetText("Outcome: Heal 25% of Max Health");
            StartCoroutine(EndLucky());
        }


        /// Invincibility & Lucky has to have a cooldown of 4 since this occurs at the end
        
        //if the outcome is 8 then gain Invincibility for 3 turns
        else if(outcome == 8)
        {
            currentBuffs buffs = player.GetComponent<currentBuffs>();
            buffs.AddBuff(buffEnum.Invincible, 4, 0);
            eventText.SetText("Outcome: Gain Invincibility for 3 turns");
            StartCoroutine(EndLucky());
        }

        //if the outcome is 9 then gain lucky for 3 turns
        else if(outcome == 9)
        {
            currentBuffs buffs = player.GetComponent<currentBuffs>();
            buffs.AddBuff(buffEnum.Lucky, 4, 0);
            eventText.SetText("Outcome: Gain Lucky for 3 turns");
            StartCoroutine(EndLucky());
        }

        //if the outcome is 10 then gain hasty for 3 turns
        else if(outcome == 10)
        {
            currentBuffs buffs = player.GetComponent<currentBuffs>();
            buffs.AddBuff(buffEnum.Hasty, 3, 0);
            eventText.SetText("Outcome: Gain Hasty for 3 turns");
            StartCoroutine(EndLucky());
        }
    }

    IEnumerator ObtainingResource(deckTypeEnum type)
    {
        yield return new WaitForSeconds(2);
        if (type == deckTypeEnum.Offence)
        {
            ObtainOffence();
        }
        else if (type == deckTypeEnum.Defence)
        {
            ObtainDefence();
        }
        else if (type == deckTypeEnum.Movement)
        {
            ObtainMovement();
        }
        else if (type == deckTypeEnum.Status) 
        { 
            ObtainStatus();
        }
        else if(type == deckTypeEnum.Item)
        {
            ObtainRelic();
        }
    }

    private void ObtainOffence()
    {

        //This section checks if the player can obtain the offence card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        offenceDeckPool offencePool = luckyPlayer.GetComponentInChildren<offenceDeckPool>();
        GameObject offenceCard = offencePool.GetAvailableOffence();
        if (offenceCard != null)
        {
            offencePool.CreateCard(CardRarity.Legendary);
        }

        else if (offenceCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Offence cards");
        }
        StartCoroutine(EndLucky());
    }

    private void ObtainDefence()
    {

        //This section checks if the player can obtain the defence card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        defenceDeckPool defencePool = luckyPlayer.GetComponentInChildren<defenceDeckPool>();
        GameObject defenceCard = defencePool.GetAvailableDefence();
        if (defenceCard != null)
        {
            defencePool.CreateCard(CardRarity.Legendary);
        }

        else if (defenceCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Defence cards");
        }
        StartCoroutine(EndLucky());
    }

    private void ObtainMovement()
    {

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        movementDeckPool movePool = luckyPlayer.GetComponentInChildren<movementDeckPool>();
        GameObject moveCard = movePool.GetAvailableMovement();
        if (moveCard != null)
        {
            movePool.CreateCard(CardRarity.Legendary);
        }

        else if (moveCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Movement cards");
        }
        StartCoroutine(EndLucky());
    }

    private void ObtainStatus()
    {

        //This section checks if the player can obtain the status card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        statusDeckPool statusPool = luckyPlayer.GetComponentInChildren<statusDeckPool>();
        GameObject statusCard = statusPool.GetAvailableStatus();
        if (statusCard != null)
        {
            statusPool.CreateCard(CardRarity.Legendary);
        }

        else if (statusCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Status cards");
        }
        StartCoroutine(EndLucky());
    }

    private void ObtainRelic()
    {

        //This section checks if the player can obtain the relic in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the relic
        //Otherwise they get nothing
        itemDeckPool itemDeck = luckyPlayer.GetComponentInChildren<itemDeckPool>();
        GameObject relic = itemDeck.GetAvailableItem();
        if(relic != null)
        {
            itemDeck.CreateItem(itemEnum.Relic);
        }

        else if(relic == null)
        {
            eventText.SetText("Unlucky, You don't have any room for more relics");
        }
        StartCoroutine(EndLucky());
    }

    IEnumerator EndLucky()
    {
        yield return new WaitForSeconds(2);
        playerState.ChangeState(playerState.InactiveState);
        
    }
}
