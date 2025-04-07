using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strangeSevenEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private startState state;

    // Upon pickup Strange 7 does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();
        state.startItemEvents += StrangeSeven;
    }

    // At the start of the player's turn both thrust and guard increase by 1%
    public void StrangeSeven(object sender, EventArgs e)
    {
        controller.ChangeThrust(controller.GetModel.ThrustMultiplier + 0.01f);
        controller.ChangeGuard(controller.GetModel.GuardMultiplier + 0.01f);
    }

    private void OnDestroy()
    {
        state.startItemEvents -= StrangeSeven;
    }
}
