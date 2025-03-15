using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timelessTakeEffect : MonoBehaviour
{
    private movementCard movementCard;
    private Transform locatePlayer;
    private GameObject player;
    private rollState rollState;
    private moveState moveState;

    void Awake()
    {
        movementCard = GetComponentInParent<movementCard>();
        movementCard.additionalEvent += AddEffect;
        //The transform is used to locate the player since the roll state doesn't mention the playrer's game object unlike the combat system
        locatePlayer = this.transform.parent.parent.parent;
        player = locatePlayer.gameObject;
        rollState = player.GetComponent<rollState>();
        moveState = player.GetComponent<moveState>();
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        moveState.endTurnEvent += TimelessTake;
        moveState.endTurnEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }

    //Timeless Take stuns the player for 1 turn after completing their movement
    public void TimelessTake(object sender, EventArgs e)
    {
        currentEffects playerEffect = player.GetComponent<currentEffects>();
        //Needs to be 2 since the stun event is at the end and decrements the cooldown
        playerEffect.AddEffect(effectEnum.Stunned, 2);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        moveState.endTurnEvent -= TimelessTake;
        moveState.endTurnEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
