using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wordsOfWisdomEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += WordsOfWisdom;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Words of Wisdom either Increases Thurst or Guard by 10% for this turn
    public void WordsOfWisdom(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        int RandomInt = UnityEngine.Random.Range(0, 2);
        if(RandomInt == 0)
        {
            controller.ChangeThrust(controller.GetModel.ThrustMultiplier + 0.1f);
        }
        else
        {
            controller.ChangeGuard(controller.GetModel.GuardMultiplier + 0.1f);
        }
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= WordsOfWisdom;
    }
}
