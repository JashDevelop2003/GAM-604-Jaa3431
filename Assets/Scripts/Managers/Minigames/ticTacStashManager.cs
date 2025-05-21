using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
/// <summary>
/// This is the manager for the minigame tic tac stash
/// In this minigame the manager will spin 9 blocks as structs and change their icon by using a enum to identify the icon
/// A win line struct is used to identify 3 blocks from a specific line are identical
/// If they are then the player will win cash depending on the icon:
/// Cherries = 25 cash
/// Lemons = 50 cash
/// Grapes = 100 cash
/// Watermelons = 250 cash
/// Sevens = 777 cash
/// </summary>

//The block struct identify the enum to identify if the block is the same as the other blocks in the win line struct (the struct below this struct)
[System.Serializable]
public struct block
{
    public iconEnum icon;
    public Image iconImage;
    public Image blockPanel;
    public bool isLocked;
}

//The win line struct provides lines that will identify the win if all 3 blocks are the same icon enumeration
[System.Serializable]
public struct winLine
{
    public int[] winBlocks;
    public GameObject line;
}

public class ticTacStashManager : Singleton<ticTacStashManager>
{
    public event EventHandler endEvent;
    
    //The manger provides 9 blocks due to the structure being tic tac toe
    [SerializeField] private block[] blocks = new block[9];
    public block[] Blocks
    {
        get { return blocks; } 
        set { blocks = value; }    
    }

    //The manager also provides 8 win lines since there are 3 horizontal wins, 3 vertical wins and 2 diagonal wins
    [SerializeField] private winLine[] winLines = new winLine[8];

    //The turn manager collects the player object to check if the state is no longer in the rules state
    private turnManager turnManager;
    [SerializeField] private GameObject player;
    
    //The manager provides the cash prize
    [SerializeField] private int cashPrize;
    private bool secondSpin;
    private bool spinProgress;

    //The minigame manager is used to add the observer method to end the game
    private minigameManager minigameManager;
    public bool SpinProgress
    {
        get { return spinProgress; }
    }

    [Header("User Interface")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Sprite[] iconImage = new Sprite[5];

    [Header("Sound Effects")]
    [SerializeField] private AudioClip appearSound;
    [SerializeField] private AudioClip[] outcomeSound = new AudioClip[2];
    private soundManager soundManager;
    private musicManager musicManager;


    // Start is called before the first frame update
    public void BeginMinigame()
    {
        soundManager = Singleton<soundManager>.Instance;
        turnManager = Singleton<turnManager>.Instance;
        minigameManager = Singleton<minigameManager>.Instance;
        musicManager = Singleton<musicManager>.Instance;
        endEvent += minigameManager.EndMinigame;
        endEvent += musicManager.MinigameOver;
        player = turnManager.CurrentPlayer;
        secondSpin = false;
        cashPrize = 0;
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].isLocked = false;
        }
        for( int i = 0;i < winLines.Length; i++)
        {
            winLines[i].line.SetActive(false);
        }
        StartCoroutine(BeginGame());
    }

    //The coroutine is started when the scene begins
    //The coroutine waits until the player has stop looking at the rules
    IEnumerator BeginGame()
    {
        ruleState ruleState = player.GetComponentInChildren<ruleState>();
        yield return new WaitUntil(() => ruleState.IsReady == true);
        StartCoroutine(Spin());
    }

    //The blocks will change their icon and apply the suitable image to inform player the icon that appear on each block
    public IEnumerator Spin()
    {
        spinProgress = true;
        int outcome;
        for (int i = 0; i < blocks.Length; i++) 
        {
            //During the second spin, the code will need to check if the player has locked that icon
            //if the player has then the icon stays as it is
            //Otherwise the icon will change their icon (or if unlucky stay the same)
            if (!blocks[i].isLocked)
            {
                blocks[i].icon = iconEnum.Null;
                yield return new WaitForSeconds(0.5f);
                outcome = UnityEngine.Random.Range(0, (int)iconEnum.Null);
                if (outcome == (int)iconEnum.Cherries)
                {
                    blocks[i].icon = iconEnum.Cherries;
                }
                else if (outcome == (int)iconEnum.Lemon)
                {
                    blocks[i].icon = iconEnum.Lemon;
                }
                else if (outcome == (int)iconEnum.Grapes)
                {
                    blocks[i].icon = iconEnum.Grapes;
                }
                else if (outcome == (int)iconEnum.Watermelon)
                {
                    blocks[i].icon = iconEnum.Watermelon;
                }
                else if (outcome == (int)iconEnum.Seven)
                {
                    blocks[i].icon = iconEnum.Seven;
                }
                blocks[i].iconImage.sprite = iconImage[outcome];
                soundManager.PlaySound(appearSound);
            }
        }

        //this checks if the coroutine is called for the second time by checking if the boolean is true
        //if the bool is false the boolean is set to true
        //otherwise the game calculates the amount of prize cash
        if (secondSpin)
        {
            StartCoroutine(Results());
        }
        else
        {
            secondSpin = true;
        }
        spinProgress = false;
    } 

    //The coroutine stars once the blocks are spun twice
    //This uses the win line struct to identify if all the blocks reference are identical
    //if they are then the line will need to increase the cash the player has won
    IEnumerator Results()
    {
        for (int i = 0; i < winLines.Length; i++) 
        {
            if (blocks[winLines[i].winBlocks[0]].icon == blocks[winLines[i].winBlocks[1]].icon && blocks[winLines[i].winBlocks[0]].icon == blocks[winLines[i].winBlocks[2]].icon)
            {
                winLines[i].line.SetActive(true);

                if (blocks[winLines[i].winBlocks[0]].icon == iconEnum.Cherries) 
                {
                    cashPrize += 25;
                }
                else if (blocks[winLines[i].winBlocks[0]].icon == iconEnum.Lemon)
                {
                    cashPrize += 50;
                }
                else if (blocks[winLines[i].winBlocks[0]].icon == iconEnum.Grapes)
                {
                    cashPrize += 100;
                }
                else if (blocks[winLines[i].winBlocks[0]].icon == iconEnum.Watermelon)
                {
                    cashPrize += 250;
                }
                else if (blocks[winLines[i].winBlocks[0]].icon == iconEnum.Seven)
                {
                    cashPrize += 777;
                }
                yield return new WaitForSeconds(1);
            }
        }

        if (cashPrize > 0) 
        {
            soundManager.PlaySound(outcomeSound[1]);
        }
        else
        {
            soundManager.PlaySound(outcomeSound[0]);
        }
        infoText.SetText("Game Over, the player has won: " +  cashPrize.ToString() + " cash");
        playerController player = turnManager.CurrentPlayer.GetComponent<playerController>();
        player.ChangeCash(cashPrize);
        yield return new WaitForSeconds(3);
        endEvent?.Invoke(this, EventArgs.Empty);
        endEvent -= minigameManager.EndMinigame;
        endEvent -= musicManager.MinigameOver;
    }
}
