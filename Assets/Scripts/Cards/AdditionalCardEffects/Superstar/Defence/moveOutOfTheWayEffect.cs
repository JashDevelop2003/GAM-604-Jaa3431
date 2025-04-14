using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveOutOfTheWayEffect : MonoBehaviour
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
        combatSystem.beforeCombatEvent += MoveOutOfTheWay;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Move Out Of The Way has a 50% to Either:
    // - Negate the Attack
    // - Increase the Attack by 10
    public void MoveOutOfTheWay(object sender, EventArgs e)
    {
        int outcome = UnityEngine.Random.Range(0, 2);
        if (outcome == 0)
        {
            combatSystem.AttackValue += 10;
            combatSystem.DefenceValue.SetText("Failed: Attack Increases by 10");
        }

        else
        {
            combatSystem.AttackValue = 0;
            combatSystem.DefenceValue.SetText("Success, Attack becomes 0");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= MoveOutOfTheWay;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
