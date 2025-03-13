using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponThrowEffect : MonoBehaviour
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

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += WeaponThrow;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void WeaponThrow(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        opponent = combatSystem.DefendingPlayer;
        passiveAgression playerStance = player.GetComponentInChildren<passiveAgression>();
        if (playerStance.Stance == stanceEnum.Aggressive)
        {
            currentEffects opponentEffect = opponent.GetComponent<currentEffects>();
            opponentEffect.AddEffect(effectEnum.Bleeding, 5);
            Debug.Log(opponent + "is bleeding for 5 turns");
        }
        else
        {
            Debug.Log("No Bleeding due to being Passive");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= WeaponThrow;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
