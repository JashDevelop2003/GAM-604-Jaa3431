using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speenAttack : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;

    void Awake()
    {
        offenceCard = GetComponentInParent<offenceCard>();
        combatSystem = combatSystem.instance;
        offenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += SpeenAttack;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void SpeenAttack(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        currentEffects playerEffect = player.GetComponent<currentEffects>();
        playerEffect.AddEffect(effectEnum.Stunned, 1);
        combatSystem.OffenceValue.SetText("Player is Confused for 3 Turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= SpeenAttack;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
