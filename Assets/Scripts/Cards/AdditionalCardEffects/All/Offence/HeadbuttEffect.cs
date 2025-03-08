using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbuttEffect : MonoBehaviour
{
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;
    private GameObject opponent;


    void Awake()
    {
        offenceCard = GetComponentInParent<offenceCard>();
        combatSystem = combatSystem.instance;
        offenceCard.additionalEvent += AddEffect;
    }

    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += Headbutt;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void Headbutt(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        opponent = combatSystem.DefendingPlayer;

        currentEffects playerEffect = player.GetComponent<currentEffects>();
        currentEffects opponentEffect = opponent.GetComponent<currentEffects>();

        playerEffect.AddEffect(effectEnum.Confused, 2);
        opponentEffect.AddEffect(effectEnum.Confused, 1);

        Debug.Log("Both Players are Confused");
    }

    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= Headbutt;
    }
}
