using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ruleState : gameStateBase, IConfirm
{
    private gameControls gameControls;
    public gameControls GameControls
    {
        get { return gameControls; }
        set { gameControls = value; }
    }

    private bool isReady;
    public bool IsReady
    {
        get { return isReady; }
    }

    [Header("User Interface")]
    [SerializeField] private GameObject rulesPanel;

    public override void EnterState(gameStateManager player)
    {
        gameControls = GetComponent<gameControls>();
        isReady = false;
        rulesPanel.SetActive(true);
        GameControls.pressedConfirm += ConfirmingChoice;
    }

    public override void UpdateState(gameStateManager player)
    {
        if (isReady) 
        { 
            player.ChangeState(player.MinigameState);
        }
    }

    public override void ExitState(gameStateManager player)
    {
        gameControls.pressedConfirm -= ConfirmingChoice;
        rulesPanel.SetActive(false);
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        isReady = true;
    }
}
