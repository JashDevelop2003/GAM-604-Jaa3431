using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += Bash;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void Bash(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects opponentEffect = opponent.GetComponent<currentEffects>();
        opponentEffect.AddEffect(effectEnum.Stunned, 1);
        combatSystem.OffenceValue.SetText("Opponent is Stunned");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= Bash;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
