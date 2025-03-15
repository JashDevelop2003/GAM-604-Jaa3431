using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyArmourEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += BodyArmour;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Body Armour Increase Resistant by 15% for 3 Turns
    public void BodyArmour(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        currentBuffs buffPlayer = player.GetComponent<currentBuffs>();
        buffPlayer.AddBuff(buffEnum.Resistant, 3, 0.15f);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= BodyArmour;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
