using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldSevenEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform deckTransform;
    private playerController controller;
    private startState state;
    [SerializeField] private bool[] hasSeven = new bool[3];
    [SerializeField] private bool hasAllSevens = false;
    [SerializeField] private List <GameObject> allItems = new List<GameObject>();

    // Upon pickup Blue 7 does nothing
    void Awake()
    {
        for (int i = 0; i < hasSeven.Length; i++) 
        { 
            hasSeven[i] = false;
        }

        playerTransform = this.transform.parent.parent.parent;
        deckTransform = this.transform.parent.parent.transform;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();
        state.startItemEvents += GoldSeven;
    }

    // At the start of the player's turn guard and thrust increases by 21% if they have strange 7, red 7 & blue 7 in their inventory
    public void GoldSeven(object sender, EventArgs e)
    {
        if (!hasAllSevens) 
        {
            allItems = new List<GameObject>();

            foreach(Transform child in deckTransform)
            {
                allItems.Add(child.gameObject);
            }

            for (int i = 0; i < allItems.Count; i++) 
            {

                if (allItems[i].name == "Red 7")
                {
                    hasSeven[0] = true;
                }
                else if (allItems[i].name == "Blue 7")
                {
                    hasSeven[1] = true;
                }
                else if (allItems[i].name == "Strange 7")
                {
                    hasSeven[2] = true;
                }
            }

            if(hasSeven[0] && hasSeven[1] && hasSeven[2])
            {
                hasAllSevens = true;
            }

            allItems = null;
        }

        if (hasAllSevens) 
        {
            controller.ChangeGuard(controller.GetModel.GuardMultiplier + 0.21f);
            controller.ChangeThrust(controller.GetModel.ThrustMultiplier + 0.21f);
        }
    }

    private void OnDestroy()
    {
        state.startItemEvents -= GoldSeven;
    }
}
