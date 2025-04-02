using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Add buffs for thrust increase
public class rageEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += Rage;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void Rage(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        currentBuffs playerbuff = player.GetComponentInParent<currentBuffs>();
        playerbuff.AddBuff(buffEnum.Impactful, 2, 0.1f);
        combatSystem.OffenceValue.SetText("Buff Player's Impactful by 10% for 1 Turn (Excluding this turn)");

    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= Rage;
    }
}
