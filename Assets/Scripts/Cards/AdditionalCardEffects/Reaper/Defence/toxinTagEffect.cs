using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toxinTagEffect : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    private GameObject opponent;
    private GameObject player;

    ///This should be used for all additional effects
    void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += ToxinTag;
        combatSystem.combatComplete += RemoveEffect;
    }

    // Toxin Tag Applies Poison to both Opponent & player for 2 Turns
    public void ToxinTag(object sender, EventArgs e)
    {
        opponent = combatSystem.AttackingPlayer;
        player = combatSystem.DefendingPlayer;
        currentEffects opponentEffects = opponent.GetComponent<currentEffects>();
        currentEffects playerEffects = player.GetComponent<currentEffects>();
        opponentEffects.AddEffect(effectEnum.Poison, 2);
        playerEffects.AddEffect(effectEnum.Poison, 2);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= ToxinTag;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
