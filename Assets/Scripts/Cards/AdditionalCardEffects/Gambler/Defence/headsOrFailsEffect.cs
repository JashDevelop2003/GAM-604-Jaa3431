using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headsOrFailsEffect : MonoBehaviour
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
        combatSystem.beforeCombatEvent += HeadsOrFails;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Heads or Fails has a 50% Chance for Defend Value = 0
    public void HeadsOrFails(object sender, EventArgs e)
    {
        int outcome = UnityEngine.Random.Range(0, 2);
        if (outcome == 1) 
        {
            combatSystem.DefendValue = 0;
            combatSystem.DefenceValue.SetText("Unlucky: " + combatSystem.DefendValue);
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= HeadsOrFails;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
