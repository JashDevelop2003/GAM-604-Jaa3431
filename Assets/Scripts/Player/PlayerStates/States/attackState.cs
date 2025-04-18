using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class attackState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm, IRevealOffence
{
    //This is to collect the combat system in the scene
    private combatSystem combatSystem;
    
    //This collects the values of the control events and set a method to a suitable event
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    //this is used to check if the currnet mana can use one of the cards
    private playerController controller;

    //This is used to check if the player is confused and chooses a random card
    //This also checks if the player is stun in which will end their turn without using a card
    private currentEffects effects;
    
    //This collects the decks
    private offenceDeckPile offenceDeck;
    public offenceDeckPile OffenceDeck
    {
        get { return offenceDeck; }
    }

    //This is toidentify the selected card the player has chosen
    private GameObject selectedCard;
    private offenceCard attackCard;

    //This checks for the lowest mana cost card to see if the player can select the lowest cost card
    [SerializeField] int lowestManaCost;

    private bool attackConfirm;
    private bool combatFinished;
    private bool unableAttack;

    private bool isRevealed;

    //This is to add UI to the cards and add description of the card.
    [Header("User Interface")]
    [SerializeField] private GameObject attackingDisplay;
    [SerializeField] private TMP_Text[] manaCostText = new TMP_Text[4];
    [SerializeField] private TMP_Text[] cardNameText = new TMP_Text[4];
    [SerializeField] private GameObject attackPanel;
    [SerializeField] private TMP_Text offenceCardDescription;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip choiceSound;
    [SerializeField] private AudioClip declineSound;
    [SerializeField] private AudioClip revealSound;
    private soundManager soundManager;

    public override void EnterState(playerStateManager player)
    {
        attackConfirm = false;
        combatFinished = false;
        unableAttack = false;
        isRevealed = false;
        selectedCard = null;
        lowestManaCost = 99;

        controller = GetComponent<playerController>();
        controls = GetComponent<boardControls>();
        effects = GetComponent<currentEffects>();
        soundManager = Singleton<soundManager>.Instance;

        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.upPressed += ChoosingSound;
        Controls.downPressed += ChoosingSound;
        Controls.leftPressed += ChoosingSound;
        Controls.rightPressed += ChoosingSound;
        Controls.confirmPressed += ConfirmingChoice;
        Controls.revealOffencePressed += RevealOffence;

        offenceDeck = GetComponentInChildren<offenceDeckPile>();
        offenceDeck.DrawCards();
        attackPanel.SetActive(true);
        attackingDisplay.SetActive(true);
        offenceCardDescription.SetText("Press R to Reveal the selected card's description & Press R again to Hide the description");

        for (int i = 0; i < offenceDeck.SelectedCards.Length; i++)
        {
            attackCard = offenceDeck.SelectedCards[i].GetComponent<offenceCard>();
            cardNameText[i].SetText(attackCard.AttackCard.cardName);
            manaCostText[i].SetText(attackCard.AttackCard.manaCost.ToString());
            if (attackCard.ManaCost < lowestManaCost)
            {
                lowestManaCost = attackCard.ManaCost;
            }
        }

        combatSystem = combatSystem.instance;
        combatSystem.combatComplete += AttackOver;

        if(controller.GetModel.CurrentMana < lowestManaCost)
        {
            combatSystem.AttackerReady(this.gameObject, 0);
            CardSelected();
            offenceCardDescription.SetText("Attacker is Ready: Unable to Choose a Card");
            unableAttack = true;
        }

        if (!unableAttack && effects.Confused)
        {
            int randomInt = UnityEngine.Random.Range(0, offenceDeck.SelectedCards.Length);
            selectedCard = offenceDeck.SelectedCards[randomInt];
            attackCard = selectedCard.GetComponent<offenceCard>();

            while (attackCard.ManaCost > controller.GetModel.CurrentMana)
            {
                randomInt = UnityEngine.Random.Range(0, offenceDeck.SelectedCards.Length);
                selectedCard = offenceDeck.SelectedCards[randomInt];
                attackCard = selectedCard.GetComponent<offenceCard>();
            }

            combatSystem.AttackerReady(this.gameObject, attackCard.AttackValue);
            controller.ChangeMana(attackCard.ManaCost);
            CardSelected();
            offenceCardDescription.SetText("Attacker is Ready: Random Card was Selected due to being Confused");
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
        attackPanel.SetActive(false);
        attackingDisplay.SetActive(false);

        combatSystem.combatComplete -= AttackOver;
    }

    public void DecidingUp(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[1];
        attackCard = selectedCard.GetComponent<offenceCard>();
        if (isRevealed)
        {
            offenceCardDescription.SetText(attackCard.AttackCard.cardDescription);
        }
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[3];
        attackCard = selectedCard.GetComponent<offenceCard>();
        if (isRevealed)
        {
            offenceCardDescription.SetText(attackCard.AttackCard.cardDescription);
        }
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[0];
        attackCard = selectedCard.GetComponent<offenceCard>();
        if (isRevealed)
        {
            offenceCardDescription.SetText(attackCard.AttackCard.cardDescription);
        }
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedCard = offenceDeck.SelectedCards[2];
        attackCard = selectedCard.GetComponent<offenceCard>();
        if (isRevealed)
        {
            offenceCardDescription.SetText(attackCard.AttackCard.cardDescription);
        }
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedCard == null) 
        {
            offenceCardDescription.SetText("You haven't chosen a card yet");
            soundManager.PlaySound(declineSound);
        }
        
        else if(controller.GetModel.CurrentMana >= attackCard.ManaCost && !attackConfirm)
        {
            attackCard.ApplyAdditionalEffect();
            combatSystem.AttackerReady(this.gameObject, attackCard.AttackValue);
            controller.ChangeMana(attackCard.ManaCost);
            offenceCardDescription.SetText("Attacker is Ready");
            CardSelected();
        }
        else
        {
            offenceCardDescription.SetText("You don't have enough Mana to use this attack card");
            soundManager.PlaySound(declineSound);
        }
    }

    public void RevealOffence(object sender, EventArgs e)
    {
        if (selectedCard != null) 
        {
            if (!isRevealed)
            {
                offenceCardDescription.SetText(attackCard.AttackCard.cardDescription);
                soundManager.PlaySound(revealSound);
                isRevealed = true;
            }
            else
            {
                offenceCardDescription.SetText("Press R to Reveal the selected card's description & Press R again to Hide the description");
                isRevealed = false;
            }
        }
        else
        {
            offenceCardDescription.SetText("You haven't selected a card yet Press R to Reveal the selected card's description & Press R again to Hide the description once selected a card");
        }
    }

    public void CardSelected()
    {
        attackConfirm = true;
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.upPressed -= ChoosingSound;
        Controls.downPressed -= ChoosingSound;
        Controls.leftPressed -= ChoosingSound;
        Controls.rightPressed -= ChoosingSound;
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.revealOffencePressed -= RevealOffence;
    }

    public void AttackOver(object sender, EventArgs e)
    {
        combatFinished = true;
    }

    public void ChoosingSound(object sender, EventArgs e)
    {
        soundManager.PlaySound(choiceSound);
    }
}
