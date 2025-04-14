using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Diry Deeds is the Superstar's Passive Ability
/// The Ability Allows the player as the defender to deal damage to the attacker if they recieved +20 Damage
/// </summary>

public class dirtyDeeds : MonoBehaviour
{
    private combatSystem combatSystem;
    private playerController playerController;
    private playerController oppponentController;

    void Awake()
    {
        combatSystem = combatSystem.instance;
        playerController = GetComponentInParent<playerController>();
        playerController.DisplayAbility(playerController.GetData.abilityIcon[0], playerController.GetData.abilityColour[0]);

        combatSystem.duringCombatEvent += DirtyDeeds;
    }


    public void DirtyDeeds(object sender, EventArgs e)
    {
        if((combatSystem.AttackValue - combatSystem.DefendValue) >= 20)
        {
            oppponentController = combatSystem.AttackingPlayer.GetComponent<playerController>();
            oppponentController.ChangeHealth(-(combatSystem.AttackValue - combatSystem.DefendValue));
            Debug.Log("Dirty Deeds deals to opponent: " + (combatSystem.AttackValue - combatSystem.DefendValue));
        }
    }

    private void OnDisable()
    {
        combatSystem.duringCombatEvent -= DirtyDeeds;
    }
}
