using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defendState : playerStateBase, IDefendUp, IDefendDown, IDefendLeft, IDefendRight, IDefendConfirm
{
    [SerializeField] private GameObject combatManager;
    private combatSystem combatSystem;

    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private playerController controller;

    private currentEffects effects;

    private defenceDeckPile defenceDeck;
    public defenceDeckPile DefenceDeck
    {
        get { return defenceDeck; }
    }

    private GameObject selectedCard;

    private int defendValue;
    private int manaCost;
    [SerializeField] private int lowestManaCost;

    private bool defendConfirm;
    private bool combatFinished;
    private bool unableDefend;

    public override void EnterState(playerStateManager player)
    {
        defendConfirm = false;
        combatFinished = false;
        unableDefend = false;
        lowestManaCost = 99;

        controller = GetComponent<playerController>();
        effects = GetComponent<currentEffects>();
        controls = GetComponent<boardControls>();
        Controls.defendUpPressed += DefendingUp;
        Controls.defendDownPressed += DefendingDown;
        Controls.defendLeftPressed += DefendingLeft;
        Controls.defendRightPressed += DefendingRight;
        Controls.defendConfirmPressed += ConfirmingDefend;

        defenceDeck = GetComponentInChildren<defenceDeckPile>();
        defenceDeck.DrawCards();
        for (int i = 0; i < defenceDeck.SelectedCards.Length; i++)
        {
            defenceCard card = defenceDeck.SelectedCards[i].GetComponent<defenceCard>();
            if (card.ManaCost < lowestManaCost)
            {
                lowestManaCost = card.ManaCost;
            }
        }

        combatSystem = combatManager.GetComponent<combatSystem>();
        combatSystem.combatComplete += DefendOver;

        //If the player doesn't have enough mana for the lowest mana card or is stunned their defence becomes 0
        if (controller.GetModel.CurrentMana < lowestManaCost || effects.Stunned)
        {
            combatSystem.DefenderReady(this.gameObject, 0);
            defendConfirm = true;
            unableDefend = true;
        }

        if (!unableDefend && effects.Confused)
        {
            int randomInt = UnityEngine.Random.Range(0, defenceDeck.SelectedCards.Length);
            selectedCard = defenceDeck.SelectedCards[randomInt];
            defenceCard defendCard = selectedCard.GetComponent<defenceCard>();

            while (defendCard.ManaCost > controller.GetModel.CurrentMana)
            {
                randomInt = UnityEngine.Random.Range(0, defenceDeck.SelectedCards.Length);
                selectedCard = defenceDeck.SelectedCards[randomInt];
                defendCard = selectedCard.GetComponent<defenceCard>();
            }

            combatSystem.DefenderReady(this.gameObject, defendCard.DefendValue);
            controller.ChangeMana(defendCard.ManaCost);
            defendConfirm = true;
        }
    }


    public override void UpdateState(playerStateManager player)
    {
        if (combatFinished)
        {
            player.ChangeState(player.InactiveState);
        }
    }


    public override void ExitState(playerStateManager player)
    {
        Controls.defendUpPressed -= DefendingUp;
        Controls.defendDownPressed -= DefendingDown;
        Controls.defendLeftPressed -= DefendingLeft;
        Controls.defendRightPressed -= DefendingRight;
        Controls.defendConfirmPressed -= ConfirmingDefend;

        combatSystem.combatComplete += DefendOver;
    }

    public void DefendingUp(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[1];
    }

    public void DefendingDown(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[3];
    }

    public void DefendingLeft(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[0];
    }

    public void DefendingRight(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[2];
    }

    public void ConfirmingDefend(object sender, EventArgs e)
    {
        defenceCard defendCard = selectedCard.GetComponent<defenceCard>();
        if (defendCard == null)
        {
            Debug.LogWarning("You haven't chosen a card yet");
        }

        if (controller.GetModel.CurrentMana >= defendCard.ManaCost && !defendConfirm)
        {
            defendCard.ApplyAdditionalEffect();
            combatSystem.DefenderReady(this.gameObject, defendCard.DefendValue);
            controller.ChangeMana(defendCard.ManaCost);
            defendConfirm = true;
        }
        else
        {
            Debug.LogWarning("You don't have enough mana to use that defend card");
        }
    }

    public void DefendOver(object sender, EventArgs e)
    {
        combatFinished = true;
    }
}
