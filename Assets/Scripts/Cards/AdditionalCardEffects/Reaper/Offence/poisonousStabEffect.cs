using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poisonousStabEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += PoisonousStab;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Poisonous Stab Applies Poison to Opponent for 2 turns
    public void PoisonousStab(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects addEffects = opponent.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Poison, 2);
        combatSystem.OffenceValue.SetText("Apply Poison to Defender for 2 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= PoisonousStab;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
