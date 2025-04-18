using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    //This is used to display the choosing player UI
    [Header("User Interface")]
    [SerializeField] private GameObject choosingPlayerUI;
    [SerializeField] private Color[] colourDisplay = new Color[2];
    [SerializeField] private Image[] sectionDisplay = new Image[4];
    [SerializeField] private TMP_Text[] playerText = new TMP_Text[4];
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip playerSound;
    [SerializeField] private AudioClip confirmSound;
    [SerializeField] private AudioClip declineSound;
    private soundManager soundManager;


    public override void EnterState(playerStateManager player)
    {
        possibleOmens = null;
        selectedOmen = null;
        playerSelected = false;

        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        soundManager = Singleton<soundManager>.Instance;
        Controls.upPressed += ChoosingSound;
        Controls.downPressed += ChoosingSound;
        Controls.leftPressed += ChoosingSound;
        Controls.rightPressed += ChoosingSound;

        turnManager = Singleton<turnManager>.Instance;
        checkingAvailability = new GameObject[turnManager.GetPlayers.Length];
        
        choosingPlayerUI.SetActive(true);
        
        for (int i = 0; i < turnManager.GetPlayers.Length; i++)
        {
            selectPlayers[i] = turnManager.GetPlayers[i];
            playerText[i].SetText(turnManager.GetPlayers[i].name);
        }

        for (int i = 0; i < selectPlayers.Length; i++) 
        {
            if (selectPlayers[i] == null)
            {
                unavailablePlayers++;
                sectionDisplay[i].color = colourDisplay[0];
            }
            else
            {
                itemDeck = selectPlayers[i].GetComponentInChildren<itemDeckPool>();
                checkingAvailability[i] = itemDeck.GetAvailableItem();
                if (checkingAvailability[i] == null)
                {
                    unavailablePlayers++;
                    sectionDisplay[i].color = colourDisplay[0];
                }

                else
                {
                    sectionDisplay[i].color = colourDisplay[1];
                }
            }
        }

        if (unavailablePlayers == selectPlayers.Length) 
        {
            StartCoroutine(CursingPlayer());
            eventText.SetText("Everyone has a full inventory. Ending Turn");
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
        choosingPlayerUI.SetActive(false);
    }

    //For Controls:
    // Left = Player One
    // Up = Player Two
    // Right = Player Three
    // Down = Player Four
    public void DecidingUp(object sender, EventArgs e)
    {
        if (selectPlayers[1] != null)
        {
            selectedPlayer = selectPlayers[1];
            eventText.SetText(selectedPlayer.name);
        }
        else
        {
            selectedPlayer = null;
            eventText.SetText("N/A");
        }
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        if (selectPlayers[3] != null)
        {
            selectedPlayer = selectPlayers[3];
            eventText.SetText(selectedPlayer.name);
        }
        else
        {
            selectedPlayer = null;
            eventText.SetText("N/A");
        }
    }


    public void DecidingLeft(object sender, EventArgs e)
    {
        if (selectPlayers[0] != null)
        {
            selectedPlayer = selectPlayers[0];
            eventText.SetText(selectedPlayer.name);
        }
        else
        {
            selectedPlayer = null;
            eventText.SetText("N/A");
        }
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        if (selectPlayers[2] != null)
        {
            selectedPlayer = selectPlayers[2];
            eventText.SetText(selectedPlayer.name);
        }
        else
        {
            selectedPlayer = null;
            eventText.SetText("N/A");
        }
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
                controller.IncrementDeck(deckTypeEnum.Item);
                soundManager.PlaySound(confirmSound);
                eventText.SetText(selectedPlayer.name + " has been selected. Omen obtained: " + item.Item.itemName + " : " + item.Item.itemDescription);
                StartCoroutine(CursingPlayer());
            }

            else
            {
                eventText.SetText("Target Failed due to Full Deck of Items");
                soundManager.PlaySound(declineSound);
            }
        }

        else 
        {
            eventText.SetText("You haven't chosen a player yet");
            soundManager.PlaySound(declineSound);
        }
    }

    IEnumerator CursingPlayer()
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.upPressed -= ChoosingSound;
        Controls.downPressed -= ChoosingSound;
        Controls.leftPressed -= ChoosingSound;
        Controls.rightPressed -= ChoosingSound;
        yield return new WaitForSeconds(4);
        playerSelected = true;
    }

    public void ChoosingSound(object sender, EventArgs e)
    {
        soundManager.PlaySound(playerSound);
    }
}
