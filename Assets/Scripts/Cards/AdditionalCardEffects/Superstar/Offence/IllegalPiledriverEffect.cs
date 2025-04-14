using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllegalPiledriverEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += IllegalPiledriver;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Illegal Piledriver Applies Unstable to Self for 2 turns
    public void IllegalPiledriver(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        currentEffects addEffects = player.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Unstabled, 2);
        combatSystem.OffenceValue.SetText("Apply Unstable to Self for 2 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= IllegalPiledriver;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
