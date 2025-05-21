using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigameManager : Singleton<minigameManager>
{
    private minigameEnum minigame;
    private bool gameInProgress;
    public bool GameInProgress
    {
        get { return gameInProgress; }
    }

    //The turn manager will be used to identify the current player if the minigame is a single player
    private turnManager turnManager;

    //These are minigame managers that will need to store the event to end the minigame
    private doubleOrNothingManager doubleOrNothingManager;
    private xIIIManager xIIIManager;
    private ticTacStashManager ticTacStashManager;

    [Header("Mingame Interface")]
    [SerializeField] private GameObject[] gameplayUI = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        gameInProgress = false;
        turnManager = Singleton<turnManager>.Instance;
        doubleOrNothingManager = Singleton<doubleOrNothingManager>.Instance;
        xIIIManager = Singleton<xIIIManager>.Instance;
        ticTacStashManager = Singleton<ticTacStashManager>.Instance;
    }

    //When the player lands on a minigame space, this method will be called
    //The integer parameter provides a random range between 0 to the minigame enum of null (max is excluded in ints)
    //Depending on the minigame the method will reference the player's game state manager to change the minigame state to the minigame chosen
    public void Minigame(int minigame)
    {
        if (minigame == (int)minigameEnum.DoubleOrNothing)
        {
            gameStateManager gameStateManager = turnManager.CurrentPlayer.GetComponentInChildren<gameStateManager>();
            gameStateManager.MinigameState = turnManager.CurrentPlayer.GetComponent<doubleOrNothingState>();
            gameStateManager.StartMinigame(minigame);
            doubleOrNothingManager.BeginMinigame();
        }
        else if (minigame == (int)minigameEnum.XIII)
        {
            for (int i = 0; i < turnManager.GetPlayers.Length; i++) 
            {
                gameStateManager gameStateManager = turnManager.GetPlayers[i].GetComponentInChildren<gameStateManager>();
                gameStateManager.MinigameState = turnManager.GetPlayers[i].GetComponent<xIIIInactiveState>();
                gameStateManager.StartMinigame(minigame);
                xIIIManager.BeginMinigame();
            }
        }
        else if(minigame == (int)minigameEnum.TicTacStash)
        {
            gameStateManager gameStateManager = turnManager.CurrentPlayer.GetComponentInChildren<gameStateManager>();
            gameStateManager.MinigameState = turnManager.CurrentPlayer.GetComponent<ticTacStashState>();
            gameStateManager.StartMinigame(minigame);
            ticTacStashManager.BeginMinigame();
        }

        gameplayUI[minigame].SetActive(true);
    }

    //Once the game is complete, the manager will disable all of the minigame UI that are currently active
    public void EndMinigame(object sender, EventArgs e)
    {
        gameInProgress = false;
        for (int i = 0; i < gameplayUI.Length; i++) 
        {
            gameplayUI[i].SetActive(false);
        }
    }
}
