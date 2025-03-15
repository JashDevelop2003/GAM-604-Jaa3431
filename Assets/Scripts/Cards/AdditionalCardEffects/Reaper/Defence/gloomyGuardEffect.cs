using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gloomyGuardEffect : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    private GameObject player;

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
        combatSystem.duringCombatEvent += GloomyGuard;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Mirrored Madness Deals Damage to Opponent based on the attack value
    public void GloomyGuard(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        currentEffects playerEffects = player.GetComponent<currentEffects>();
        playerEffects.AddEffect(effectEnum.Slowed, 2);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= GloomyGuard;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
