using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;
    private int pierced = 0;

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
        combatSystem.beforeCombatEvent += Pierce;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Pierce Only Deals Damage when Aggressive
    //Pierce Ignores the Defend Value Entirely
    public void Pierce(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        passiveAgression playerStance = player.GetComponentInChildren<passiveAgression>();
        if(playerStance.Stance == stanceEnum.Aggressive)
        {
            combatSystem.DefendValue = pierced;
            combatSystem.OffenceValue.SetText("Pierce Successful, Defend Value at: " + pierced);

        }
        else
        {
            combatSystem.AttackValue = pierced;
            combatSystem.OffenceValue.SetText("Pierce Failed, Attack value at: " + pierced);
        }    
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= Pierce;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
