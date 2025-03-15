using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class madlyPassive : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    private GameObject player;

    private void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += MadlyPassive;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Madly Passive Applies Confused to the player for 2 turns
    public void MadlyPassive(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        currentEffects effectPlayer = player.GetComponent<currentEffects>();
        effectPlayer.AddEffect(effectEnum.Confused, 2);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= MadlyPassive;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
