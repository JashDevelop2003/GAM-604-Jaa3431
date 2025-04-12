using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Luck of Your Life is Gambler's One Use Ability that provides 1 of the 6 possible outcome:
/// - Jumping Jackpot = Gain 300 Cash
/// - Buff Bonanza = Apply Lucky, Hasty, Healthy & Invincible for 3 turns
/// - Random Relic = Obtain a Random Relic
/// - Suprised Steal = Steal 20 Cash from Each Opponent
/// - Healthy Wealthy = Gain Health = Cash, Then Gain Cash = Health
/// - Cashout Crashout = Lose All Cash
/// </summary>

public class luckOfYourLife : MonoBehaviour
{
    private playerController controller;
    private currentBuffs buffs;
    private turnManager turnManager;
    private GameObject opponent;
    private int outcome;

    //This is for the relics and provides the possible relics to select from to give to the player landing on the item space
    [SerializeField] private List<itemStats> possibleRelics;
    private itemStats selectedRelic;
    private itemDeckPool itemDeck;


    void Awake()
    {
        controller = GetComponentInParent<playerController>();
        buffs = GetComponentInParent<currentBuffs>();
        itemDeck = transform.parent.GetComponentInChildren<itemDeckPool>();
        turnManager = Singleton<turnManager>.Instance;
        controller.oneUseEvent += LuckOfYourLife;
        possibleRelics = controller.GetData.possibleRelics;

    }

    public void LuckOfYourLife(object sender, EventArgs e)
    {
        controller.oneUseEvent -= LuckOfYourLife;
        outcome = UnityEngine.Random.Range(1, 7);
        if (outcome == 1)
        {
            JumpingJackpot();
        }
        else if (outcome == 2)
        {
            BuffBonanza();
        }
        else if (outcome == 3)
        {
            RandomRelic();
        }
        else if (outcome == 4)
        {
            SuprisedSteal();
        }
        else if (outcome == 5) 
        { 
            HealthyWealthy();
        }
        else if(outcome == 6)
        {
            CashoutCrashout();
        }
        else
        {
            Debug.LogError("Int value was inefficient towards choosing 1 of the 6 values, check again");
        }

    }

    void JumpingJackpot()
    {
        controller.ChangeCash(300);
        Debug.Log("Jumping Jackpot = Gain 300");
    }

    void BuffBonanza()
    {
        buffs.AddBuff(buffEnum.Lucky, 3, 0);
        buffs.AddBuff(buffEnum.Hasty, 3, 0);
        buffs.AddBuff(buffEnum.Healthy, 3, 0);
        buffs.AddBuff(buffEnum.Invincible, 3, 0);
        Debug.Log("Buff Bonanza = Apply Lucky, Healthy, Hasty & Invincible for 3 turns");
    }

    void RandomRelic()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleRelics.Count);
        selectedRelic = possibleRelics[selectedInt];
        GameObject relic = itemDeck.GetAvailableItem();
        if (relic != null)
        {
            relic.SetActive(true);
            itemBehaviour item = relic.AddComponent<itemBehaviour>();
            item.CreateItem(selectedRelic);
            controller.IncrementDeck(deckTypeEnum.Item);
            Debug.Log("Random Relic = Obtain Random Relic: " + item.Item.itemName + " : " + item.Item.itemDescription);
        }
        else
        {
            Debug.Log("Random Relic = Obtain Random Relic");
        }
    }

    void SuprisedSteal()
    {
        for (int i = 0; i < turnManager.GetPlayers.Length; i++)
        {
            if (turnManager.GetPlayers[i].gameObject != this.gameObject)
            {
                opponent = turnManager.GetPlayers[i].gameObject;
                playerController opponentController = opponent.GetComponent<playerController>();
                controller.ChangeCash(20);
                opponentController.ChangeCash(-20);
            }
        }
        Debug.Log("Suprised Steal = Steal 20 Cash from Each Opponent");
    }

    void HealthyWealthy()
    {
        controller.ChangeHealth(controller.GetModel.CurrentCash);
        controller.ChangeCash(controller.GetModel.CurrentHealth);
        Debug.Log("Healthy Whealthy = Gain Health = Cash and then Gain Cash = Health");
    }

    void CashoutCrashout()
    {
        controller.ChangeCash(-controller.GetModel.CurrentCash);
        Debug.Log("Lose All your cash :(");
    }

    private void OnDisable()
    {
        controller.oneUseEvent -= LuckOfYourLife;
    }
}
