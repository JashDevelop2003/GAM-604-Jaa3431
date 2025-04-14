using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areYaGonnaMakeAMoveEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += AreYaGonnaMakeAMove;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Are Ya Gonna Make A Move Applies Slow to Self for 3 turns
    public void AreYaGonnaMakeAMove(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        currentEffects addEffects = player.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Slowed, 3);
        combatSystem.OffenceValue.SetText("Apply Slow to Self for 3 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= AreYaGonnaMakeAMove;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
