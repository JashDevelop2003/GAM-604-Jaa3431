using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class costAArmAndALegEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += CostAArmAndALeg;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Cost a Arm & A Leg Deals 10 Damage to Self
    // However the player gains 50 cash
    public void CostAArmAndALeg(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeCash(50);
        controller.ChangeHealth(-10);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= CostAArmAndALeg;
    }
}
