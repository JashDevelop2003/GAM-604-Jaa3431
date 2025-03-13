using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldChargeEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;

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
        combatSystem.beforeCombatEvent += ShieldCharge;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void ShieldCharge(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        passiveAgression playerStance = player.GetComponentInChildren<passiveAgression>();
        if (playerStance.Stance == stanceEnum.Passive)
        {
            currentBuffs playerBuff = player.GetComponent<currentBuffs>();
            playerBuff.AddBuff(buffEnum.Resistant, 3, 0.1f);
            Debug.Log("Apply Damage to Opponent");
        }
        else
        {
            combatSystem.AttackValue = 0;
            Debug.Log("Attack Value becomes " + combatSystem.AttackValue + " due to being in Aggressive Stance");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= ShieldCharge;
        combatSystem.combatComplete -= RemoveEffect;

    }
}
