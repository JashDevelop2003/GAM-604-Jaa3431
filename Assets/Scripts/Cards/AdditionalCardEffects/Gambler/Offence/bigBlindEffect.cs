using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigBlindEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += BigBlind;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Big Blind Applies Blind to Opponent for 4 turns
    public void BigBlind(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects addEffects = opponent.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Blind, 4);
        combatSystem.OffenceValue.SetText("Apply Blind to Defender for 4 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= BigBlind;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
