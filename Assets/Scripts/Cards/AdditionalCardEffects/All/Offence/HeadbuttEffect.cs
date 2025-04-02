using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is the additional effect of headbutt
/// The effect applies confuse to both players for 1 full turn after combat has ended
/// </summary>

public class HeadbuttEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;
    private GameObject opponent;

    ///This should be used for all additional effects
    void Awake()
    {
        offenceCard = GetComponentInParent<offenceCard>();
        combatSystem = combatSystem.instance;
        offenceCard.additionalEvent += AddEffect;
    }

    //Once the player chosen the card, the player then invokes the effect
    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += Headbutt;
        combatSystem.combatComplete += RemoveEffect;
    }

    //This then applies the confusion to both players for 1 full turn
    //the reason for the player to have 2 turns is becuase the effect decrements at the end
    //This means that the player technically has 1 & a half of the effect
    public void Headbutt(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        opponent = combatSystem.DefendingPlayer;

        currentEffects playerEffect = player.GetComponent<currentEffects>();
        currentEffects opponentEffect = opponent.GetComponent<currentEffects>();

        playerEffect.AddEffect(effectEnum.Confused, 2);
        opponentEffect.AddEffect(effectEnum.Confused, 1);

        combatSystem.OffenceValue.SetText("Both Players are Confused");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= Headbutt;
    }
}
