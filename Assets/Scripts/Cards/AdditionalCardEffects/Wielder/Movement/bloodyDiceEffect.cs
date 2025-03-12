using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodyDiceEffect : MonoBehaviour
{
    private movementCard movementCard;
    [SerializeField] private Transform locatePlayer;
    [SerializeField] private GameObject player;
    private rollState rollState;
    
    void Awake()
    {
        movementCard = GetComponentInParent<movementCard>();
        movementCard.additionalEvent += AddEffect;
        //The transform is used to locate the player since the roll state doesn't mention the playrer's game object unlike the combat system
        locatePlayer = this.transform.parent.parent.parent;
        player = locatePlayer.gameObject;
        rollState = player.GetComponent<rollState>();
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        rollState.rollEvent += BloodyDice;
        rollState.rollEvent += RemoveEffect;
        rollState.rollCancelEvent += RemoveEffect;
    }
    public void BloodyDice(object sender, EventArgs e)
    {
        playerController controller = player.GetComponent<playerController>();
        currentEffects currentEffects = player.GetComponent<currentEffects>();
        currentEffects.AddEffect(effectEnum.Bleeding, controller.GetModel.RollValue);
        Debug.Log("Player bled by " + controller.GetModel.RollValue);
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        rollState.rollEvent -= BloodyDice;
        rollState.rollEvent -= RemoveEffect;
        rollState.rollCancelEvent -= RemoveEffect;
    }
}
