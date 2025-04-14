using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landOnYourFeetEffect : MonoBehaviour
{
    //This requires the defence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;

    private void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent += LandOnYourFeet;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Land On Your Feet Blocks between 0 to 10
    public void LandOnYourFeet(object sender, EventArgs e)
    {
        int outcome = UnityEngine.Random.Range(0, 11);
        combatSystem.DefendValue = outcome;
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= LandOnYourFeet;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
