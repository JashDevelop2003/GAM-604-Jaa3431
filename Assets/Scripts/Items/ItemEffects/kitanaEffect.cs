using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kitanaEffect : MonoBehaviour
{
    private combatSystem combatSystem;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private currentEffects effects;
    private defendState state;

    // Upon pickup does nothing
    void Awake()
    {
        combatSystem = combatSystem.instance;
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        effects = player.GetComponent<currentEffects>();
        state = player.GetComponent<defendState>();
        state.defendItemEvents += ApplyEffect;
    }

    public void ApplyEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent += Kitana;
        combatSystem.combatComplete += RemoveEffect;
    }

    // When Defending in Combat:
    // - 80% Chance on Defend Value Decreasing by 50%
    // - 20 Chance on Attack Value Decreasing by 30%
    public void Kitana(object sender, EventArgs e)
    {
        if(combatSystem.DefendValue > combatSystem.AttackValue)
        {
            playerController opponentController = combatSystem.AttackingPlayer.GetComponent<playerController>();
            opponentController.ChangeHealth(-10);
            Debug.Log("Slice & Dice");
        }
    }

    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= Kitana;
        combatSystem.combatComplete -= RemoveEffect;
    }

    private void OnDestroy()
    {
        state.defendItemEvents -= ApplyEffect;
    }
}
