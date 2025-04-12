using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fourBulletCloverEffect : MonoBehaviour
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
        combatSystem.afterCombatEvent += FourBulletClover;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Four Bullet Clover Applies Bleeding to Opponent for 4 turns
    public void FourBulletClover(object sender, EventArgs e)
    {
        opponent = combatSystem.DefendingPlayer;
        currentEffects addEffects = opponent.GetComponent<currentEffects>();
        addEffects.AddEffect(effectEnum.Bleeding, 4);
        combatSystem.OffenceValue.SetText("Apply Bleeding to Defender for 4 turns");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= FourBulletClover;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
