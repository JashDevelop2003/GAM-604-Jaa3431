using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldenDiceEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    private moveState state;

    // Upon pickup does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<moveState>();
        state.beginItemMoveEvent += GoldenDice;
    }

    // When the player has rolled the dice they will incrase cash equal to the roll value
    public void GoldenDice(object sender, EventArgs e)
    {
        controller.ChangeCash(controller.GetModel.RollValue);
    }

    private void OnDestroy()
    {
        state.beginItemMoveEvent -= GoldenDice;
    }
}
