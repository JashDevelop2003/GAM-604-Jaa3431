using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class combatSystem : MonoBehaviour
{
    private GameObject attackingPlayer;
    private playerController attackingPlayerController;
    private int attackValue;
    private float thrustMultiplier;
    private bool attackerReady;


    private GameObject defendingPlayer;
    private playerController defendingPlayerController;
    private int defendValue;
    private float guardMultiplier;
    private bool defenderReady;

    public event EventHandler combatComplete;

    public void AttackerReady(GameObject attacker, int value)
    {
        attackingPlayer = attacker;
        attackingPlayerController = attackingPlayer.GetComponent<playerController>();
        thrustMultiplier = attackingPlayerController.GetModel.ThrustMultiplier;
        attackValue = (int)(value * thrustMultiplier);
        attackerReady = true;
        CombatReady();

    }

    public void DefenderReady(GameObject defender, int value)
    {
        defendingPlayer = defender;
        defendingPlayerController = defendingPlayer.GetComponent<playerController>();
        guardMultiplier = defendingPlayerController.GetModel.GuardMultiplier;
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
