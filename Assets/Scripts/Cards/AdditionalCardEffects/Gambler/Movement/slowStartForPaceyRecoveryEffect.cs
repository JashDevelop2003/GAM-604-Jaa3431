using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowStartForPaceyRecoveryEffect : MonoBehaviour
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
        rollState.rollEvent += SlowStartForPaceyRecovery;
        rollState.rollEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }

    //Slow Start for Pacey Recovery Applies Self Hasty for 2 turns
    public void SlowStartForPaceyRecovery(object sender, EventArgs e)
    {
        currentBuffs buffs = player.GetComponent<currentBuffs>();
        buffs.AddBuff(buffEnum.Hasty, 2, 0);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        rollState.rollEvent -= SlowStartForPaceyRecovery;
        rollState.rollEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
