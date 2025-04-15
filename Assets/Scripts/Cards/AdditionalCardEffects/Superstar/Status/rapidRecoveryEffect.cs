using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rapidRecoveryEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += RapidRecovery;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Rapid Recovery Heals 10 Health
    public void RapidRecovery(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeHealth(10);
        Debug.Log("Heal 10 Health");
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= RapidRecovery;
    }
}
