using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    private List<int> pathDirectionInt;
    private GameObject selectedPathway;
    private GameObject[] pathDirection;

    //the controller is reference to apply the new path to follow once the path is selected
    private playerController controller;

    //The current effects is reference to choose a random direction if they are blind
    private currentEffects effects;
    
    //the boolean is use to ensure that when true the current state changes
    private bool hasSelected;

    [Header("User Interface")]
    //This is used to display the choosing player UI
    [SerializeField] private GameObject choosingPathUI;
    [SerializeField] private Color[] colourDisplay = new Color[2];
    [SerializeField] private Image[] sectionDisplay = new Image[4];
    [SerializeField] private TMP_Text[] pathText = new TMP_Text[4];
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effects")]
    private soundManager soundManager;
    [SerializeField] private AudioClip pathSound;
    [SerializeField] private AudioClip confirmSound;
    [SerializeField] private AudioClip declineSound;

    public override void EnterState(playerStateManager player)
    {
        //the pathDirection creates 4 arrays for up, down, left and right choices
        pathDirection = new GameObject[4];
        
        //This blanks the choices in from the selection to ignore unavailable
        for (int i = 0; i < sectionDisplay.Length; i++)
        {
            sectionDisplay[i].color = colourDisplay[0];
            pathText[i].SetText("N/A");
        }
        
        choosingPathUI.SetActive(true);
        
        //the selectedPathway is turn to empty to prevent players from deciding a direction that's not meant to be chosen
        selectedPathway = null;

        //this boolean turns false to prevent the player from instantly changing states
        hasSelected = false;

        //the controller and controls are reference in the player object
        controller = GetComponent<playerController>();
        controls = GetComponent<boardControls>();
        effects = GetComponent<currentEffects>();

        //The multi path calls the method to check if any paths are restricted due to the current direction type
        pathSelection.PathSelections(currentDirection);

        //this creates all the available paths for the player to choose from
        pathList = pathSelection.PlayerChoices;
        pathDirectionInt = pathSelection.DirectionInts;

        //this checks if there is only 1 path or more
        //if there is only one path then the player is forced to choose that path
        if (pathList.Count < 2)
        {
            controller.Path = pathList[0];
            hasSelected = true;
        }

        //otherwise if the player is blind then choose a random path
        else if (effects.Blind)
        {
            int randomPath = UnityEngine.Random.Range(0, pathList.Count);
            controller.Path = pathList[randomPath];
            hasSelected = true;
        }
        
        //otherwise the controls are enable depending on the type of direction the path is
        else
        {
            //if the directionInt is 0 then upPressed in controls is enabled
            //else if the directionInt is 2 then downPressed in controls is enabled
            //else if the directionInt is 3 then leftPressed in controls is enabled
            //else if the directionInt is 1 then rightPressed in controls is enabled

            for (int i = 0; i < pathList.Count; i++) 
            {
                {
                    pathOrder path = pathList[i].GetComponent<pathOrder>();


                    if (pathDirectionInt[i] == 0)
                    {
                        pathDirection[0] = pathList[i];
                        Controls.upPressed += DecidingUp;
                        Controls.upPressed += ChoosingSound;
                        pathText[0].SetText("Up");
                        sectionDisplay[0].color = colourDisplay[1];

                    }
                    else if (pathDirectionInt[i] == 2)
                    {
                        pathDirection[2] = pathList[i];
                        Controls.downPressed += DecidingDown;
                        Controls.downPressed += ChoosingSound;
                        pathText[2].SetText("Down");
                        sectionDisplay[2].color = colourDisplay[1];
                    }
                    else if (pathDirectionInt[i] == 3)
                    {
                        pathDirection[3] = pathList[i];
                        Controls.leftPressed += DecidingLeft;
                        Controls.leftPressed += ChoosingSound;
                        pathText[3].SetText("Left");
                        sectionDisplay[3].color = colourDisplay[1];
                    }
                    else if (pathDirectionInt[i] == 1)
                    {
                        pathDirection[1] = pathList[i];
                        Controls.rightPressed += DecidingRight;
                        Controls.rightPressed += ChoosingSound;
                        pathText[1].SetText("Right");
                        sectionDisplay[1].color = colourDisplay[1];
                    }

                    else
                    {
                        Debug.LogError(pathList[i] + " has a int of " + pathDirectionInt[i] + " which is not suitable for the path struct. You'll need to change the int between 0 to 3");
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
        choosingPathUI.SetActive(false);
        Controls.upPressed -= DecidingUp;
        Controls.downPressed -= DecidingDown;
        Controls.leftPressed -= DecidingLeft;
        Controls.rightPressed -= DecidingRight;
        Controls.confirmPressed -= ConfirmingChoice;
        Controls.upPressed -= ChoosingSound;
        Controls.downPressed -= ChoosingSound;
        Controls.leftPressed -= ChoosingSound;
        Controls.rightPressed -= ChoosingSound;
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
        eventText.SetText("Up");
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        selectedPathway = pathDirection[2];
        eventText.SetText("Down");
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        selectedPathway = pathDirection[3];
        eventText.SetText("Left");
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        selectedPathway = pathDirection[1];
        eventText.SetText("Right");
    }

    //When confirming the interface method must check if there is a path for the player to use
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //if there is a pathway, then the controller's path becomes the selected path
        if (selectedPathway != null)
        {
            controller.Path = selectedPathway;
            soundManager.PlaySound(confirmSound);
            hasSelected = true;
        }
        //else this infroms the player to choose another path or hasn't chosen one
        else
        {
            soundManager.PlaySound(declineSound);
            eventText.SetText("You either haven't chosen a path or chosen an empty path, select another path");
        }
    }

    public void ChoosingSound(object sender, EventArgs e)
    {
        soundManager.PlaySound(pathSound);
    }

}
