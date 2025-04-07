using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickOrTreatBag : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private currentBuffs buffs;
    private startState startState;
    private pickingState pickingState;

    // Upon pickup does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        buffs = player.GetComponent<currentBuffs>();
        startState = player.GetComponent<startState>();
        pickingState = player.GetComponent<pickingState>();
        startState.startItemEvents += StartTrickOrTreating;
        pickingState.pickingItemEvents += Tricked;
    }

    // When the player starts their turn they gain lucky
    public void StartTrickOrTreating(object sender, EventArgs e)
    {
        buffs.AddBuff(buffEnum.Lucky, 1, 0);
    }

    //When the player lands on a card space there's a 50% chance for the player to not obtain a card
    public void Tricked(object sender, EventArgs e)
    {
        int tricked = UnityEngine.Random.Range(0, 2);
        if (tricked == 0) 
        {
            StartCoroutine(pickingState.CardObtained());
            pickingState.EventText.SetText("You were Tricked! No Treat for you!  >:)");
        }
    }

    private void OnDestroy()
    {
        startState.startItemEvents -= StartTrickOrTreating;
        pickingState.pickingItemEvents -= Tricked;
    }
}
