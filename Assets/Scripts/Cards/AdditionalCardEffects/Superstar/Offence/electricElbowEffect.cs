using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricElbowEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += ElectricElbow;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Electric Elbow Applies Shock to Opponent for 1 turn
    public void ElectricElbow(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects addEffects = opponent.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Shocked, 1);
        combatSystem.OffenceValue.SetText("Apply Shock to Defender for 1 turn");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= ElectricElbow;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
