using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class xIIIInactiveState : gameStateBase
{
    private bool startTurn;
    public bool StartingGame
    {
        get { return startTurn; }
        set { startTurn = value; }
    }
    private xIIIManager xIIIManager;
    private xIIIActiveState nextState;

    public override void EnterState(gameStateManager player)
    {
        startTurn = false;
        xIIIManager = Singleton<xIIIManager>.Instance;
        xIIIManager.changeTurn += StartTurn;
    }

    public override void UpdateState(gameStateManager player)
    {
        if (startTurn)
        {
            player.MinigameState = nextState;
            player.ChangeState(player.MinigameState);
        }
    }

    public override void ExitState(gameStateManager player)
    {
        xIIIManager.changeTurn -= StartTurn;
    }

    public void StartTurn(object sender, EventArgs e)
    {
        startTurn = true;
    }
}
