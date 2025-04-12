using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badKarmaSomehowPaysOffEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;
    [SerializeField] private Transform deckTransform;
    [SerializeField] private List<GameObject> allItems = new List<GameObject>();
    private int amountofOmens;


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += ActingHubris;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        deckTransform = this.transform.parent.parent.transform;
        player = playerTransform.gameObject;
    }


    // Acting Hubris Doubles Cash
    public void ActingHubris(object sender, EventArgs e)
    {
        amountofOmens = 0;

        foreach (Transform child in deckTransform)
        {
            allItems.Add(child.gameObject);
        }

        for (int i = 0; i < allItems.Count; i++)
        {
            itemBehaviour item = allItems[i].GetComponent<itemBehaviour>();
            if (item != null) 
            { 
                if(item.Item.itemType == itemEnum.Omen)
                {
                    amountofOmens++;
                }
            }
            else
            {
                Debug.LogError("Error occurred when collecting the items");
            }
        }

        playerController controller = player.GetComponent<playerController>();
        controller.ChangeCash(amountofOmens * 10);
        allItems = null;
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= ActingHubris;
    }
}
