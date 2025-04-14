using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyCollisionEffect : MonoBehaviour
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
        combatSystem.duringCombatEvent += BodyCollision;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Body Collision Deals Damage to Attacker = Damage Recieved
    public void BodyCollision(object sender, EventArgs e)
    {
        if (combatSystem.AttackValue > combatSystem.DefendValue) 
        {
            opponent = combatSystem.AttackingPlayer;
            playerController controller = opponent.GetComponent<playerController>();
            controller.ChangeHealth(-(combatSystem.AttackValue - combatSystem.DefendValue));
            combatSystem.DefenceValue.SetText("Deals: " + (combatSystem.AttackValue - combatSystem.DefendValue) + " Damage to Attacker");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= BodyCollision;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
