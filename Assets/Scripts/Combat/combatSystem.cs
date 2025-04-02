using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
/// <summary>
/// This is the combat system that provides the outcome of the battle
/// This checks the attack and defend values and then calculates the damage
/// If Attack is greater than Defend then the defending player must take damage equal to the difference
/// 
/// </summary>

public class combatSystem : MonoBehaviour
{
    //The combat system will need to reference itself to provide other abilities & additional effects to be used
    public static combatSystem instance;

    //The attacker provides:
    // - Current Player's Turn Object
    // - Player's controller
    // - Attack Value of the Card they chosen
    // - The multiplier of the thrust
    private GameObject attackingPlayer;
    private playerController attackingPlayerController;
    [SerializeField] private int attackValue;
    private float thrustMultiplier;
    private bool attackerReady;

    //The defender provides:
    // - Tge player that is on the same space as the current player's turn
    // - Player's controller
    // - Defend Value of the Card they chosen
    // - The multiplier of the guard
    private GameObject defendingPlayer;
    private playerController defendingPlayerController;
    [SerializeField] private int defendValue;
    private float guardMultiplier;
    private bool defenderReady;

    //These events handle to additional card effects that occur in the game.
    public event EventHandler combatComplete;
    public event EventHandler beforeCombatEvent;
    public event EventHandler duringCombatEvent;
    public event EventHandler afterCombatEvent;

    //This is the UI texts that are require to identify the value and outcome of the combat
    [SerializeField] private TMP_Text offenceValue;
    [SerializeField] private TMP_Text defenceValue;
    [SerializeField] private TMP_Text eventText;


    //These are essential for additional effects and abilities
    public GameObject DefendingPlayer
    {
        get { return defendingPlayer; }
    }

    public GameObject AttackingPlayer
    {
        get { return attackingPlayer; }
    }
    
    public int AttackValue
    {
        get { return attackValue; }
        set { attackValue = value; }
    }
    public int DefendValue
    {
        get { return defendValue; }
        set { defendValue = value; }
    }

    //this is used to make this a singular instance of the component
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AttackerReady(GameObject attacker, int value)
    {
        //This method collect the defender's object along with the controller to collect the guard multiplier
        attackingPlayer = attacker;
        attackingPlayerController = attackingPlayer.GetComponent<playerController>();
        thrustMultiplier = attackingPlayerController.GetModel.ThrustMultiplier;

        //This is the passive ability that either doubles or halves the wielder's thrust depending on the stance
        //This instance the ability will doulbe the thrust when in aggressive stance
        //And the ability will half the thrust when in passive stance
        if (attackingPlayerController.GetModel.Character == characterEnum.Wielder) 
        { 
            passiveAgression wielderStance = attackingPlayer.GetComponentInChildren<passiveAgression>();
            if(wielderStance.Stance == stanceEnum.Aggressive)
            {
                thrustMultiplier *= 2;
            }
            else
            {
                thrustMultiplier /= 2;
            }
        }

        if (attackingPlayerController.GetModel.Character == characterEnum.Reaper)
        {
            lastReapsort abilityActive = attackingPlayerController.GetComponentInChildren<lastReapsort>();
            if (abilityActive.LastReapsortActive)
            {
                thrustMultiplier *= 3;
            }
        }

        //This calculates the defend value in an integer on the value multiplied by the thrust
        attackValue = (int)(value * thrustMultiplier);
        attackerReady = true;
        CombatReady();

    }

    public void DefenderReady(GameObject defender, int value)
    {
        //This method collect the defender's object along with the controller to collect the guard multiplier
        defendingPlayer = defender;
        defendingPlayerController = defendingPlayer.GetComponent<playerController>();
        guardMultiplier = defendingPlayerController.GetModel.GuardMultiplier;

        //This is the passive ability that either doubles or halves the wielder's thrust depending on the stance
        //This instance the ability will doulbe the thrust when in aggressive stance
        //And the ability will half the thrust when in passive stance
        if (defendingPlayerController.GetModel.Character == characterEnum.Wielder)
        {
            passiveAgression wielderStance = defendingPlayer.GetComponentInChildren<passiveAgression>();
            if (wielderStance.Stance == stanceEnum.Passive)
            {
                guardMultiplier *= 2;
            }
            else
            {
                guardMultiplier /= 2;
            }
        }

        //This calculates the defend value in an integer on the value multiplied by the guard
        defendValue = (int)(value * guardMultiplier);
        defenderReady = true;
        CombatReady();
    }

    private void CombatReady()
    {
        if (attackerReady && defenderReady) 
        {
            beforeCombatEvent?.Invoke(this, EventArgs.Empty);
            StartCoroutine(Calculating());
        }
    }

    //This Coroutine provides time for other methods to call and finish their coding to ready the combat
    IEnumerator Calculating()
    {
        eventText.SetText("Combat Begin!");
        yield return new WaitForSeconds(1);
        offenceValue.SetText("Offence Value: " + attackValue.ToString());
        yield return new WaitForSeconds(2);
        defenceValue.SetText("Defence Value: " + defendValue.ToString());
        yield return new WaitForSeconds(2);
        BattleCalculation();
    }

    public void BattleCalculation()
    {
        //This checks if the attack value is above the defend value
        //If it is then have the defender recieve the difference between the value as damage
        if(attackValue > defendValue)
        {
            eventText.SetText("Defender Recieved " + (defendValue - attackValue).ToString() + " Damage");
            defendingPlayerController.ChangeHealth(defendValue - attackValue);
            
            //this checks if the attacker's character is reaper which allows the reaper to heal 25% damage
            if(attackingPlayerController.GetModel.Character == characterEnum.Reaper)
            {
                //This only allows the reaper to heal if the value is above 4
                //This is because the value will be an int and healing 0 is unecessary
                if(attackValue - defendValue >= 4)
                {
                    attackingPlayerController.ActivatePassive();
                }

                //This checks if the reaper is in her last reapsort form
                //If she is then invoke the new event to check if the opponent is defeated
                lastReapsort abilityActive = attackingPlayerController.GetComponentInChildren<lastReapsort>();
                if (abilityActive.LastReapsortActive)
                {
                    attackingPlayerController.ActivateOneUse();
                }
            }
        }

        else
        {
            eventText.SetText("Defender Didn't Recieved Any Damage");
        }

        StartCoroutine(DuringCombat());
    }

    //This Coroutine is used to provide time during battle calculation
    IEnumerator DuringCombat()
    {
        duringCombatEvent?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(4);
        StartCoroutine(BattleFinished());
    }

    //This Coroutine is used to provide time for applying additional effect that occur.
    IEnumerator BattleFinished()
    {
        afterCombatEvent?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(4);
        BattleOver();
    }

    //Once the battle has finished this turns the booleans to false & invokes each character to the correct state
    //The Attacker will return to the moving state & the Defender will return to the exit state
    private void BattleOver()
    {
        attackerReady = false;
        defenderReady = false;
        combatComplete?.Invoke(this, EventArgs.Empty);
    }
}
