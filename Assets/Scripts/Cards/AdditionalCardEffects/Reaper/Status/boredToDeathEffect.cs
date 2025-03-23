using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boredToDeathEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += BoredToDeath;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Bored to Death Applies Healthy for 1 turn
    // However Thurst & Roll Multiplier Decreases for this turn by 25%
    public void BoredToDeath(object sender, EventArgs e)
    {
        currentBuffs playerBuff = player.GetComponent<currentBuffs>();
        playerBuff.AddBuff(buffEnum.Healthy, 1, 0);

        playerController controller = player.GetComponent<playerController>();
        controller.ChangeThrust(controller.GetModel.ThrustMultiplier - 0.25f);
        controller.ChangeRoll(controller.GetModel.RollMultiplier - 0.25f);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= BoredToDeath;
    }
}
