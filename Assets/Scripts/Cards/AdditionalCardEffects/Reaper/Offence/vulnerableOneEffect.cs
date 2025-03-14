using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vulnerableOneEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += VulnerableOne;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Vulnerable One Applies Fear to Opponent for 2 turns
    public void VulnerableOne(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects addEffects = opponent.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Feared, 2);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= VulnerableOne;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
