using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passiveIsRelaxiveEffect : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    private GameObject player;

    ///This should be used for all additional effects
    void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent += PassiveIsRelaxive;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Passive Is Relaxive Blocks 40 & Changes stance
    //However the card only works if the player's stance is currently in aggressive
    //Otherwise the card fails to defend
    public void PassiveIsRelaxive(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        passiveAgression playerStance = player.GetComponentInChildren<passiveAgression>();
        if (playerStance.Stance == stanceEnum.Aggressive)
        {
            playerStance.Stance = stanceEnum.Passive;
            combatSystem.DefendValue *= 2;
            Debug.Log("Change Stance to " + playerStance.Stance);
        }
        else
        {
            combatSystem.DefendValue = 0;
            Debug.Log("Defend Value becomes " + combatSystem.DefendValue + " due to being in Aggressive Stance");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= PassiveIsRelaxive;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
