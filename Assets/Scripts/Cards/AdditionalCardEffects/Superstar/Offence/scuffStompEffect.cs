using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scuffStompEffect : MonoBehaviour
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
        combatSystem.beforeCombatEvent += ScuffStomp;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Scuff Stomp has a 50% chance to set Attack Value to 0 & Deal 3 Damage to Self
    public void ScuffStomp(object sender, EventArgs e)
    {
        int outcome = UnityEngine.Random.Range(0, 2);
        if (outcome == 0)
        {
            player = combatSystem.AttackingPlayer;
            playerController controller = player.GetComponent<playerController>();
            controller.ChangeHealth(-3);
            combatSystem.OffenceValue.SetText("Attack Failed: Set Attack to 0 & Recieve 3 Damage");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= ScuffStomp;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
