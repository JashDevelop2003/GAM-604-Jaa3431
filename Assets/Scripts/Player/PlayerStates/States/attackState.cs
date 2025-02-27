using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm
{
    [SerializeField] private GameObject combatManager;
    [SerializeField] private combatSystem combatSystem;
    
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    playerController controller;
    
    [SerializeField] private offenceDeckPile offenceDeck;
    public offenceDeckPile OffenceDeck
    {
        get { return offenceDeck; }
    }

    [SerializeField] private GameObject selectedCard;

    private bool attackConfirm;
    private bool combatFinished;

    public override void EnterState(playerStateManager player)
    {
        attackConfirm = false;
        combatFinished = false;

        controller = GetComponent<playerController>();
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        offenceDeck = GetComponentInChildren<offenceDeckPile>();
        offenceDeck.DrawCards();

        combatSystem = combatManager.GetComponent<combatSystem>();
        combatSystem.combatComplete += AttackOver;
    }

    public override void UpdateState(playerStateManager player)
    {
        if (combatFinished)
        {
            player.ChangeState(player.MoveState);
        }
    }


    public override void ExitState(playerStateManager player)
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;

        combatSystem.combatComplete -= AttackOver;
    }

    public void DecidingUp(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[1];
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[3];
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[0];
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[1];
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        offenceCard attackCard = selectedCard.GetComponent<offenceCard>();
        if(controller.GetModel.CurrentMana >= attackCard.ManaCost && !attackConfirm)
        {
            combatSystem.AttackerReady(this.gameObject, attackCard.AttackValue);
            attackConfirm = true;
        }
    }

    public void AttackOver(object sender, EventArgs e)
    {
        combatFinished = true;
    }
}
