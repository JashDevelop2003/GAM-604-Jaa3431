using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doYouYieldEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += DoYouYield;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Do You Yield Applies Healthy to Self whilst everyone else gains negative effects
    public void DoYouYield(object sender, EventArgs e)
    {
        currentBuffs currentBuffs = player.GetComponent<currentBuffs>();
        currentBuffs.AddBuff(buffEnum.Healthy, 1, 0);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= DoYouYield;
    }
}
