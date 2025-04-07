using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wantedPoster : MonoBehaviour
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
        combatSystem.beforeCombatEvent += WantedPoster;
        combatSystem.combatComplete += RemoveEffect;
    }
    
    // When Defending in Combat:
    // - 80% Chance on Defend Value Decreasing by 50%
    // - 20 Chance on Attack Value Decreasing by 30%
    public void WantedPoster(object sender, EventArgs e)
    {
        int wantedChance = UnityEngine.Random.Range(0, 5);
        if (wantedChance == 0)
        {
            Debug.Log("Old Value: " + combatSystem.AttackValue);
            combatSystem.AttackValue = (int)(combatSystem.AttackValue * 0.3f);
        }
        else
        {
            Debug.Log("Old Value: " + combatSystem.DefendValue);
            combatSystem.DefendValue /= 2;
        }
    }

    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.beforeCombatEvent -= WantedPoster;
        combatSystem.combatComplete -= RemoveEffect;
    }

    private void OnDestroy()
    {
        state.defendItemEvents -= ApplyEffect;
    }
}
