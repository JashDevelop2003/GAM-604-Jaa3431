using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cherriesEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private playerController controller;
    [SerializeField] private bool hasPickup;

    // Cherries Increases Max Health by 3 & Gains 3 Health
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();

        //This ensures that when loading the game again the effect of the relic doesn't play again
        if (!hasPickup)
        {
            controller.GetModel.MaxHealth += 3;
            controller.ChangeHealth(3);
            hasPickup = true;
            Debug.Log("Increase Max Health: " +  controller.GetModel.MaxHealth);
        }
    }

}
