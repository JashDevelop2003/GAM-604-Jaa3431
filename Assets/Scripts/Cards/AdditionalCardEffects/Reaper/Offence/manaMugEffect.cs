using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manaMugEffect : MonoBehaviour
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
        combatSystem.duringCombatEvent += GhostlySlice;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Mana Mug steals mana from the opponent based on the amount of damage dealt
    public void GhostlySlice(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        playerController playerController = player.GetComponent<playerController>();

        opponent = combatSystem.DefendingPlayer;
        playerController opponentController = opponent.GetComponent<playerController>();

        if(combatSystem.AttackValue > combatSystem.DefendValue)
        {
            int difference = combatSystem.AttackValue - combatSystem.DefendValue;
            playerController.ChangeMana(difference);
            opponentController.ChangeMana(-difference);

            Debug.Log(difference + " Mana was Stolen:");
        }

        else
        {
            Debug.Log("No Mana were stolen");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= GhostlySlice;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
