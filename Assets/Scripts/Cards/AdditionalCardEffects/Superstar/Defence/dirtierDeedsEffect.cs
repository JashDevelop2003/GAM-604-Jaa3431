using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirtierDeedsEffect : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    private GameObject opponent;

    private void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent += Counter;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Dirtier Deeds Halves the Attack Value & Deals 20 Damage to Attacker
    public void Counter(object sender, EventArgs e)
    {

        opponent = combatSystem.AttackingPlayer;
        playerController controller = opponent.GetComponent<playerController>();
        controller.ChangeHealth(-20);
        combatSystem.AttackValue /= 2;
        combatSystem.DefenceValue.SetText("Defender Deals 20 Damage to Attacker & Halves the Attack");

    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= Counter;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
