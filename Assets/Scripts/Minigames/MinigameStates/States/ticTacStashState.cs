using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This is the state that is only used for the minigame tic tac stash
/// the state waits until the first spin is done, which the player can start making choices
/// The player can select a certain block and then either lock or unlock the block to prevent or allow to spin again on the second spin
/// The player can press W and then confirm to spin again which the player doesn't need to do anything else from here.
/// </summary>

public class ticTacStashState : gameStateBase, IDecideLeft, IDecideRight, IDecideUp, IConfirm, IRules
{
    //The game controls are needed to provide input for the player
    private gameControls gameControls;
    public gameControls GameControls
    {
        get { return gameControls; }
        set { gameControls = value; }
    }

    //the game manager for this game needs to be reference to obtain the blocks and checking if the spin progress boolean is complete
    private ticTacStashManager gameManager;

    //this is the boolean to change the minigame state to the rules state
    private bool checkingRules;

    //this is the boolean that pretty much ends the player's turn and waits for the results
    private bool beginSpin;

    //This boolean ends the minigame and set the player's game state back to inactive
    private bool endGame;

    //this int is to identify the current block the player is on towards them to either lock or unlock
    private int selectedBlock;

    [Header ("User Interface")]
    [SerializeField] private Color[] blockColour = new Color[3];
    [SerializeField] private Color[] spinColour = new Color[2];
    [SerializeField] private Image spinPanel;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip inputSound;
    [SerializeField] private AudioClip spinSound;
    [SerializeField] private AudioClip[] lockSound = new AudioClip[2];
    private soundManager soundManager;

    public override void EnterState(gameStateManager player)
    {
        //The booleans must be false to ensure that the player doesn't acidentally start spinning when they want to make a choice or kept on the rules state
        checkingRules = false;
        beginSpin = false;
        endGame = false;

        selectedBlock = 0;

        //The tic tac stash manager needs be to be reference to gather the struct and boolean variable by using a singleton instance
        gameManager = Singleton<ticTacStashManager>.Instance;
        gameControls = GetComponent<gameControls>();

        gameManager.endEvent += EndGame;

        spinPanel.color = spinColour[0];

        //The sound manager is reference to provide the input sound
        soundManager = Singleton<soundManager>.Instance;

        //At the start of this state, a Coroutine starts to wait until the first spin is complete.
        StartCoroutine(WaitSpin());
    }

    public override void UpdateState(gameStateManager player)
    {
        //When the checking rules boolean is true then the state will change back to the rules state
        if (checkingRules) 
        { 
            player.ChangeState(player.RuleState);
        }

        if (endGame) 
        { 
            player.ChangeState(player.InactiveState);
        }
    }

    //When exiting the state, all controls must be disabled since entering the state will always enable the controls.
    public override void ExitState(gameStateManager player) 
    {
        GameControls.pressedLeft -= DecidingLeft;
        GameControls.pressedRight -= DecidingRight;
        GameControls.pressedUp -= DecidingUp;
        GameControls.pressedConfirm -= ConfirmingChoice;
        GameControls.pressedRules -= Rules;
        GameControls.pressedLeft -= PlaySound;
        GameControls.pressedRight -= PlaySound;
        GameControls.pressedUp -= PlaySound;
        gameManager.endEvent -= EndGame;
    }

    //When pressing backspace, the player heads back to the rule state
    public void Rules(object sender, EventArgs e)
    {
        checkingRules = true;
    }

    //When pressing A, the player will go to the previous block, if the current block is the first block then they'll go to the last block
    //Begin Spin boolean must be false to allow the player to lock and unlock blocks
    public void DecidingLeft(object sender, EventArgs e)
    {
        beginSpin = false;
        spinPanel.color = spinColour[0];
        selectedBlock--;
        if(selectedBlock < 0)
        {
            selectedBlock = gameManager.Blocks.Length - 1;
        }
        HighlightBlock();
    }

    //When pressing D, the player will go to the next block, if the current block is the last block then they'll go to the first block
    //Begin Spin boolean must be false to allow the player to lock and unlock blocks
    public void DecidingRight(object sender, EventArgs e)
    {
        beginSpin = false;
        spinPanel.color = spinColour[0];
        selectedBlock++;
        if (selectedBlock >= gameManager.Blocks.Length) 
        {
            selectedBlock = 0;
        }
        HighlightBlock();
    }
    
    //When pressing W, the player will be allow to spin, if they're next input is confirm
    public void DecidingUp(object sender, EventArgs e)
    {
        beginSpin = true;
        spinPanel.color = spinColour[1];
    }

    //When pressing spacebar, the input method will need to check if the player wants to spin or lock/unlock a block
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //If the player wants to spin then all the controls are disabled since the player doesn't need to do anything else
        if (beginSpin)
        {
            selectedBlock = 9;
            HighlightBlock();
            GameControls.pressedLeft -= DecidingLeft;
            GameControls.pressedRight -= DecidingRight;
            GameControls.pressedUp -= DecidingUp;
            GameControls.pressedLeft -= PlaySound;
            GameControls.pressedRight -= PlaySound;
            GameControls.pressedUp -= PlaySound;
            GameControls.pressedConfirm -= ConfirmingChoice;
            GameControls.pressedRules -= Rules;
            StartCoroutine(gameManager.Spin());
            soundManager.PlaySound(spinSound);
        }

        //otherwise the block is either locked or unlocked depending if the block's boolean is true (unlock) or false (locked)
        else
        {
            if (gameManager.Blocks[selectedBlock].isLocked)
            {
                gameManager.Blocks[selectedBlock].isLocked = false;
                soundManager.PlaySound(lockSound[1]);
            }
            else
            {
                gameManager.Blocks[selectedBlock].isLocked = true;
                soundManager.PlaySound(lockSound[0]);
            }
        }
    }

    //This method occurs when either pressing A or D
    //The method highlights which block the player is currently on and whether other blocks are locked or unlocked
    void HighlightBlock()
    {
        for (int i = 0; i < gameManager.Blocks.Length; i++) 
        {
            if(i == selectedBlock)
            {
                gameManager.Blocks[i].blockPanel.color = blockColour[1];
            }
            else
            {
                if (gameManager.Blocks[i].isLocked)
                {
                    gameManager.Blocks[i].blockPanel.color = blockColour[2];
                }
                else
                {
                    gameManager.Blocks[i].blockPanel.color = blockColour[0];
                }
            }           
        }
    }

    //This Coroutine prevents the player from inputting any of their controls until the all the blocks are spun for the first time
    private IEnumerator WaitSpin()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => gameManager.SpinProgress == false);
        GameControls.pressedLeft += DecidingLeft;
        GameControls.pressedRight += DecidingRight;
        GameControls.pressedUp += DecidingUp;
        GameControls.pressedConfirm += ConfirmingChoice;
        GameControls.pressedRules += Rules;

        GameControls.pressedLeft += PlaySound;
        GameControls.pressedRight += PlaySound;
        GameControls.pressedUp += PlaySound;
        HighlightBlock();
    }

    public void EndGame(object sender, EventArgs e)
    {
        endGame = true;
    }

    //The observer method is to play sound for every input performed
    public void PlaySound(object sender, EventArgs e)
    {
        soundManager.PlaySound(inputSound);
    }


}
