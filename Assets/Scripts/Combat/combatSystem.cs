using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// This is the combat system that provides the outcome of the battle
/// This checks the attack and defend values and then calculates the damage
/// If Attack is greater than Defend then the defending player must take damage equal to the difference
/// 
/// </summary>

public class combatSystem : MonoBehaviour
{
    //The attacker provides:
    // - Current Player's Turn Object
    // - Player's controller
    // - Attack Value of the Card they chosen
    // - The multiplier of the thrust
    private GameObject attackingPlayer;
    private playerController attackingPlayerController;
    [SerializeField] private int attackValue;
    [SerializeField] private float thrustMultiplier;
    private bool attackerReady;

    //The defender provides:
    // - Tge player that is on the same space as the current player's turn
    // - Player's controller
    // - Defend Value of the Card they chosen
    // - The multiplier of the guard
    private GameObject defendingPlayer;
    private playerController defendingPlayerController;
    [SerializeField] private int defendValue;
    [SerializeField] private float guardMultiplier;
    private bool defenderReady;

    public event EventHandler combatComplete;

    public void AttackerReady(GameObject attacker, int value)
    {
        //This method collect the defender's object along with the controller to collect the guard multiplier
        attackingPlayer = attacker;
        attackingPlayerController = attackingPlayer.GetComponent<playerController>();
        thrustMultiplier = attackingPlayerController.GetModel.ThrustMultiplier;

        //This is the passive ability that either doubles or halves the wielder's thrust depending on the stance
        //This instance the ability will doulbe the thrust when in aggressive stance
        //And the ability will half the thrust when in passive stance
        if (attackingPlayerController.GetData.character == characterEnum.Wielder) 
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
        if (defendingPlayerController.GetData.character == characterEnum.Wielder)
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
            BattleCalculation();
        }
    }

    public void BattleCalculation()
    {
        if(attackValue > defendValue)
        {
            defendingPlayerController.ChangeHealth(defendValue - attackValue);
        }

        BattleOver();
    }

    private void BattleOver()
    {
        combatComplete?.Invoke(this, EventArgs.Empty);
    }
}
