using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueSevenEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private startState state;

    // Upon pickup Blue 7 does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();
        state.startItemEvents += BlueSeven;
    }

    // At the start of the player's turn guard increases by 7%
    public void BlueSeven(object sender, EventArgs e)
    {
        controller.ChangeGuard(controller.GetModel.GuardMultiplier + 0.07f);
    }

    private void OnDestroy()
    {
        state.startItemEvents -= BlueSeven;
    }

}
