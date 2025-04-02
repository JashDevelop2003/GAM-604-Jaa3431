using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stabEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject opponent;

    void Awake()
    {
        offenceCard = GetComponentInParent<offenceCard>();
        combatSystem = combatSystem.instance;
        offenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += Stab;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void Stab(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects opponentEffect = opponent.GetComponent<currentEffects>();
        opponentEffect.AddEffect(effectEnum.Bleeding, 3);
        combatSystem.OffenceValue.SetText("Defender is Bleeding for 3 Turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= Stab;
    }
}
