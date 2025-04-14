using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barbedWireBatEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;

    ///This should be used for all additional effects
    void Awake()
    {
        offenceCard = GetComponentInParent<offenceCard>();
        combatSystem = combatSystem.instance;
        offenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent += BarbedwireBat;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Barbed-wire Bat Deals 15 to self if the player fails to deal any damage to defender
    public void BarbedwireBat(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        if (combatSystem.AttackValue <= combatSystem.DefendValue) 
        {
            playerController controller = player.GetComponent<playerController>();
            controller.ChangeHealth(-15);
            combatSystem.OffenceValue.SetText("Failed to Deal Damage, Recieve 15 Damage");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= BarbedwireBat;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
