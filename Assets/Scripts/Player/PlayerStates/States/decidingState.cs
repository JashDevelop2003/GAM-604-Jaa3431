using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Tomorrow: Improve the States in order to be tested
public class decidingState : playerStateBase, IDecideDown, IDecideUp, IDecideRight, IDecideLeft, IConfirm

{
    public override void EnterState(playerStateManager player)
    {

        player.Controls.upPressed += DecidingUp;
        player.Controls.downPressed += DecidingDown;
        player.Controls.leftPressed += DecidingLeft;
        player.Controls.rightPressed += DecidingRight;
        player.Controls.confirmPressed += ConfirmingChoice;

    }

    public override void UpdateState(playerStateManager player)
    {
        Debug.LogWarning("Needs Moving State");
    }

    public override void ExitState(playerStateManager player)
    {
        player.Controls.upPressed += DecidingUp;
        player.Controls.downPressed += DecidingDown;
        player.Controls.leftPressed += DecidingLeft;
        player.Controls.rightPressed += DecidingRight;
        player.Controls.confirmPressed += ConfirmingChoice;
    }

    void DecidingUp(object sender, EventArgs e)
    {
        Debug.LogWarning("Needs to Implement Movement Card");
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        Debug.LogWarning("Needs to Implement Status Card");
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        Debug.LogWarning("Needs to Implement Movement Card");
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        Debug.LogWarning("Needs to Implement Movement Card");
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        Debug.Log("This will be changing to the Moving State");
    }
}
