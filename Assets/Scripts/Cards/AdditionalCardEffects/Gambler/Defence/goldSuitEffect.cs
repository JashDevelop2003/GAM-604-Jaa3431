using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldSuitEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += GoldSuit;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Gold Suit Gains 20 Cash to Player
    public void GoldSuit(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeCash(20);
        combatSystem.DefenceValue.SetText("Gain 20 Coins");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= GoldSuit;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
