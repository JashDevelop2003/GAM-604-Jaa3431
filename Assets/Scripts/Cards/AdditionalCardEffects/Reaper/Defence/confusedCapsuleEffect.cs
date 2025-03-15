using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class confusedCapsuleEffect : MonoBehaviour
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
        combatSystem.duringCombatEvent += ConfusedCapsule;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Confused Capsule Confuses the player for 2 turns
    public void ConfusedCapsule(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        currentEffects playerEffects = player.GetComponent<currentEffects>();
        playerEffects.AddEffect(effectEnum.Confused, 2);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= ConfusedCapsule;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
