using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Passive Agression is a passive ability that belongs to the Wielder Character
/// The ability has 2 stances:
/// - Passive which Doubles Guard, However Halves Thrust
/// - Aggressive which Doubles Thrust, However Halves Guard
/// The ability switches to the other stance once either these three conditions:
/// - During 3 of the Player's Turn
/// - The One Use Ability
/// - TODO: Specifc Cards provide an effect onto the card
/// </summary>

public class passiveAgression : MonoBehaviour
{
    //This is the controller which will need to reference the player object into the parent
    private playerController controller;

    //This checks the state of the player
    private playerStateManager stateManager;

    //These are required to add the methods to suitable events
    private combatSystem combatSystem;
    private startState state;

    //This is the stance which will be use to identify which stance the character is currently on
    //This will be use for the battle system to check which stance the character is on to double/half thrust or guard
    [SerializeField] private stanceEnum stance;
    public stanceEnum Stance
    {
        get { return stance; } 
        set { stance = value; }   
    }

    //this is the starting stance which will make the character be on a random stance during the beginning of the game
    private int startingStance;

    //This is the cooldown to provide on the character
    [SerializeField] private int changeCooldown;

    private dataManager dataManager;

    //When awake the class has to gather the controller component and then decide if the player starts their character of with being passive or aggressive
    void Awake()
    {
        controller = GetComponentInParent<playerController>();
        state = GetComponentInParent<startState>();
        stateManager = GetComponentInParent<playerStateManager>();
        combatSystem = combatSystem.instance;
        dataManager = Singleton<dataManager>.Instance;
        dataManager.saveFiles += SaveStance;

        combatSystem.beforeCombatEvent += BattleStance;
        state.startEvent += DecrementCooldown;

        StanceData stancedata = stanceSystem.Retrieve();
        if (stancedata != null)
        {
            if (stancedata.stance == (int)stanceEnum.Aggressive)
            {
                stance = stanceEnum.Aggressive;
            }
            else
            {
                stance = stanceEnum.Passive;

            }
            controller.DisplayAbility(controller.GetData.abilityIcon[(int)stance], controller.GetData.abilityColour[(int)stance]);
            changeCooldown = stancedata.stanceCooldown;
        }
    }

    public void SaveStance(object sender, EventArgs e)
    {
        StanceData stanceData = new StanceData
        {
            stance = (int)stance,
            stanceCooldown = changeCooldown,
        };

        stanceSystem.Store(stanceData);
    }

    //During the start state, the event will invoke this method which decrements the cooldown
    //If the cooldown is equal to 0 then this will call change stance method
    public void DecrementCooldown(object sender, EventArgs e)
    {
        changeCooldown--;
        if (changeCooldown == 0) 
        {
            ChangeStance();
        }
    }

    //This changes the current stance to the other stance and resets the cooldown to 3 turns.
    public void ChangeStance()
    {
        if (stance == stanceEnum.Passive) 
        {
            stance = stanceEnum.Aggressive;
        }
        else if (stance == stanceEnum.Aggressive)
        {
            stance = stanceEnum.Passive;
        }
        controller.DisplayAbility(controller.GetData.abilityIcon[(int)stance], controller.GetData.abilityColour[(int)stance]);

        changeCooldown = 3;
    }

    //Before Combat, the value doubles or half depending on the player's state and the stance they're currently in
    //Attack Doubles if stance is Agressive, however Defend Halves in that stance
    //Defend Dobules if stance is Passive, howeveder Attack halves in that stance
    public void BattleStance(object sender, EventArgs e)
    {
        if (stateManager.CurrentState == stateManager.AttackState) 
        {
            if (stance == stanceEnum.Passive) 
            {
                combatSystem.AttackValue /= 2;
            }

            else if(stance == stanceEnum.Aggressive)
            {
                combatSystem.AttackValue *= 2;
            }
        }

        else if(stateManager.CurrentState == stateManager.DefendState)
        {
            if (stance == stanceEnum.Aggressive)
            {
                combatSystem.DefendValue /= 2;
            }

            else if (stance == stanceEnum.Passive)
            {
                combatSystem.DefendValue *= 2;
            }
        }
    }

    private void OnDisable()
    {
        dataManager.saveFiles -= SaveStance;
        state.startEvent -= DecrementCooldown;
        combatSystem.beforeCombatEvent -= BattleStance;
    }



}
