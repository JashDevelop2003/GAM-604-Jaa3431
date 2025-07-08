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
    private inactiveState state;
    private currentBuffs buffs;
    private turnManager turnManager;
    private dataManager dataManager;
    private GameObject opponent;
    private int outcome;

    //This is for the relics and provides the possible relics to select from to give to the player landing on the item space
    private itemDeckPool itemDeck;

    private List<buffEnum> bonanzaBuffs = new List<buffEnum>()
    {
        buffEnum.Invincible,
        buffEnum.Healthy,
        buffEnum.Hasty,
        buffEnum.Lucky,
    };

    [Header("User Interface")]
    private decidingState stateUI;

    void Start()
    {
        controller = GetComponentInParent<playerController>();
        state = GetComponentInParent<inactiveState>();
        stateUI = GetComponentInParent<decidingState>();
        buffs = GetComponentInParent<currentBuffs>();
        itemDeck = transform.parent.GetComponentInChildren<itemDeckPool>();

        turnManager = Singleton<turnManager>.Instance;
        dataManager = Singleton<dataManager>.Instance;

        controller.oneUseEvent += LuckOfYourLife;
        state.endEvents += RandomiseOutcome;

        LuckOutcomeData outcomeData = luckOutcomeSystem.Retrieve();
        if (outcomeData != null)
        {
            outcome = outcomeData.luckOutcome;
        }
    }

    public void RandomiseOutcome(object sender, EventArgs e)
    {
        outcome = UnityEngine.Random.Range(1, 7);
        LuckOutcomeData outcomeData = new LuckOutcomeData
        {
            luckOutcome = outcome,
        };        
        luckOutcomeSystem.Store(outcomeData);
    }

    public void LuckOfYourLife(object sender, EventArgs e)
    {
        controller.oneUseEvent -= LuckOfYourLife;
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

    }

    void JumpingJackpot()
    {
        controller.ChangeCash(300);
        stateUI.EventText.SetText("Using Ability - Luck of Your Life: Jumping Jackpot = Gain 300");
    }

    void BuffBonanza()
    {
        for (int i = 0; i < bonanzaBuffs.Count; i++) 
        {
            buffs.AddBuff(bonanzaBuffs[i], 3, 0);
        }
        stateUI.EventText.SetText("Using Ability - Luck of Your Life: Buff Bonanza = Apply Lucky, Healthy, Hasty & Invincible for 3 turns");
    }

    void RandomRelic()
    {
        GameObject relic = itemDeck.GetAvailableItem();
        if (relic != null)
        {
            itemDeck.CreateItem(itemEnum.Relic);
        }
        else
        {
            stateUI.EventText.SetText("Using Ability - Luck of Your Life: Random Relic = Obtain Random Relic. You don't have room unfortunately ;-;");
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
        stateUI.EventText.SetText("Using Ability - Luck of Your Life: Suprised Steal = Steal 20 Cash from Each Opponent");
    } 

    void HealthyWealthy()
    {
        controller.ChangeHealth(controller.GetModel.CurrentCash);
        controller.ChangeCash(controller.GetModel.CurrentHealth);
        stateUI.EventText.SetText("OUsing Ability - Luck of Your Life: Healthy Whealthy = Gain Health = Cash. Then Gain Cash = Health");
    }

    void CashoutCrashout()
    {
        controller.ChangeCash(-controller.GetModel.CurrentCash);
        stateUI.EventText.SetText("Using Ability - Luck of Your Life: Crashout Cashout: Lose All your cash :(");
    }

    private void OnDisable()
    {
        controller.oneUseEvent -= LuckOfYourLife;
        state.endEvents -= RandomiseOutcome;
    }
}
