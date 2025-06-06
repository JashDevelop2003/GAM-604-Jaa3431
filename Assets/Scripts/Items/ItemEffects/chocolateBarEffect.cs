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
    private List<buffEnum> possibleBuffs = new List<buffEnum>()
    {
        buffEnum.Invincible,
        buffEnum.Healthy,
        buffEnum.Hasty,
        buffEnum.Lucky,
    };

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
        //This provides a random range between 0 to the list count of possible buffs
        //This will then add the buff depending on the random int
        int randomBuff = UnityEngine.Random.Range(0, possibleBuffs.Count);
        buffs.AddBuff(possibleBuffs[randomBuff], 10, 0);

        item.pickupEvent -= UponPickup;
    }
}
