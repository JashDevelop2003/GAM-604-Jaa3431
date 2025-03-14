using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yourSoulIsMineEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;
    private GameObject opponent;

    ///This should be used for all additional effects
    void Awake()
    {
        offenceCard = GetComponentInParent<offenceCard>();
        combatSystem = combatSystem.instance;
        offenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent += YourSoulIsMine;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Your Soul is Mine instantly defeats the opponent under these 2 conditions:
    // - The player (who's reaper) is in their Last Reapsort Ability
    // - The player Dealt at least 1 damage to the opponent
    public void YourSoulIsMine(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        lastReapsort playerAbility = player.GetComponentInChildren<lastReapsort>();

        opponent = combatSystem.DefendingPlayer;
        playerController opponentController = opponent.GetComponent<playerController>();

        if(playerAbility.LastReapsortActive)
        {
            if (combatSystem.AttackValue > combatSystem.DefendValue) 
            {
                opponentController.GetModel.IsAlive = false;
                playerAbility.LastReapsortActive = false;
                Debug.Log("You dealt Damage. Opponent is Defeated");
            }

            else
            {
                Debug.Log("You didn't dealt damage. Opponent is Alive");
            }
        }

        else
        {
            Debug.Log("You weren't in Last Reapsort Status. Opponent is Alive");
        }

    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= YourSoulIsMine;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
