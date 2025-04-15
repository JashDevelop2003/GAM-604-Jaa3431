using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suckItEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += SuckIt;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Suck It Increases Thrust by 30% this turn
    public void SuckIt(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeThrust(controller.GetModel.ThrustMultiplier + 0.3f);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= SuckIt;
    }
}
