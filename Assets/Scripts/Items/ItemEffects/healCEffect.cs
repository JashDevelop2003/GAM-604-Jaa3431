using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healCEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private pickingState state;

    // Upon pickup does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<pickingState>();
        state.pickingItemEvents += HealC;
    }

    // When landing on a card space will heal 5 health to the player
    public void HealC(object sender, EventArgs e)
    {
        controller.ChangeHealth(5);
    }

    private void OnDestroy()
    {
        state.pickingItemEvents -= HealC;
    }
}
