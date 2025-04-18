using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct PossibleWinnings
{
    public outcomeEnum outcome;
    public int cashPrize;
    public int bonusChance;
    public Sprite[] symbol;
}

public class spinState : playerStateBase, IConfirm, ICancel
{
    //the controls are used to select the cards or even ignore collecting
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private playerController controller;

    [SerializeField] private List<itemStats> possibleItems;
    private itemStats selectedItem;
    private itemDeckPool itemDeck;
    private GameObject checkingAvailability;

    private bool spinEnded;
    [SerializeField] private PossibleWinnings spinOutcome;

    [Header("Possible Winnings Struct")]
    [SerializeField] private PossibleWinnings[] winnings = new PossibleWinnings[10];

    [Header("User Interface")]
    [SerializeField] private GameObject fruitMachineUI;
    [SerializeField] private GameObject[] dialSymbols = new GameObject[3];
    [SerializeField] private Image[] symbolIcon = new Image[3];
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip confirmSound;
    [SerializeField] private AudioClip rejectSound;
    [SerializeField] private AudioClip spinSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip luckySound;
    [SerializeField] private AudioClip unluckySound;
    private soundManager soundManager;

    public override void EnterState(playerStateManager player)
    {
        spinEnded = false;

        //this enables to deciding events towards selecting a type of card
        controls = GetComponent<boardControls>();
        Controls.confirmPressed += ConfirmingChoice;
        Controls.cancelPressed += Cancel;

        soundManager = Singleton<soundManager>.Instance;

        //the controller is referenced to collect the character data of the possible card to obtain
        controller = GetComponent<playerController>();
        possibleItems = controller.GetData.possibleRelics;

        //This checks if there is an avaialble slot for the player to create cards and items
        itemDeck = GetComponentInChildren<itemDeckPool>();
        checkingAvailability = itemDeck.GetAvailableItem();

        if(controller.GetModel.CurrentCash < 20)
        {
            StartCoroutine(SpinEnded(3));
            eventText.SetText("You don't have enough cash");
        }

        fruitMachineUI.SetActive(true);
        for (int i = 0; i < dialSymbols.Length; i++) 
        {
            dialSymbols[i].SetActive(false);
            symbolIcon[i].sprite = null;
        }
    }

    public override void UpdateState(playerStateManager player)
    {
        if (spinEnded)
        {
            player.ChangeState(player.InactiveState);
        }
    }

    public override void ExitState(playerStateManager player) 
    {
        fruitMachineUI.SetActive(false);
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {

        //This provides the probability of the outcome of the spin
        int outcome = UnityEngine.Random.Range(1, 101);

        //Outcome = Jumbled (30% Chance)
        if (outcome >= 1 && outcome <= 30) 
        {
            spinOutcome = winnings[0];
        }
        
        //Outcome = Cherries (15% Chance)
        else if(outcome >= 31 && outcome <= 45)
        {
            spinOutcome = winnings[1];
        }

        //Outcome = Mixed 7(11% Chance)
        else if (outcome >= 46 && outcome <= 56)
        {
            spinOutcome = winnings[2];
        }

        //Outcome Lemon (10% Chance)
        else if(outcome >= 57 && outcome <= 66)
        {
            spinOutcome = winnings[3];
        }

        //Outcome Grapes(10% Chance)
        else if(outcome >= 67 && outcome <= 76)
        {
            spinOutcome = winnings[4];
        }

        //Outcome Bar(8% Chance)
        else if (outcome >= 77 && outcome <= 84)
        {
            spinOutcome = winnings[5];
        }

        //Outcome Watermelon (7% Chance)
        else if (outcome >= 85 && outcome <= 91)
        {
            spinOutcome = winnings[6];
        }

        //Outcome Bell (5% Chance)
        else if (outcome >= 92 && outcome <= 96)
        {
            spinOutcome = winnings[7];
        }

        //Outcome Blue Seven (3% Chance)
        else if (outcome >= 97 && outcome <= 99)
        {
            spinOutcome = winnings[8];
        }

        //Outcome Red Seven (1% Chance)
        else if(outcome == 100)
        {
            spinOutcome = winnings[9];
        }

        else
        {
            Debug.LogError("Probability was off. Check the coding in when Confirming");
        }

        controller.ChangeCash(-20);
        soundManager.PlaySound(confirmSound);
        StartCoroutine(Spin());
    }

    public void Cancel(object sender, EventArgs e)
    {
        StartCoroutine(SpinEnded(2));
        soundManager.PlaySound(rejectSound);
        eventText.SetText("You decided to not spin");
    }

    IEnumerator Spin()
    {
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.cancelPressed -= Cancel;

        for(int i = 0; i < spinOutcome.symbol.Length; i++)
        {
            yield return new WaitForSeconds(i);
            dialSymbols[i].SetActive(true);
            symbolIcon[i].sprite = spinOutcome.symbol[i];
            soundManager.PlaySound(spinSound);
        }

        yield return new WaitForSeconds(1);
        if(spinOutcome.outcome == outcomeEnum.Jumbled)
        {
            StartCoroutine(SpinEnded(2));
            soundManager.PlaySound(loseSound);
            eventText.SetText("Unfortunate, You got Jumbled");
        }
        else
        {
            controller.ChangeCash(spinOutcome.cashPrize);
            soundManager.PlaySound(winSound);
            StartCoroutine(BonusChance(spinOutcome.bonusChance));
            eventText.SetText("Congratulations, you got: " + spinOutcome.outcome.ToString() + " Gain: " + spinOutcome.cashPrize.ToString() + " Cash. Bonus Chance: " + spinOutcome.bonusChance.ToString() + "% Chance");
        }
    }

    IEnumerator BonusChance(int chance)
    {
        int outcome = UnityEngine.Random.Range(1, 101);
        yield return new WaitForSeconds(4);
        if(outcome <= chance)
        {
            ObtainItem();
            soundManager.PlaySound(luckySound);
        }
        else
        {
            StartCoroutine(SpinEnded(2));
            eventText.SetText("Unlucky, No Bonus.");
            soundManager.PlaySound(unluckySound);
        }
    }

    void ObtainItem()
    {
        int selectedInt = UnityEngine.Random.Range(0, possibleItems.Count);
        selectedItem = possibleItems[selectedInt];

        checkingAvailability = itemDeck.GetAvailableItem();
        GameObject relic = checkingAvailability;
        if (relic != null)
        {
            relic.SetActive(true);
            itemBehaviour item = relic.AddComponent<itemBehaviour>();
            item.CreateItem(selectedItem);
            controller.IncrementDeck(deckTypeEnum.Item);

            eventText.SetText("Bonus Item: " + item.Item.itemName + " : " + item.Item.itemDescription + " Press Backspace once you're done shopping");
        }
        else
        {
            eventText.SetText("Despite Bonus, There is no Available Slot for Items so here's an extra 100 Cash");
            controller.ChangeCash(100);
        }

        StartCoroutine(SpinEnded(5));

    }

    IEnumerator SpinEnded(int timer)
    {
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.cancelPressed -= Cancel;
        yield return new WaitForSeconds(timer);
        spinEnded = true;
    }
}
