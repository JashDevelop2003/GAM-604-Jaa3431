using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bringTheTableEffect : MonoBehaviour
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
        combatSystem.duringCombatEvent += BringTheTable;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Bring The Table Deals 7 Damage to self
    public void BringTheTable(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeHealth(-7);
        combatSystem.OffenceValue.SetText("Attack Recieves 7 Damage");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= BringTheTable;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
