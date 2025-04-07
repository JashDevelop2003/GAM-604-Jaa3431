using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redSevenEffect : MonoBehaviour
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
        state.startItemEvents += RedSeven;
    }

    // At the start of the player's turn thrust increases by 7%
    public void RedSeven(object sender, EventArgs e)
    {
        controller.ChangeThrust(controller.GetModel.ThrustMultiplier + 0.07f);
    }

    private void OnDestroy()
    {
        state.startItemEvents -= RedSeven;
    }
}
