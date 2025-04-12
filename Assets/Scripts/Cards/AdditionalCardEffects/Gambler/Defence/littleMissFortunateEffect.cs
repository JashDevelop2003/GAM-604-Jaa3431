using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleMissFortunateEffect : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;

    private void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent += LittleMissFortunate;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Little Miss Fortunate Negates the Attack
    public void LittleMissFortunate(object sender, EventArgs e)
    {
        combatSystem.AttackValue = 0;
        combatSystem.DefenceValue.SetText(combatSystem.DefendValue + " + Negate Attack");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= LittleMissFortunate;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
