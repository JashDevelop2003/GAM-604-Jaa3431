using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Tomorrow: Improve the States in order to be tested
public class decidingState : playerStateBase, IDecideDown, IDecideUp, IDecideRight, IDecideLeft, IConfirm

{
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }

    }

    public override void EnterState(playerStateManager player)
    {
        controls = GetComponent<boardControls>();
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;

    }

    public override void UpdateState(playerStateManager player)
    {
        Debug.LogWarning("Needs Moving State");
    }

    public override void ExitState(playerStateManager player)
    {
        Controls.upPressed += DecidingUp;
        Controls.downPressed += DecidingDown;
        Controls.leftPressed += DecidingLeft;
        Controls.rightPressed += DecidingRight;
        Controls.confirmPressed += ConfirmingChoice;
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
