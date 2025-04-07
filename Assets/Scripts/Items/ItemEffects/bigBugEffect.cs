using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigBugEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private pickingState state;

    // Upon pickup does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        state = player.GetComponent<pickingState>();
        state.pickingItemEvents += BigBug;
    }

    // When landing on a card space will heal 5 health to the player
    public void BigBug(object sender, EventArgs e)
    {
        if (state.Rarity == CardRarity.Rare) 
        {
            StartCoroutine(state.CardObtained());
            state.EventText.SetText("The bug just ate your opportunity to obtain a rare card. Unlucky!");
        }
    }

    private void OnDestroy()
    {
        state.pickingItemEvents -= BigBug;
    }
}
