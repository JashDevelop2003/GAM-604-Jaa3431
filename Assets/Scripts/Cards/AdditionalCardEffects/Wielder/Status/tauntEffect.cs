using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tauntEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += Taunt;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Taunt Decreases Guard Multiplier by 40% for this turn
    public void Taunt(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeGuard(controller.GetModel.GuardMultiplier - 0.4f);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= Taunt;
    }
}
