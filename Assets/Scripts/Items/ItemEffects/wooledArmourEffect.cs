using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wooledArmourEffect : MonoBehaviour
{
        [SerializeField] private Transform playerTransform;
        [SerializeField] private GameObject player;
        private playerController controller;
        private startState state;

        // Upon pickup Wooled Armour does nothing
        void Awake()
        {
            playerTransform = this.transform.parent.parent.parent;
            player = playerTransform.gameObject;
            controller = player.GetComponent<playerController>();
            state = player.GetComponent<startState>();
            state.startItemEvents += WooledArmour;
        }

        // At the start of the player's turn guard increases by 5%
        public void WooledArmour(object sender, EventArgs e)
        {
            controller.ChangeGuard(controller.GetModel.GuardMultiplier + 0.05f);
        }

        private void OnDestroy()
        {
            state.startItemEvents -= WooledArmour;
        }
}
