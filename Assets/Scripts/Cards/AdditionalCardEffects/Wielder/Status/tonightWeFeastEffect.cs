using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tonightWeFeastEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += TonightWeFeast;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Tonight We Feast Increases the player's health by 20 HP
    public void TonightWeFeast(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeHealth(20);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= TonightWeFeast;
    }
}
