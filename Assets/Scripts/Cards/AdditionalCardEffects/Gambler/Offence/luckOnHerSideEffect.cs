using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class luckOnHerSideEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += LuckOnHerSide;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Luck on Her Side Heals 7 Health to Player
    public void LuckOnHerSide(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeHealth(7);
        combatSystem.OffenceValue.SetText("Player heals 7 Health");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= LuckOnHerSide;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
