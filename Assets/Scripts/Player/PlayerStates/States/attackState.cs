using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm
{
    private combatSystem combatSystem;
    
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private playerController controller;

    private currentEffects effects;
    
    private offenceDeckPile offenceDeck;
    public offenceDeckPile OffenceDeck
    {
        get { return offenceDeck; }
    }

    private GameObject selectedCard;
    [SerializeField] int lowestManaCost;

    private bool attackConfirm;
    private bool combatFinished;
    private bool unableAttack;

    public override void EnterState(playerStateManager player)
    {
        attackConfirm = false;
        combatFinished = false;
        unableAttack = false;
        lowestManaCost = 99;

        controller = GetComponent<playerController>();
        controls = GetComponent<boardControls>();
        effects = GetComponent<currentEffects>();

        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        offenceDeck = GetComponentInChildren<offenceDeckPile>();
        offenceDeck.DrawCards();

        for (int i = 0; i < offenceDeck.SelectedCards.Length; i++)
        {
            offenceCard card = offenceDeck.SelectedCards[i].GetComponent<offenceCard>();
            if (card.ManaCost < lowestManaCost)
            {
                lowestManaCost = card.ManaCost;
            }
        }

        combatSystem = combatSystem.instance;
        combatSystem.combatComplete += AttackOver;

        if(controller.GetModel.CurrentMana < lowestManaCost)
        {
            combatSystem.AttackerReady(this.gameObject, 0);
            attackConfirm = true;
            unableAttack = true;
        }

        if (!unableAttack && effects.Confused)
        {
            int randomInt = UnityEngine.Random.Range(0, offenceDeck.SelectedCards.Length);
            selectedCard = offenceDeck.SelectedCards[randomInt];
            offenceCard offendCard = selectedCard.GetComponent<offenceCard>();

            while (offendCard.ManaCost > controller.GetModel.CurrentMana)
            {
                randomInt = UnityEngine.Random.Range(0, offenceDeck.SelectedCards.Length);
                selectedCard = offenceDeck.SelectedCards[randomInt];
                offendCard = selectedCard.GetComponent<offenceCard>();
            }

            combatSystem.AttackerReady(this.gameObject, offendCard.AttackValue);
            controller.ChangeMana(offendCard.ManaCost);
            attackConfirm = true;
        }
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
        selectedCard = offenceDeck.SelectedCards[2];
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        offenceCard attackCard = selectedCard.GetComponent<offenceCard>();
        if (attackCard == null) 
        {
            Debug.LogWarning("You haven't chosen a card yet");
        }
        
        if(controller.GetModel.CurrentMana >= attackCard.ManaCost && !attackConfirm)
        {
            attackCard.ApplyAdditionalEffect();
            combatSystem.AttackerReady(this.gameObject, attackCard.AttackValue);
            controller.ChangeMana(attackCard.ManaCost);
            attackConfirm = true;
        }
        else
        {
            Debug.LogWarning("You don't have enough Mana to use this attack card");
        }
    }

    public void AttackOver(object sender, EventArgs e)
    {
        combatFinished = true;
    }
}
