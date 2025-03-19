using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The cursing state allows the player to choose any player (including themselves) to obtain an omen
/// </summary>

public class cursingState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm
{
    //This section is using the same process as the targeting state
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private turnManager turnManager;
    [SerializeField] private GameObject[] selectPlayers = new GameObject[4];
    [SerializeField] private GameObject selectedPlayer;

    private bool playerSelected;

    //This process is being use similar to the item state
    [SerializeField] private List<itemStats> possibleOmens;
    [SerializeField] private itemStats selectedOmen;
    private itemDeckPool itemDeck;

    [SerializeField] private GameObject[] checkingAvailability;
    [SerializeField] private int unavailablePlayers;


    public override void EnterState(playerStateManager player)
    {
        possibleOmens.Clear();
        selectedOmen = null;
        playerSelected = false;
        checkingAvailability = new GameObject[4];

        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        turnManager = Singleton<turnManager>.Instance;

        for(int i = 0; i < turnManager.GetPlayers.Length; i++)
        {
            selectPlayers[i] = turnManager.GetPlayers[i];
        }

        for (int i = 0; i < selectPlayers.Length; i++) 
        { 
            itemDeck = selectPlayers[i].GetComponentInChildren<itemDeckPool>();
            if(itemDeck == null)
            {
                unavailablePlayers++;
            }

            else
            {
                checkingAvailability[i] = itemDeck.GetAvailableItem();
                if (checkingAvailability[i] == null)
                {
                    unavailablePlayers++;
                }
            }
        }

        if (unavailablePlayers == selectPlayers.Length) 
        { 
            playerSelected = true;
            Debug.Log("Everyone has a full inventory");
        }
    }

    //Update State will be playing constantly in which provides the code to keep on running
    //This state should also provide a condition to exit the state
    public override void UpdateState(playerStateManager player)
    {
        if (playerSelected) 
        { 
            player.ChangeState(player.InactiveState);
        }
    }

    //Exit State will only play once and that's when the state is being swapped out
    public override void ExitState(playerStateManager player)
    {
        itemDeck = null;
        
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

    //For Controls:
    // Left = Player One
    // Up = Player Two
    // Right = Player Three
    // Down = Player Four
    public void DecidingUp(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[1];
        Debug.Log(selectedPlayer);
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[3];
        Debug.Log(selectedPlayer);
    }


    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[0];
        Debug.Log(selectedPlayer);
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[2];
        Debug.Log(selectedPlayer);
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedPlayer != null) 
        {
            playerController controller = selectedPlayer.GetComponent<playerController>();
            possibleOmens = controller.GetData.possibleOmens;
            int randomOmen = UnityEngine.Random.Range(0, possibleOmens.Count);
            selectedOmen = possibleOmens[randomOmen];

            itemDeck = selectedPlayer.GetComponentInChildren<itemDeckPool>();
            GameObject omen = itemDeck.GetAvailableItem();
            if (omen != null) 
            {
                omen.SetActive(true);
                itemBehaviour item = omen.AddComponent<itemBehaviour>();
                item.CreateItem(selectedOmen);
                playerSelected = true;
            }

            else
            {
                Debug.LogWarning("Target Failed due to Full Deck of Items");
            }
        }

        else 
        {
            Debug.LogWarning("You haven't chosen a player yet");
        }
    }
}
