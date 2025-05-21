using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameInactiveState : gameStateBase
{
    private bool beginMinigame;


    public override void EnterState(gameStateManager player)
    {
        beginMinigame = false;
        player.beginEvent += BeginMinigame;
    }

    public override void UpdateState(gameStateManager player)
    {
        if (beginMinigame)
        {
            player.ChangeState(player.RuleState);
        }
    }

    public override void ExitState(gameStateManager player)
    {
        player.beginEvent -= BeginMinigame;
    }

    public void BeginMinigame(object sender, EventArgs e)
    {
        beginMinigame = true;
    }
}
