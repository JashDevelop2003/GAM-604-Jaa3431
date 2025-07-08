using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pentagramNecklessEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform deckTransform;
    private playerController controller;
    private startState state;
    private int omenCount;
    [SerializeField] private List<GameObject> allItems = new List<GameObject>();

    // Upon pickup does nothing
    void Awake()
    {

        playerTransform = this.transform.parent.parent.parent;
        deckTransform = this.transform.parent.parent.transform;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();
        state.startItemEvents += PentagramNeckless;
    }

    // At the start of the player's turn thrust increases by 10% per omen obained
    public void PentagramNeckless(object sender, EventArgs e)
    { 
        allItems = new List<GameObject>();

        foreach (Transform child in deckTransform)
        {
             allItems.Add(child.gameObject);
        }

        for (int i = 0; i < allItems.Count; i++)
        {
            itemBehaviour checkType = allItems[i].GetComponent<itemBehaviour>();
            if (checkType != null)
            {
                if (checkType.Item.itemType == itemEnum.Omen)
                {
                    omenCount++;
                }
            }
        }

        if (omenCount > 0) 
        {
            controller.ChangeThrust(controller.GetModel.ThrustMultiplier + (omenCount * 0.1f));
            omenCount = 0;
        }

        allItems = null;
    }

    private void OnDestroy()
    {
        state.startItemEvents -= PentagramNeckless;
    }
}
