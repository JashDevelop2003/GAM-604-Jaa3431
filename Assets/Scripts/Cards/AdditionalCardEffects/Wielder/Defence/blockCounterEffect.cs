using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockCounterEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    [SerializeField] private GameObject opponent;

    private void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent += BlockCounter;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Block Counter Deals 10 Damage to the Attacker if the Defend Value is Higher than the Attack Value
    public void BlockCounter(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        if(combatSystem.DefendValue >= combatSystem.AttackValue)
        {
            playerController controller = opponent.GetComponent<playerController>();
            controller.ChangeHealth(-10);
            Debug.Log("COUNTER!");
        }
        else
        {
            Debug.Log("No Counter? :(");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= BlockCounter;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
