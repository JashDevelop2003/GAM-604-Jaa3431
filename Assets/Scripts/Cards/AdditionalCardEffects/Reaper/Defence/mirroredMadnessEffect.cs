using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirroredMadnessEffect : MonoBehaviour
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
        combatSystem.duringCombatEvent += MirroredMadness;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Mirrored Madness Deals Damage to Opponent based on the attack value
    public void MirroredMadness(object sender, EventArgs e)
    {
        opponent = combatSystem.AttackingPlayer;
        playerController opponentController = opponent.GetComponent<playerController>();
        opponentController.ChangeHealth(-combatSystem.AttackValue);
        combatSystem.DefenceValue.SetText("The Attacker is also recieving: " + combatSystem.AttackValue.ToString() + " damage");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= MirroredMadness;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
