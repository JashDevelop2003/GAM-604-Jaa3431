using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class targetState : playerStateBase, IDecideUp, IDecideDown, IDecideLeft, IDecideRight, IConfirm
{
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    [SerializeField] private GameObject selectedCard;
    private statusCard statusCard;
    private turnManager turnManager;
    [SerializeField] private GameObject[] selectPlayers = new GameObject[4];
    [SerializeField] private GameObject selectedPlayer;

    private bool playerSelected;

    //This is used to display the choosing player UI
    [SerializeField] private GameObject choosingPlayerUI;
    [SerializeField] private Color[] colourDisplay = new Color [2];
    [SerializeField] private Image[] sectionDisplay = new Image[4];
    [SerializeField] private TMP_Text[] playerText = new TMP_Text[4];
    [SerializeField] private TMP_Text eventText;


    public override void EnterState(playerStateManager player)
    {
        selectedPlayer = null;
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

        statusCard = selectedCard.GetComponent<statusCard>();
        turnManager = Singleton<turnManager>.Instance;

        if (statusCard.Target == targetEnum.Any)
        {
            choosingPlayerUI.SetActive(true);

            for (int i = 0; i < turnManager.GetPlayers.Length; i++)
            {
                selectPlayers[i] = turnManager.GetPlayers[i];
                playerText[i].SetText(turnManager.GetPlayers[i].name);
            }

            for(int i = 0; i < selectPlayers.Length; i++)
            {
                if(selectPlayers[i] != null)
                {
                    sectionDisplay[i].color = colourDisplay[0];
                }

                else
                {
                    sectionDisplay[i].color = colourDisplay[1];
                }
            }
        }

        else if (statusCard.Target == targetEnum.Self)
        {
            statusCard.ActivateAdditionalEffect();

            statusCard.ActivateEffect(this.gameObject);
            eventText.SetText("The current Player is affected by: " + statusCard.StatusCard.cardName);
            StartCoroutine(ActivateCard());
        }

        else if (statusCard.Target == targetEnum.All)
        {
            statusCard.ActivateAdditionalEffect();
            for (int i = 0; i < turnManager.GetPlayers.Length; i++)
            {
                statusCard.ActivateEffect(turnManager.GetPlayers[i]);
            }
            eventText.SetText("Everyone is affected by: " + statusCard.StatusCard.cardName);
            StartCoroutine(ActivateCard());
        }

        else if (statusCard.Target == targetEnum.Random) 
        {
            int randomPlayer = UnityEngine.Random.Range(0, turnManager.GetPlayers.Length);
            statusCard.ActivateAdditionalEffect();
            statusCard.ActivateEffect(turnManager.GetPlayers[randomPlayer]);
            eventText.SetText("The random player that is affected by: " + statusCard.StatusCard.cardName + "is : " + turnManager.GetPlayers[randomPlayer].name);
            StartCoroutine(ActivateCard());
        }

        else
        {
            Debug.LogError("Unsuitable Target is Set on Selected Card");
        }

    }

    public override void UpdateState(playerStateManager player)
    {
        if (playerSelected) 
        {
            player.ChangeState(player.DecidingState);
        }
    }

    public override void ExitState(playerStateManager player) 
    {
        selectedPlayer = null;
        selectedCard = null;
        playerSelected = false;
        for (int i = 0; i < selectPlayers.Length; i++) 
        { 
            selectPlayers [i] = null;
        }

        choosingPlayerUI.SetActive(false);
    }   

    public void CollectStatusCard(GameObject statCard)
    {
        selectedCard = statCard;

    }


    //For Controls:
    // Left = Player One
    // Up = Player Two
    // Right = Player Three
    // Down = Player Four

    public void DecidingUp(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[1];
        eventText.SetText("Target: " + selectedPlayer.name);
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[3];
        eventText.SetText("Target: " + selectedPlayer.name);
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[0];
        eventText.SetText("Target: " + selectedPlayer.name);
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[2];
        eventText.SetText("Target: " + selectedPlayer.name);
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedPlayer != null) 
        {
            statusCard.ActivateAdditionalEffect();
            statusCard.ActivateEffect(selectedPlayer);
            StartCoroutine(ActivateCard());
            eventText.SetText("Player has chosen " + selectedPlayer.name + " to be affected by: " + statusCard.StatusCard.cardName);
        }
        else
        {
            eventText.SetText("You haven't chosen a player");
        }
    }

    IEnumerator ActivateCard()
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
        yield return new WaitForSeconds(2);
        playerSelected = true;
    }

}
