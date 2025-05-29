using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Data Manager is a singleton manager that stores, retireves and changes data
/// The data that requires changing and storing are the player and the game data itself
/// </summary>

public class dataManager : Singleton<dataManager>
{
    //The manager reference the turn manager to identify the current player's turn
    private turnManager turnManager;
    
    //These public variables are use to storing and changing game data
    public bool progressGame;
    public int selectedBoard;
    public int currentTurn;

    //The events are used to call each player's data component
    public event EventHandler saveFiles;
    public event EventHandler loadFiles;

    private bool[] loadedPlayer = new bool [2];
    private bool loadComplete;

    public bool LoadComplete
    {
        get { return loadComplete; }
    }

    public bool[] LoadedPlayer
    {
        get { return loadedPlayer; }
        set { loadedPlayer = value; }
    }

    void Start()
    {
        turnManager = Singleton<turnManager>.Instance;


        //At the start of the game, the manager needs to check if there is an existing game data
        //If there is then store the existing data varaibles to the suitable public variables in this manager.
        //Also invoke the loaded files for each player data
        //Otherwise set suitable values to the game to be stored.
        GameData gameData = saveSystem.Load();
        if (gameData != null) 
        {
            currentTurn = gameData.playerTurn;
            selectedBoard = gameData.board;
            progressGame = gameData.midGame;
            loadFiles?.Invoke(this, EventArgs.Empty);
            StartCoroutine(LoadingPlayers());
        }
        else
        {
            currentTurn = UnityEngine.Random.Range(0, turnManager.GetPlayers.Length);
            selectedBoard = (int)sceneEnum.TestYourGame;
            progressGame = true;
        }

        turnManager.CurrentPlayerTurn = currentTurn;
    }

    public IEnumerator LoadingPlayers()
    {
        yield return new WaitUntil(() => loadedPlayer[0] == true && loadedPlayer[1] == true);
        loadComplete = true;
    }
    
    //This saves the game data by creating a new game data to replace the old data.
    public void SaveGame()
    {
        currentTurn = turnManager.CurrentPlayerTurn;

        GameData gameData = new GameData
        {
            midGame = progressGame,
            board = selectedBoard,
            playerTurn = currentTurn,
        };
        
        saveSystem.Save(gameData);
        saveFiles?.Invoke(this, EventArgs.Empty);

    }
}
