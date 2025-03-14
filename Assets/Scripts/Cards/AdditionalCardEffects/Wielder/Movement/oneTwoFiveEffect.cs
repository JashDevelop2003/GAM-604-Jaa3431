using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneTwoFiveEffect : MonoBehaviour
{
    private movementCard movementCard;
    private Transform locatePlayer;
    private GameObject player;
    private rollState rollState;

    void Awake()
    {
        movementCard = GetComponentInParent<movementCard>();
        movementCard.additionalEvent += AddEffect;
        //The transform is used to locate the player since the roll state doesn't mention the playrer's game object unlike the combat system
        locatePlayer = this.transform.parent.parent.parent;
        player = locatePlayer.gameObject;
        rollState = player.GetComponent<rollState>();
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        rollState.rollEvent += OneTwoFive;
        rollState.rollEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }

    //One Two Five Will Change the value of 3 to 5
    public void OneTwoFive(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        if(controller.GetModel.RollValue == 3)
        {
            controller.GetModel.RollValue = 5;
            Debug.Log("1... 2... 5!");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        rollState.rollEvent -= OneTwoFive;
        rollState.rollEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
