using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class chocolateBarEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private itemBehaviour item;
    private currentBuffs buffs;

    // Chocolate Bar Applies a random buff (except Resistant & Impactful for 10 turns)
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        buffs = player.GetComponent<currentBuffs>();
        item = GetComponentInParent<itemBehaviour>();
        item.pickupEvent += UponPickup;
    }

    public void UponPickup(object sender, EventArgs e)
    {
        int randomBuff = UnityEngine.Random.Range(0, (int)buffEnum.Null);
        while (randomBuff == (int)buffEnum.Impactful || randomBuff == (int)buffEnum.Resistant) 
        { 
            randomBuff = UnityEngine.Random.Range(0, (int)buffEnum.Null);
        }
        
        if(randomBuff == (int)buffEnum.Invincible)
        {
            buffs.AddBuff(buffEnum.Invincible, 10, 0);
        }

        else if (randomBuff == (int)buffEnum.Healthy)
        {
            buffs.AddBuff(buffEnum.Healthy, 10, 0);
        }

        else if (randomBuff == (int)buffEnum.Hasty)
        {
            buffs.AddBuff(buffEnum.Hasty, 10, 0);
        }

        else if (randomBuff == (int)buffEnum.Lucky)
        {
            buffs.AddBuff(buffEnum.Lucky, 10, 0);
        }

        item.pickupEvent -= UponPickup;
    }
}
