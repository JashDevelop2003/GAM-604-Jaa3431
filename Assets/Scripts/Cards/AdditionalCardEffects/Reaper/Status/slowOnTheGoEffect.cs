using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowOnTheGoEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += SlowOnTheGo;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Slow on the Go Decreases Guard Multiplier by 10% for this turn
    public void SlowOnTheGo(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.GetModel.GuardMultiplier += 0.1f;
        Debug.Log("Guard Multiplier has went down by 0.1f, new multiplier is: " + controller.GetModel.GuardMultiplier);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= SlowOnTheGo;
    }
}
