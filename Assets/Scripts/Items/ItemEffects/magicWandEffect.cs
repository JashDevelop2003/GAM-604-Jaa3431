using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicWandEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private itemBehaviour item;
    private playerController controller;
    int manaValue = 7;

    // Magic Wand will increase their max mana by 7 only being picked up
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        item = GetComponentInParent<itemBehaviour>();
        item.pickupEvent += UponPickup;

    }

    public void UponPickup(object sender, EventArgs e)
    {
        controller.GetModel.MaxMana += manaValue;
        //Double Negative = Positive
        controller.ChangeMana(-manaValue);
    }

    private void OnDestroy()
    {
        item.pickupEvent -= UponPickup;
    }
}
