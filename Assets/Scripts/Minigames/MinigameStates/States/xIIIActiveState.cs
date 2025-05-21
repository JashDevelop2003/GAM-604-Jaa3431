using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// The active state of the XIII is when the player can choose cards or choose to skip
/// The player can navigate the card they want to select with A & D to move to the next or previous card
/// The player can also press S if they want to select to skip their turn
/// The player then finally chooses to confirm to confirm their choice which will then identify the card struct selected
/// If the player finds a fruit they will gain cash depeending on the fruit
/// otherwise the XIII manager will start a coroutine to make it a game over & the prize cash will be set to 0
/// </summary>

public class xIIIActiveState : gameStateBase, IDecideDown, IDecideLeft, IDecideRight, IConfirm, IRules
{
    //This collects the controls for the player to perform the inputs
    private gameControls controls;
    public gameControls GameControls
    {
        get { return controls; } 
        set { controls = value; }
    }

    //these booleans will provide a change on the state
    //End turn will change the state to the inactive state for this game
    //check rules will change the state to the rules state
    private bool endTurn;
    private bool checkRules;
    private bool endGame;

    //These booleans check if the player is planning to skip and if the has used their skip
    private bool usedSkip = false;
    private bool isSkipping;

    //The XIII manager is needed towards changing the states by using an event along with calling to skip the next turn and revealing the card
    private xIIIManager xIIIManager;

    //Prize cash is use to store the amount of cash the player can possibly win
    private int prizeCash;


    //The selected card is used to identify which stuct card in the array of cards the player is currently on and which one to reveal when confirming
    private int selectedCard;

    //This is to inform the parameter of the player is currently playing
    [SerializeField] private int playerInt;

    [Header("User Interface")]
    [SerializeField] private Color[] cardColors = new Color[2];
    [SerializeField] private GameObject skipPanel;
    [SerializeField] private Image skipImage;
    [SerializeField] private TMP_Text infoText;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip inputSound;
    [SerializeField] private AudioClip skipSound;
    soundManager soundManager;


    public override void EnterState(gameStateManager player)
    {
        //The booleans that cause the change in the state must be false to prevent the player from changing instantly
        endTurn = false;
        endGame = false;
        isSkipping = false;
        checkRules = false;

        //The controls are collect with a method to be added onto the specific event to be listening to when a controls input has been invoked.
        controls = GetComponent<gameControls>();
        GameControls.pressedLeft += DecidingLeft;
        GameControls.pressedRight += DecidingRight;
        GameControls.pressedDown += DecidingDown;
        GameControls.pressedConfirm += ConfirmingChoice;
        GameControls.pressedRules += Rules;

        //The XIII manager is collected from the singleton instance to ensure that there is only 1 XIII manager that is being used for referencing and collecting methods
        xIIIManager = Singleton<xIIIManager>.Instance;
        xIIIManager.changeTurn += EndTurn;

        //The sound manager is collected from the singleton instance to play sound towards input
        soundManager = Singleton<soundManager>.Instance;
        GameControls.pressedLeft += PlaySound;
        GameControls.pressedRight += PlaySound;
        GameControls.pressedDown += PlaySound;

        //The while loop checks if the lowest card integer that isn't revealed yet
        selectedCard = 0;
        while (xIIIManager.Cards[selectedCard].isRevealed) 
        { 
            selectedCard++;
        }
        for (int i = 0; i < xIIIManager.Cards.Length; i++)
        {
            if (i == selectedCard)
            {
                xIIIManager.Cards[i].backCardColour.color = cardColors[1];
            }
            else
            {
                xIIIManager.Cards[i].backCardColour.color = cardColors[0];
            }
        }

        //This checks if the player can skip, if they can then the skip panel will appear, otherwise the panel won't appear
        if (!usedSkip)
        {
            skipPanel.SetActive(true);
            skipImage.color = cardColors[0];
        }
        else
        {
            skipPanel.SetActive(false);
        }

        //This displays the info on who turn it is with setting the text
        infoText.SetText("Player " + playerInt.ToString() + "'s turn");
    }

    //Update state will update to check if any of the booleans are true which will change to the specifc state
    public override void UpdateState(gameStateManager player)
    {
        if (endTurn) 
        {
            player.MinigameState = GetComponent<xIIIInactiveState>();
            player.ChangeState(player.MinigameState);
        }
        
        if (checkRules) 
        { 
            player.ChangeState(player.RuleState);
        }

        if (endGame)
        {
            player.ChangeState(player.InactiveState);
        }
    }

    //When exiting the state, the script will need to disable all of the events to prevent overlapping the other player's turn
    public override void ExitState(gameStateManager player)
    {
        xIIIManager.changeTurn -= EndTurn;
        GameControls.pressedLeft -= DecidingLeft;
        GameControls.pressedRight -= DecidingRight;
        GameControls.pressedDown -= DecidingDown;
        GameControls.pressedConfirm -= ConfirmingChoice;
        GameControls.pressedRules -= Rules;
        GameControls.pressedLeft -= PlaySound;
        GameControls.pressedRight -= PlaySound;
        GameControls.pressedDown -= PlaySound;
    }

    //Pressing Backspace will return the player back to the rules
    public void Rules(object sender, EventArgs e)
    {
        checkRules = true;
    }

    //Pressing A (left) will move navigate to the previous card that hasnt been revealed
    public void DecidingLeft(object sender, EventArgs e)
    {
        //The 2 lines of code prevent the player from skipping their turn
        isSkipping = false;
        skipImage.color = cardColors[0];

        //A Do While loop is use to decrement the integer until the card struct array based on the selected card int's isRevealed boolean is set to false
        do
        {
            selectedCard--;
            //A If conditional statement is used to check if the selected card is below the lowest array value which will set the intger to the highest array value (12 since there are 13 cards)
            if( selectedCard < 0)
            {
                selectedCard = xIIIManager.Cards.Length - 1;
            }
        }
        while (xIIIManager.Cards[selectedCard].isRevealed);

        //A for loop is used to identify the selected card to indicate that's the card the player will be revealing when confirmed.
        for (int i = 0; i < xIIIManager.Cards.Length; i++) 
        {
            if (i == selectedCard)
            {
                xIIIManager.Cards[i].backCardColour.color = cardColors[1];
            }
            else 
            {
                xIIIManager.Cards[i].backCardColour.color = cardColors[0];
            }
        }
    }

    //Pressing D (Right) does the same process as pressing Left instead of decrementing the selected card integer, it will increment instead
    public void DecidingRight(object sender, EventArgs e)
    {
        isSkipping = false;
        skipImage.color = cardColors[0];

        //A Do While loop is use to increment the integer until the card struct array based on the selected card int's isRevealed boolean is set to false
        do
        {
            selectedCard++;
            //A if conditional statement is used to check if the intger went over the length of the card struct array, which will set the integer back to 0 (lowest array value)
            if (selectedCard >= xIIIManager.Cards.Length)
            {
                selectedCard = 0;
            }
        }
        while (xIIIManager.Cards[selectedCard].isRevealed);
        for (int i = 0; i < xIIIManager.Cards.Length; i++)
        {
            if (i == selectedCard)
            {
                xIIIManager.Cards[i].backCardColour.color = cardColors[1];
            }
            else
            {
                xIIIManager.Cards[i].backCardColour.color = cardColors[0];
            }
        }
    }

    //When pressing S (Down), the player can skip if they deicde to confirm whilst is skipping is true
    //However they can only do so if they haven't used their skip yet
    public void DecidingDown(object sender, EventArgs e)
    {
        if (!usedSkip)
        {
            isSkipping = true;
            skipImage.color = cardColors[1];
        }
        else
        {
            UnityEngine.Debug.LogWarning("You already used your skip");
        }
    }

    //This confirms the player choice when pressing spacebar
    //If the boolean of isSkipping is true then the player skips their turn
    // Otherwise the card is reveal to identify the outcome of the card.
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        if (isSkipping)
        {
            usedSkip = true;
            soundManager.PlaySound(skipSound);
            xIIIManager.ChangeTurn();
        }
        else 
        { 
            xIIIManager.RevealCard(selectedCard, playerInt);
        }
    }

    //This observer method is added onto the change turn subject from the XIII manager
    //This will set the end turn boolean to true, which will trigger the update state to change the state to inactive state
    public void EndTurn(object sender, EventArgs e)
    {
        endTurn = true;
    }

    public void EndGame(object sender, EventArgs e)
    {
        endGame = true;
    }

    //This observer method is added onto each input (excluding confirm) which plays every time the player presses left, right or down
    public void PlaySound(object sender, EventArgs e)
    {
        soundManager.PlaySound(inputSound);
    }
}

