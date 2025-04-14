using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class losingThePlotEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += LosingthePlot;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Losing the Plot Increases Thrust by 15%
    public void LosingthePlot(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeThrust(controller.GetModel.ThrustMultiplier + 0.15f);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= LosingthePlot;
    }
}
