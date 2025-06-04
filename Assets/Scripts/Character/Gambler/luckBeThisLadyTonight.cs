using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
/// <summary>
/// Luck in this Lady Tonight is the Gambler's Passive Ability
/// At the start of the turn the gambler can have between 1 to 30 mana
/// The Gambler also gains cash = mana
/// </summary>

public class luckBeThisLadyTonight : MonoBehaviour
{
    private dataManager dataManager;
    private playerController controller;
    private playerView view;
    private startState startState;
    private inactiveState inactiveState;
    private int randomMana;

    // Start is called before the first frame update
    void Awake()
    {
        dataManager = Singleton<dataManager>.Instance;
        controller = GetComponentInParent<playerController>();
        view = GetComponentInParent<playerView>();
        startState = GetComponentInParent<startState>();
        inactiveState = GetComponentInParent<inactiveState>();
        //This has to go in the item events because placing it inside the start events with the stat reset will always set the mana to 0
        startState.startItemEvents += LuckInThisLadyTonight;
        inactiveState.endEvents += RandomiseMana;
        controller.DisplayAbility(controller.GetData.abilityIcon[0], controller.GetData.abilityColour[0]);
        StartCoroutine(LoadMana());
    }

    public IEnumerator LoadMana()
    {
        yield return new WaitUntil(() => dataManager.LoadComplete == true);
        LuckData data = luckSystem.Retrieve();
        if (data != null)
        {
            controller.GetModel.CurrentMana = data.storedMana;
            randomMana = data.randomisedMana;
            view.ManaUI();
        }
        else
        {
            Debug.LogError("Something went wrong with the luck data");
        }
    }

    public void LuckInThisLadyTonight(object sender, EventArgs e)
    {
        controller.GetModel.CurrentMana = randomMana;
        view.ManaUI();
        controller.ChangeCash(randomMana);
    }

    public void RandomiseMana(object sender, EventArgs e)
    {
        randomMana = UnityEngine.Random.Range(1, 31);

        LuckData data = new LuckData
        {
            storedMana = controller.GetModel.CurrentMana,
            randomisedMana = randomMana,
        };

        luckSystem.Store(data);
    }

    private void OnDisable()
    {
        startState.startItemEvents -= LuckInThisLadyTonight;
        inactiveState.endEvents -= RandomiseMana;
    }
}
