using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pollutedProtectionEffect : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    private GameObject opponent;

    ///This should be used for all additional effects
    void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += PollutedProtection;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Polluted Protection Applies Poison to both Opponent for 4 Turns
    public void PollutedProtection(object sender, EventArgs e)
    {
        opponent = combatSystem.AttackingPlayer;
        currentEffects opponentEffects = opponent.GetComponent<currentEffects>();
        opponentEffects.AddEffect(effectEnum.Poison, 4);
        combatSystem.DefenceValue.SetText("Attacker is Applied with Poison for 4 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= PollutedProtection;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
