using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theOneWithTheGoldenGunEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
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
        combatSystem.afterCombatEvent += TheOneWithTheGoldenGun;
        combatSystem.combatComplete += RemoveEffect;
    }

    //The One With The Golden Gun Applies Bleeding to Opponent for 5 turns
    public void TheOneWithTheGoldenGun(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects addEffects = opponent.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Bleeding, 5);
        combatSystem.OffenceValue.SetText("Apply Bleeding to Defender for 5 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= TheOneWithTheGoldenGun;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
