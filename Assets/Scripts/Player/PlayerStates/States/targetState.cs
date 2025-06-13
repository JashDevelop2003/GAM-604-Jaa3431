using System;
using System.Collections;
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

    private playerController controller;

    private bool playerSelected;

    [Header("User Interface")]
    //This is used to display the choosing player UI
    [SerializeField] private GameObject choosingPlayerUI;
    [SerializeField] private Color[] colourDisplay = new Color [2];
    [SerializeField] private Image[] sectionDisplay = new Image[4];
    [SerializeField] private TMP_Text[] playerText = new TMP_Text[4];
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip playerSound;
    [SerializeField] private AudioClip confirmSound;
    [SerializeField] private AudioClip declineSound;
    private soundManager soundManager;

    [Header("Animation")]
    private stateAnimation animator;


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
        soundManager = Singleton<soundManager>.Instance;
        Controls.upPressed += ChoosingSound;
        Controls.downPressed += ChoosingSound;
        Controls.leftPressed += ChoosingSound;
        Controls.rightPressed += ChoosingSound;

        controller = GetComponent<playerController>();

        animator = GetComponentInChildren<stateAnimation>();

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
            controller.ChangeMana(statusCard.ManaCost);
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
            controller.ChangeMana(statusCard.ManaCost);
            StartCoroutine(ActivateCard());
        }

        else if (statusCard.Target == targetEnum.Random) 
        {
            int randomPlayer = UnityEngine.Random.Range(0, turnManager.GetPlayers.Length);
            statusCard.ActivateAdditionalEffect();
            controller.ChangeMana(statusCard.ManaCost);
            statusCard.ActivateEffect(turnManager.GetPlayers[randomPlayer]);
            eventText.SetText("The random player that is affected by: " + statusCard.StatusCard.cardName + "is : " + turnManager.GetPlayers[randomPlayer].name);
            StartCoroutine(ActivateCard());
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
        ChoosingPlayer(1);
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        ChoosingPlayer(3);
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        ChoosingPlayer(0);
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        ChoosingPlayer(2);
    }

    private void ChoosingPlayer(int player)
    {
        if (selectPlayers[player] != null)
        {
            selectedPlayer = selectPlayers[player];
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
            statusCard.ActivateAdditionalEffect();
            statusCard.ActivateEffect(selectedPlayer);
            StartCoroutine(ActivateCard());
            soundManager.PlaySound(confirmSound);
            controller.ChangeMana(statusCard.ManaCost);
            eventText.SetText("Player has chosen " + selectedPlayer.name + " to be affected by: " + statusCard.StatusCard.cardName);
        }
        else
        {
            soundManager.PlaySound(declineSound);
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
        Controls.upPressed -= ChoosingSound;
        Controls.downPressed -= ChoosingSound;
        Controls.leftPressed -= ChoosingSound;
        Controls.rightPressed -= ChoosingSound;
        yield return new WaitForSeconds(2);
        playerSelected = true;
    }

    public void ChoosingSound(object sender, EventArgs e)
    {
        soundManager.PlaySound(playerSound);
    }

}
