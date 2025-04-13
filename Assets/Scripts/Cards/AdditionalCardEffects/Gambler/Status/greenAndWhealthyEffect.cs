using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenAndWhealthyEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += GreenAndWhealthy;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Green & Whealthy Increases Health by 10% of Current Cash currently the player has
    public void GreenAndWhealthy(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        Debug.Log("Heals: " + (int)(controller.GetModel.CurrentCash * 0.1f));
        controller.ChangeHealth((int)(controller.GetModel.CurrentCash * 0.1f));
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= GreenAndWhealthy;
    }
}
