using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargeEffect : MonoBehaviour
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
        rollState.rollEvent += Charge;
        rollState.rollEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }

    //One Two Five Will Change the value of 3 to 5
    public void Charge(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        Debug.Log("Previous Multiplier" + controller.GetModel.ThrustMultiplier);
        controller.GetModel.ThrustMultiplier *= 2;
        Debug.Log("New Multiplier" + controller.GetModel.ThrustMultiplier);

    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        rollState.rollEvent -= Charge;
        rollState.rollEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
