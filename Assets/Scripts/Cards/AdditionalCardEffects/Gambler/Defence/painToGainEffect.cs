using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class painToGainEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += PainToGain;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Pain to Gain Increases Cash = Attack Value (Damage Taken since Defend Value should be 0)
    public void PainToGain(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeCash(combatSystem.AttackValue);
        combatSystem.DefenceValue.SetText("Defender gains: " + combatSystem.AttackValue + " cash");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= PainToGain;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
