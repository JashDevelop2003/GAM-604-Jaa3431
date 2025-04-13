using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snoozeYouLoseEffect : MonoBehaviour
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
        combatSystem.duringCombatEvent += SnoozeYouLose;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Snooze You Lose Stuns the Attacker for 1 turn
    public void SnoozeYouLose(object sender, EventArgs e)
    {
        if (combatSystem.DefendValue >= combatSystem.AttackValue) 
        {
            opponent = combatSystem.AttackingPlayer;
            currentEffects opponentEffects = opponent.GetComponent<currentEffects>();
            opponentEffects.AddEffect(effectEnum.Stunned, 1);
            combatSystem.DefenceValue.SetText("Attacker is Stunned for 1 turn (the next turn)");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= SnoozeYouLose;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
