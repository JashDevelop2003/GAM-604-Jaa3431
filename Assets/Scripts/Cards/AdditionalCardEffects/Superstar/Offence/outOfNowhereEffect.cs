using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfNowhereEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
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
        combatSystem.beforeCombatEvent += OutOfNowhere;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Out Of Nowhere Deals Damage and Applies Shock to Defender for 2 turns if their Defence is Below 7
    // If Defend Value is Equal to or Above 7 then the Attack Value becomes 0
    public void OutOfNowhere(object sender, EventArgs e)
    {
        if(combatSystem.DefendValue >= 7)
        {
            combatSystem.AttackValue = 0;
            combatSystem.OffenceValue.SetText("Attack Failed");
        }
        else
        {
            opponent = combatSystem.DefendingPlayer;
            currentEffects addEffects = opponent.GetComponent<currentEffects>();
            addEffects.AddEffect(effectEnum.Shocked, 2);
            combatSystem.OffenceValue.SetText("Attack Successs: Apply Shock to Defender for 2 turns");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= OutOfNowhere;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
