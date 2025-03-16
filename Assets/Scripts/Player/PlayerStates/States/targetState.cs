using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void EnterState(playerStateManager player)
    {
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
            for (int i = 0; i < turnManager.GetPlayers.Length; i++)
            {
                selectPlayers[i] = turnManager.GetPlayers[i];
            }
        }

        else if (statusCard.Target == targetEnum.Self)
        {
            statusCard.ActivateAdditionalEffect();

            statusCard.ActivateEffect(this.gameObject);
            playerSelected = true;
        }

        else if (statusCard.Target == targetEnum.All)
        {
            statusCard.ActivateAdditionalEffect();
            for (int i = 0; i < turnManager.GetPlayers.Length; i++)
            {
                statusCard.ActivateEffect(turnManager.GetPlayers[i]);
            }
            playerSelected = true;
        }

        else if (statusCard.Target == targetEnum.Random) 
        {
            int randomPlayer = UnityEngine.Random.Range(0, turnManager.GetPlayers.Length);
            statusCard.ActivateAdditionalEffect();
            statusCard.ActivateEffect(turnManager.GetPlayers[randomPlayer]);
            playerSelected = true;
        }

        else
        {
            Debug.LogWarning("Unsuitable Target is Set on Selected Card");
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

        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }   

    public void CollectStatusCard(GameObject statCard)
    {
        selectedCard = statCard;

    }

    public void DecidingUp(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[1];
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[3];
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[0];
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedPlayer = selectPlayers[2];
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedPlayer != null) 
        {
            statusCard.ActivateAdditionalEffect();
            statusCard.ActivateEffect(selectedPlayer);
            playerSelected = true;
        }
        else
        {
            Debug.LogWarning("You haven't chosen a player");
        }
    }

}
