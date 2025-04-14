using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterEffect : MonoBehaviour
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

    //Counter has a 80% Negates the Attack & Deals Damage to Attacker Equal to the Attack Value
    public void Counter(object sender, EventArgs e)
    {
        int outcome = UnityEngine.Random.Range(0, 5);
        if (outcome == 0)
        {
            combatSystem.DefenceValue.SetText("Counter has Failed");
        }

        else
        {
            opponent = combatSystem.AttackingPlayer;
            playerController controller = opponent.GetComponent<playerController>();
            controller.ChangeHealth(-combatSystem.AttackValue);
            combatSystem.DefenceValue.SetText("Defender Deals: " + combatSystem.AttackValue + " Damage to Attacker");
            combatSystem.AttackValue = 0;
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= Counter;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
