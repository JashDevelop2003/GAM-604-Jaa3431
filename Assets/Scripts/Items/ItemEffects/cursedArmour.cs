using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursedArmour : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private startState state;

    // Upon pickup Cursed Armour does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();
        state.startItemEvents += CursedArmour;
    }

    // At the start of the player's turn guard increases by 10%
    // However roll decreases by 20%
    public void CursedArmour(object sender, EventArgs e)
    {
        controller.ChangeGuard(controller.GetModel.GuardMultiplier + 0.1f);
        controller.ChangeRoll(controller.GetModel.RollMultiplier - 0.2f);
    }

    private void OnDestroy()
    {
        state.startItemEvents -= CursedArmour;
    }
}
