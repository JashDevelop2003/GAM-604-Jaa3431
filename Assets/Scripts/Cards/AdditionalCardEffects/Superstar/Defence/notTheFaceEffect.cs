using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notTheFaceEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += NotTheFace;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Not The Face! Applies Resistant by 5% for 2 turns
    public void NotTheFace(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        currentBuffs buffs = player.GetComponent<currentBuffs>();
        buffs.AddBuff(buffEnum.Resistant, 2, 0.05f);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= NotTheFace;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
