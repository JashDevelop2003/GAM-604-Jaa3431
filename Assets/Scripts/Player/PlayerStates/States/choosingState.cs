using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// First Playable: The choosing state occurs when the player is on a multi path object
/// The state provides the options for the player to choose from and decide which path they want to take
/// If there is only one option, the player will be force to choose that path
/// </summary>

public class choosingState : playerStateBase, IDecideUp, IDecideLeft, IDecideRight, IDecideDown, IConfirm
{
    //The board controls are use depending on which paths are availble to take
    private boardControls controls;
    public boardControls Controls
    {
        get { return controls; }
        set { controls = value; }
    }

    //These variables are collect to identify which multi path object the player landed on and which direction they came from to get to that object
    //The direction enum is use to provide the player's current direction to apply restrictions for some paths
    //the multiplePathway is the multi path object which will be used for referencing the pathSelection to provide the paths for the player to choose from
    private directionEnum currentDirection;
    private GameObject multiplePathway;
    private pathSelection pathSelection;

    //The path has a struct inside of the pathSelection that is used to provide all the path choices available with the direction restriction and path object
    private List<GameObject> pathList;
    private GameObject selectedPathway;
    private GameObject[] pathDirection;

    //the controller is reference to apply the new path to follow once the path is selected
    private playerController controller;
    
    //the boolean is use to ensure that when true the current state changes
    private bool hasSelected;

    public override void EnterState(playerStateManager player)
    {
        //the pathDirection creates 4 arrays for up, down, left and right choices
        pathDirection = new GameObject[4];
        
        //the selectedPathway is turn to empty to prevent players from deciding a direction that's not meant to be chosen
        selectedPathway = null;

        //this boolean turns false to prevent the player from instantly changing states
        hasSelected = false;

        //the controller and controls are reference in the player object
        controller = GetComponent<playerController>();
        controls = GetComponent<boardControls>();

        //The multi path calls the method to check if any paths are restricted due to the current direction type
        pathSelection.PathSelections(currentDirection);

        //this creates all the available paths for the player to choose from
        pathList = pathSelection.PlayerChoices;

        //this checks if there is only 1 path or more
        //if there is only one path then the player is forced to choose that path
        if (pathList.Count < 2)
        {
            controller.Path = pathList[0];
            hasSelected = true;
        }

        //otherwise the controls are enable depending on the type of direction the path is
        else
        {
            //if the direction is Up then upPressed in controls is enabled
            //else if the direction is Down then downPressed in controls is enabled
            //else if the direction is Left then leftPressed in controls is enabled
            //else if the direction is Right then rightPressed in controls is enabled

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
        //once the player has decided the path or is force to take a path then the state goes back to move state
        if (hasSelected)
        {
            player.ChangeState(player.MoveState);
        }
    }

    //all controls that have events from this state are disabled
    public override void ExitState(playerStateManager player) 
    {
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
    }

    //This method collects the multi path object the player has landed and the current direction the player is heading
    public void CollectCurrentPath(GameObject multiPath, directionEnum direction)
    {
        currentDirection = direction;
        multiplePathway = multiPath;
        pathSelection = multiPath.GetComponent<pathSelection>();

    }

    //These Deciding Interfaces change the selected pathway to that path (if it has a path in the first place)
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

    //When confirming the interface method must check if there is a path for the player to use
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //if there is a pathway, then the controller's path becomes the selected path
        if (selectedPathway != null)
        {
            controller.Path = selectedPathway;
            hasSelected = true;
        }
        //else this infroms the player to choose another path
        else
        {
            Debug.LogError("You cannot go that direction");
        }
    }

}
