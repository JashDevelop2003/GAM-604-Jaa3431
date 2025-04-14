using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeItOff : MonoBehaviour
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
        rollState.rollEvent += ShakeItOff;
        rollState.rollEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }

    //Shake It Off Applies Self Healthy for 1 turn (and this turn)
    public void ShakeItOff(object sender, EventArgs e)
    {
        currentBuffs buffs = player.GetComponent<currentBuffs>();
        buffs.AddBuff(buffEnum.Healthy, 2, 0); //Has to be 2 due to the event invoking at the end
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        rollState.rollEvent -= ShakeItOff;
        rollState.rollEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
