using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mummifiedMaskEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private currentEffects effects;
    private startState state;

    // Upon pickup does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        effects = player.GetComponent<currentEffects>();
        state = player.GetComponent<startState>();
        state.startItemEvents += MummifiedMask;
    }

    // At the start of the player's turn thrusst increases by 5%
    // However has a 50% Chance for the player to go Confused for this turn
    public void MummifiedMask(object sender, EventArgs e)
    {
        controller.ChangeThrust(controller.GetModel.ThrustMultiplier + 0.05f);
        int blindChance = UnityEngine.Random.Range(0, 2);
        if (blindChance == 1)
        {
            effects.AddEffect(effectEnum.Confused, 1);
        }
    }

    private void OnDestroy()
    {
        state.startItemEvents -= MummifiedMask;
    }
}
