using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overpreparedEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += Overprepared;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Overprepared causes the player to also half their roll value
    public void Overprepared(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.GetModel.RollMultiplier /= 2;
        Debug.Log("Roll Multiplier is Halved to: " + controller.GetModel.RollMultiplier);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= Overprepared;
    }
}
