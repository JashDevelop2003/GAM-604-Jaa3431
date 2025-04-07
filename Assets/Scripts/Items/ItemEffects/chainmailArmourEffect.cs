using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainmailArmourEffect : MonoBehaviour
{

        [SerializeField] private Transform playerTransform;
        [SerializeField] private GameObject player;
        private itemBehaviour item;
        private playerController controller;
        private currentEffects effects;
        private startState state;

        // Upon pickup Chainmail Armour Applies Slow to Self for 2 Turns
        void Awake()
        {
            playerTransform = this.transform.parent.parent.parent;
            player = playerTransform.gameObject;
            controller = player.GetComponent<playerController>();
            state = player.GetComponent<startState>();
            effects = player.GetComponent<currentEffects>();
            item = GetComponentInParent<itemBehaviour>();
            item.pickupEvent += UponPickup;
            state.startItemEvents += ChainmailArmour;
            
        }

        public void UponPickup(object sender, EventArgs e)
        {
            effects.AddEffect(effectEnum.Slowed, 2);
            item.pickupEvent -= UponPickup;
        }

        // At the start of the player's turn guard increases by 10%
        public void ChainmailArmour(object sender, EventArgs e)
        {
            controller.ChangeGuard(controller.GetModel.GuardMultiplier + 0.1f);
        }

        private void OnDestroy()
        {
            state.startItemEvents -= ChainmailArmour;
        }
}
