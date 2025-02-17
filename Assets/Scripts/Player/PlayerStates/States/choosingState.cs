using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class choosingState : playerStateBase, IDecideUp, IDecideLeft, IDecideRight, IDecideDown, IConfirm
{
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    private directionEnum currentDirection;
    private GameObject multiplePathway;
    private pathSelection pathSelection;
    private List<GameObject> pathList;
    private GameObject selectedPathway;
    private GameObject[] pathDirection;

    private playerController controller;
    
    private bool hasSelected;

    public override void EnterState(playerStateManager player)
    {
        pathDirection = new GameObject[4];
        selectedPathway = null;
        hasSelected = false;
        controller = GetComponent<playerController>();
        controls = GetComponent<boardControls>();
        pathSelection.PathSelections(currentDirection);
        pathList = pathSelection.PlayerChoices;


        if (pathList.Count < 2)
        {
            controller.Path = pathList[0];
            hasSelected = true;
        }

        else
        {
            for (int i = 0; i < pathList.Count; i++) 
            {
                {
                    pathOrder path = pathList[i].GetComponent<pathOrder>();
                    if (path.Direction == directionEnum.Up)
                    {
                        pathDirection[0] = pathList[i];
                        Controls.upPressed += DecidingUp;
                    }
                    else if (path.Direction == directionEnum.Down)
                    {
                        pathDirection[1] = pathList[i];
                        Controls.downPressed += DecidingDown;
                    }
                    else if (path.Direction == directionEnum.Left)
                    {
                        pathDirection[2] = pathList[i];
                        Controls.leftPressed += DecidingLeft;
                    }
                    else if (path.Direction == directionEnum.Right)
                    {
                        pathDirection[3] = pathList[i];
                        Controls.rightPressed += DecidingRight;
                    }
                }
            }          

            Controls.confirmPressed += ConfirmingChoice;
        }


    }

    public override void UpdateState(playerStateManager player)
    {
        if (hasSelected)
        {
            player.ChangeState(player.MoveState);
        }
    }

    public override void ExitState(playerStateManager player) 
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

    public void CollectCurrentPath(GameObject multiPath, directionEnum direction)
    {
        currentDirection = direction;
        multiplePathway = multiPath;
        pathSelection = multiPath.GetComponent<pathSelection>();

    }

    void DecidingUp(object sender, EventArgs e)
    {
        selectedPathway = pathDirection[0];
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedPathway = pathDirection[1];
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedPathway = pathDirection[2];
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedPathway = pathDirection[3];
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (selectedPathway != null)
        {
            controller.Path = selectedPathway;
            hasSelected = true;
        }
        else
        {
            Debug.LogError("You cannot go that direction");
        }
    }

}
