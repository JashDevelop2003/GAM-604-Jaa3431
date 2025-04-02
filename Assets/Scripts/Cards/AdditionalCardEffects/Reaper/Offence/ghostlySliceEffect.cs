using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostlySliceEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private int pierced = 0;

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
        combatSystem.beforeCombatEvent += GhostlySlice;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Ghostly Slice Ignores the Defend Value Entirely
    public void GhostlySlice(object sender, EventArgs e)
    {
        combatSystem.DefendValue = pierced;
        combatSystem.OffenceValue.SetText("Pierced Opponent, Defend Value at: " + pierced);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= GhostlySlice;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
