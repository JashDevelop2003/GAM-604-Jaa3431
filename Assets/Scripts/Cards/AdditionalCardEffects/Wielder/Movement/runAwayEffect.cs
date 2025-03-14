using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runAwayEffect : MonoBehaviour
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
        rollState.rollEvent += RunAway;
        rollState.rollEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }

    //Bloody Dice Applies Bleeding to the player equal to the roll value
    public void RunAway(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeCash(-controller.GetModel.RollValue);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        rollState.rollEvent -= RunAway;
        rollState.rollEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
