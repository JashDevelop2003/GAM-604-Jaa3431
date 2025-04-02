using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiritualShieldEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += SpritualShield;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Spiritual Shield Applies Fear to Opponent for 2 Turns
    public void SpritualShield(object sender, EventArgs e)
    {
        opponent = combatSystem.AttackingPlayer;
        currentEffects opponentEffects = opponent.GetComponent<currentEffects>();
        opponentEffects.AddEffect(effectEnum.Feared, 2);
        combatSystem.DefenceValue.SetText("Attacker is Applied with Feared for 2 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= SpritualShield;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
