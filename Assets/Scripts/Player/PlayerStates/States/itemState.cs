using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

/// <summary>
/// This state allows the player to either obtain a relic or move to the cursing state to select an player to obtain a omen item
/// If the player chooses relic then they obtain a relice
/// If the player choosess omen then the player moves to the cursing state
/// </summary>

public class itemState : playerStateBase, IDecideLeft, IDecideRight, IConfirm
{

    //the controls are used to obtain a relic or give a curse to a player
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    //the controller is used to provide the possible items the player can obtain based on their character
    private playerController controller;

    //This is for the relics and provides the possible relics to select from to give to the player landing on the item space
    [SerializeField] private List<itemStats> possibleRelics;
    private itemDeckPool itemDeck;

    public itemEnum typeSelected;

    //this boolean is to change the state once the player has confirm the type they want to provide
    private bool itemDecided;

    private GameObject checkingAvailability;

    [Header("User Interface")]
    //This is used to display picking either an relic or omen.
    [SerializeField] private GameObject pickingItemUI;
    [SerializeField] private Color[] setColour = new Color [3];
    [SerializeField] private Image[] sectionDisplay = new Image[4];
    [SerializeField] private TMP_Text[] itemText = new TMP_Text[2];
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] itemSound = new AudioClip[2];
    [SerializeField] private AudioClip confirmSound;
    private soundManager soundManager;

    public override void EnterState(playerStateManager player)
    {
        //This reference the sound manager as a singleton
        soundManager = Singleton<soundManager>.Instance;

        //These are to prevent the process ending with the player not having a chance to select
        typeSelected = itemEnum.Null;
        itemDecided = false;
        checkingAvailability = null;

        pickingItemUI.SetActive(true);
        //This display the options for the player to either obtain a relic or make someone obtain a omen
        sectionDisplay[0].color = setColour[0];
        itemText[0].SetText("Relic");
        sectionDisplay[1].color = setColour[1];
        itemText[1].SetText("Omen");

        //prevent more than 2 options for showing up
        sectionDisplay[2].color = setColour[2];
        sectionDisplay[3].color = setColour[2];
        eventText.SetText("Do you want to obtain a relic or give someone a omen?");

        //The controller references the possible relic cards from the character data
        controller = GetComponent<playerController>();
        possibleRelics = controller.GetData.possibleRelics;

        //This provides the controls to decide and confirm on the type of item use
        controls = GetComponent<boardControls>();
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.leftPressed += ItemSound;
        Controls.rightPressed += ItemSound;
        Controls.confirmPressed += ConfirmingChoice;

        //the relic pool is required to pool a relic if the player has chosen an item
        itemDeck = GetComponentInChildren<itemDeckPool>();

        //This checks if there is an availabe slot for the player to add a relic
        checkingAvailability = itemDeck.GetAvailableItem();

        if (checkingAvailability == null) 
        {
            typeSelected = itemEnum.Omen;
            soundManager.PlaySound(itemSound[2]);
            StartCoroutine(WaitingforItem());
            eventText.SetText("You have to play omens since you don't have any space for a relic. Select a player (Except for yourself) to obtain a omen");
        }

    }

    public override void UpdateState(playerStateManager player)
    {
        if (itemDecided)
        {
            if(typeSelected == itemEnum.Relic)
            {
                player.ChangeState(player.InactiveState);
            }

            else if(typeSelected == itemEnum.Omen)
            {
                player.ChangeState(player.CursingState);
            }
        }
    }

    public override void ExitState(playerStateManager player) 
    {
        pickingItemUI.SetActive(false);
    }

    //Choosing Left will make the selected type to relic
    public void DecidingLeft(object sender, EventArgs e)
    {
        typeSelected = itemEnum.Relic;
        eventText.SetText(typeSelected.ToString());
    }

    //choosing Right will make the selected type to Omen
    public void DecidingRight(object sender, EventArgs e)
    {
        typeSelected = itemEnum.Omen;
        eventText.SetText(typeSelected.ToString());
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if(typeSelected == itemEnum.Relic)
        {
            itemDeck.CreateItem(itemEnum.Relic);
            StartCoroutine(WaitingforItem());
        }
        else if( typeSelected == itemEnum.Omen)
        {
            eventText.SetText(typeSelected.ToString() + " was selected. Select a player to obtain a omen");
            StartCoroutine(WaitingforItem());
        }
        else
        {
            eventText.SetText("You haven't selected anything yet");
        }
    }

    IEnumerator WaitingforItem()
    {
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.leftPressed -= ItemSound;
        Controls.rightPressed -= ItemSound;
        Controls.confirmPressed -= ConfirmingChoice;
        yield return new WaitForSeconds(4);
        itemDecided = true;
    }

    // Relic Sound = 0
    // Omen Sound = 1
    public void ItemSound(object sender, EventArgs e)
    {
        soundManager.PlaySound(itemSound[(int)typeSelected]);
    }
}
