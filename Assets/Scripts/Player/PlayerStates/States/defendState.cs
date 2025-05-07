using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class defendState : playerStateBase, IDefendUp, IDefendDown, IDefendLeft, IDefendRight, IDefendConfirm, IReveal
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

    private defenceDeckPile defenceDeck;
    public defenceDeckPile DefenceDeck
    {
        get { return defenceDeck; }
    }

    private GameObject selectedCard;
    private defenceCard defendCard;

    private int defendValue;
    private int manaCost;
    [SerializeField] private int lowestManaCost;

    private bool defendConfirm;
    private bool combatFinished;
    private bool unableDefend;

    private bool isRevealed;

    //This is to Add Item Events when Defending
    public event EventHandler defendItemEvents;

    //This is to add UI to the cards and add description of the card.
    [Header("User Interface")]
    [SerializeField] private GameObject defendingDisplay;
    [SerializeField] private TMP_Text[] manaCostText = new TMP_Text[4];
    [SerializeField] private TMP_Text[] cardNameText = new TMP_Text[4];
    [SerializeField] private GameObject defendPanel;
    [SerializeField] private TMP_Text defenceCardDescription;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip choiceSound;
    [SerializeField] private AudioClip declineSound;
    [SerializeField] private AudioClip revealSound;
    private soundManager soundManager;



    public override void EnterState(playerStateManager player)
    {
        defendConfirm = false;
        combatFinished = false;
        unableDefend = false;
        isRevealed = false;
        selectedCard = null;
        lowestManaCost = 99;

        controller = GetComponent<playerController>();
        effects = GetComponent<currentEffects>();
        controls = GetComponent<boardControls>();
        soundManager = Singleton<soundManager>.Instance;
        combatSystem = combatSystem.instance;

        defenceDeck = GetComponentInChildren<defenceDeckPile>();

        StartCoroutine(WaitForTurn());
    }

    IEnumerator WaitForTurn()
    {
        //This Coroutine waits until the attacker has chosen the card. Once becoming true the player can then take their turn to defend
        yield return new WaitUntil(() => combatSystem.AttackerIsReady == true);
        Controls.upPressed += DefendingUp;
        Controls.downPressed += DefendingDown;
        Controls.leftPressed += DefendingLeft;
        Controls.rightPressed += DefendingRight;
        Controls.upPressed += ChoosingSound;
        Controls.downPressed += ChoosingSound;
        Controls.leftPressed += ChoosingSound;
        Controls.rightPressed += ChoosingSound;
        Controls.confirmPressed += ConfirmingDefend;
        Controls.revealPressed += Reveal;


        defenceDeck.DrawCards();
        for (int i = 0; i < defenceDeck.SelectedCards.Length; i++)
        {
            defendCard = defenceDeck.SelectedCards[i].GetComponent<defenceCard>();
            cardNameText[i].SetText(defendCard.DefendCard.cardName);
            manaCostText[i].SetText(defendCard.DefendCard.manaCost.ToString());
            if (defendCard.ManaCost < lowestManaCost)
            {
                lowestManaCost = defendCard.ManaCost;
            }
        }

        combatSystem.combatComplete += DefendOver;
        defendPanel.SetActive(true);
        defendingDisplay.SetActive(true);
        defenceCardDescription.SetText("Press R to Reveal the selected card's description & Press R again to Hide the description");

        //If the player doesn't have enough mana for the lowest mana card or is stunned their defence becomes 0
        if (controller.GetModel.CurrentMana < lowestManaCost || effects.Stunned)
        {
            combatSystem.DefenderReady(this.gameObject, 0);
            CardSelected();
            unableDefend = true;
            defenceCardDescription.SetText("Defender is Ready: Unable to Choose a Card");
        }

        if (!unableDefend && effects.Confused)
        {
            int randomInt = UnityEngine.Random.Range(0, defenceDeck.SelectedCards.Length);
            selectedCard = defenceDeck.SelectedCards[randomInt];
            defendCard = selectedCard.GetComponent<defenceCard>();

            while (defendCard.ManaCost > controller.GetModel.CurrentMana)
            {
                randomInt = UnityEngine.Random.Range(0, defenceDeck.SelectedCards.Length);
                selectedCard = defenceDeck.SelectedCards[randomInt];
                defendCard = selectedCard.GetComponent<defenceCard>();
            }

            combatSystem.DefenderReady(this.gameObject, defendCard.DefendValue);
            controller.ChangeMana(defendCard.ManaCost);
            CardSelected();
            defenceCardDescription.SetText("Defender is Ready: Random Card was Selected due to being Confused");
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
        defendPanel.SetActive(false);
        defendingDisplay.SetActive(false);

        combatSystem.combatComplete -= DefendOver;
    }

    public void DefendingUp(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[1];
        defendCard = selectedCard.GetComponent<defenceCard>();
        if (isRevealed)
        {
            defenceCardDescription.SetText(defendCard.DefendCard.cardDescription);
        }

    }

    public void DefendingDown(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[3];
        defendCard = selectedCard.GetComponent<defenceCard>();
        if (isRevealed)
        {
            defenceCardDescription.SetText(defendCard.DefendCard.cardDescription);
        }
    }

    public void DefendingLeft(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[0];
        defendCard = selectedCard.GetComponent<defenceCard>();
        if (isRevealed)
        {
            defenceCardDescription.SetText(defendCard.DefendCard.cardDescription);
        }
    }

    public void DefendingRight(object sender, EventArgs e)
    {
        selectedCard = defenceDeck.SelectedCards[2];
        defendCard = selectedCard.GetComponent<defenceCard>();
        if (isRevealed)
        {
            defenceCardDescription.SetText(defendCard.DefendCard.cardDescription);
        }
    }

    public void ConfirmingDefend(object sender, EventArgs e)
    {
        if (selectedCard == null)
        {
            defenceCardDescription.SetText("You haven't chosen a card yet");
            soundManager.PlaySound(declineSound);
        }

        else if (controller.GetModel.CurrentMana >= defendCard.ManaCost && !defendConfirm)
        {
            defendCard.ApplyAdditionalEffect();
            combatSystem.DefenderReady(this.gameObject, defendCard.DefendValue);
            controller.ChangeMana(defendCard.ManaCost);
            defenceCardDescription.SetText("Defender is Ready");
            CardSelected();
        }
        else
        {
            defenceCardDescription.SetText("You don't have enough mana to use that defend card");
            soundManager.PlaySound(declineSound);
        }
    }

    public void Reveal(object sender, EventArgs e)
    {
        if (selectedCard != null)
        {
            if (!isRevealed)
            {
                defenceCardDescription.SetText(defendCard.DefendCard.cardDescription);
                soundManager.PlaySound(revealSound);
                isRevealed = true;
            }
            else
            {
                defenceCardDescription.SetText("Press P to Reveal the selected card's description & Press P again to Hide the description");
                isRevealed = false;
            }
        }
        else
        {
            defenceCardDescription.SetText("You haven't selected a card yet. Press P to Reveal the selected card's description & Press P again to Hide the description once selected a card");
        }
    }

    public void CardSelected()
    {
        defendConfirm = true;
        defendItemEvents?.Invoke(this, EventArgs.Empty);
        Controls.upPressed -= DefendingUp;
        Controls.downPressed -= DefendingDown;
        Controls.leftPressed -= DefendingLeft;
        Controls.rightPressed -= DefendingRight;
        Controls.upPressed -= ChoosingSound;
        Controls.downPressed -= ChoosingSound;
        Controls.leftPressed -= ChoosingSound;
        Controls.rightPressed -= ChoosingSound;
        Controls.confirmPressed -= ConfirmingDefend;
        Controls.revealPressed -= Reveal;
    }

    public void DefendOver(object sender, EventArgs e)
    {
        combatFinished = true;
    }

    public void ChoosingSound(object sender, EventArgs e)
    {
        soundManager.PlaySound(choiceSound);
    }
}
