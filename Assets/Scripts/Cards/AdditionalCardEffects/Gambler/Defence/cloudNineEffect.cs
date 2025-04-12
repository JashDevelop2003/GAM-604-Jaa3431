using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudNineEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += CloudNine;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Cloud Nine Applies Hasty to Self for 2 turns
    public void CloudNine(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        currentBuffs buffs = player.GetComponent<currentBuffs>();
        buffs.AddBuff(buffEnum.Hasty, 2, 0);
        combatSystem.DefenceValue.SetText("Apply Hasty for 2 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= CloudNine;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
