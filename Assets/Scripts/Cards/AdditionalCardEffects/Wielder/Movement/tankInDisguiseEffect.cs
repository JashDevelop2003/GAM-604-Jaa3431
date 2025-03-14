using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankInDisguiseEffect : MonoBehaviour
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
        rollState.rollEvent += TankInDisguise;
        rollState.rollEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }

    //Tank In Disguise Confuses the player for this turn & next turn
    public void TankInDisguise(object sender, EventArgs e)
    {
        currentEffects currentEffects = player.GetComponent<currentEffects>();
        currentEffects.AddEffect(effectEnum.Confused, 2);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        rollState.rollEvent -= TankInDisguise;
        rollState.rollEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
