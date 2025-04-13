using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badKarmaSomehowPaysOffEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;
    itemDeckPool itemDeck;
    private int amountofOmens;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += ActingHubris;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        itemDeck = player.GetComponentInChildren<itemDeckPool>();
    }


    // Acting Hubris Doubles Cash
    public void ActingHubris(object sender, EventArgs e)
    {
        amountofOmens = 0;


        for (int i = 0; i < itemDeck.Items.Count; i++)
        {
            itemBehaviour item = itemDeck.Items[i].GetComponent<itemBehaviour>();
            if (item != null) 
            { 
                if(item.Item.itemType == itemEnum.Omen)
                {
                    amountofOmens++;
                }
            }
        }

        playerController controller = player.GetComponent<playerController>();
        controller.ChangeCash(amountofOmens * 10);
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= ActingHubris;
    }
}
