using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missFortunateEffect : MonoBehaviour
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
        combatSystem.beforeCombatEvent += MissFortunate;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Miss Fortunate Negates the Attack & Applies Invincible to Self for 1 turn (excluding the first turn where it become pointless)
    public void MissFortunate(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        combatSystem.AttackValue = 0;
        currentBuffs buffs = player.GetComponent<currentBuffs>();
        //Requires 2 turns since the effect goes when the player's turn has ended.
        buffs.AddBuff(buffEnum.Invincible, 2, 0);
        combatSystem.DefenceValue.SetText(combatSystem.DefendValue + " + Negate Attack & Apply Invincible for 1 turn");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= MissFortunate;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
