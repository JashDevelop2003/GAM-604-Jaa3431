using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class betOnRedEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += ActingHubris;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Bet on Red has a 50% to either Double or Half Cash
    public void ActingHubris(object sender, EventArgs e)
    {
        int outcome = UnityEngine.Random.Range(0, 2);

        playerController controller = player.GetComponent<playerController>();
        controller.ChangeCash(controller.GetModel.CurrentCash);
        if (outcome == 0)
        {
            controller.ChangeCash(controller.GetModel.CurrentCash);
        }
        else if (outcome == 1) 
        { 
            controller.ChangeCash(-(controller.GetModel.CurrentCash / 2));
        }
        else
        {
            Debug.LogError("Something went wrong with the Int outcome");
        }
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= ActingHubris;
    }
}
