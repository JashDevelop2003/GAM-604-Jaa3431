using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Luck in this Lady Tonight is the Gambler's Passive Ability
/// At the start of the turn the gambler can have between 1 to 30 mana
/// The Gambler also gains cash = mana
/// </summary>

public class luckBeThisLadyTonight : MonoBehaviour
{
    private playerController controller;
    private playerView view;
    private startState state;
    private int randomMana;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponentInParent<playerController>();
        view = GetComponentInParent<playerView>();
        state = GetComponentInParent<startState>();
        //This has to go in the item events because placing it inside the start events with the stat reset will always set the mana to 0
        state.startItemEvents += LuckInThisLadyTonight;
        controller.DisplayAbility(controller.GetData.abilityIcon[0], controller.GetData.abilityColour[0]);
    }

    public void LuckInThisLadyTonight(object sender, EventArgs e)
    {
        randomMana = UnityEngine.Random.Range(1, 31);
        controller.GetModel.CurrentMana = randomMana;
        view.ManaUI();
        controller.ChangeCash(randomMana);
    }

    private void OnDisable()
    {
        state.startItemEvents -= LuckInThisLadyTonight;
    }
}
