using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private itemStats selectedRelic;
    private itemDeckPool itemDeck;

    public itemEnum typeSelected;

    //this boolean is to change the state once the player has confirm the type they want to provide
    private bool itemDecided;

    [SerializeField] private GameObject checkingAvailability;

    public override void EnterState(playerStateManager player)
    {
        //These are to prevent the process ending with the player not having a chance to select
        typeSelected = itemEnum.Null;
        itemDecided = false;
        checkingAvailability = null;
        selectedRelic = null;

        //The controller references the possible relic cards from the character data
        controller = GetComponent<playerController>();
        possibleRelics = controller.GetData.possibleRelics;

        //This provides the controls to decide and confirm on the type of item use
        controls = GetComponent<boardControls>();
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        //the relic pool is required to pool a relic if the player has chosen an item
        itemDeck = GetComponentInChildren<itemDeckPool>();

        //This checks if there is an availabe slot for the player to add a relic
        checkingAvailability = itemDeck.GetAvailableItem();

        if (checkingAvailability == null) 
        {
            typeSelected = itemEnum.Omen;
            itemDecided = true;
            Debug.Log("You have to play omens");
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
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

    //Choosing Left will make the selected type to relic
    public void DecidingLeft(object sender, EventArgs e)
    {
        typeSelected = itemEnum.Relic;
        Debug.Log(typeSelected);
    }

    //choosing Right will make the selected type to Omen
    public void DecidingRight(object sender, EventArgs e)
    {
        typeSelected = itemEnum.Omen;
        Debug.Log(typeSelected);
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if(typeSelected == itemEnum.Relic)
        {
            int selectedInt = UnityEngine.Random.Range(0, possibleRelics.Count);
            selectedRelic = possibleRelics[selectedInt];
            GameObject relic = checkingAvailability;
            relic.SetActive(true);
            itemBehaviour item = relic.AddComponent<itemBehaviour>();
            item.CreateItem(selectedRelic);
            itemDecided = true;
        }
        else if( typeSelected == itemEnum.Omen)
        {
            itemDecided = true;
        }
        else
        {
            Debug.LogWarning("You haven't selected anything yet");
        }
    }
}
