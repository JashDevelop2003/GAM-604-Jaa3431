using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This is using a generic singleton which makes this the only manager running in the board map
/// This will organise the turn of the player and checks if the player is defeated
/// If the player is defeated then it must ignore the player's turn and move to the next player
/// </summary>

public class turnManager : Singleton<turnManager>
{
    private dataManager dataManager;

    //this provides an array to collect all of the players
    [SerializeField] private GameObject[] Players;
    public GameObject[] GetPlayers
    {
        get { return Players; }
    }

    [SerializeField] private GameObject currentPlayer;
    public GameObject CurrentPlayer
    {
        get { return currentPlayer; }
    }

    [SerializeField] private int currentPlayerTurn;
    public int CurrentPlayerTurn
    {
        get { return currentPlayerTurn; }
        set { currentPlayerTurn = value; }
    }
    
    // Start is called before the first frame update
    void Start()

    {
        dataManager = Singleton<dataManager>.Instance;

        //This gathers all of the players since each player object provides a tag called "Player"
        Players = GameObject.FindGameObjectsWithTag("Player");

        //To start the game the manager will need to see who starts first
        //Arrays always start with [0] and since the max is excluded this only provide a value of 0 or max player
        //Example: If there's 3 players then the randomiser will pick an integer between 0 to 2 with 0 representing the first player and 2 representing the third player
        currentPlayerTurn = Random.Range(0, Players.Length);

        StartCoroutine(WaitForLoad());

    }

    IEnumerator WaitForLoad()
    {
        yield return new WaitUntil(() => dataManager.LoadComplete == true);
        StartTurn();
    }

    //this provides starting the next player's turn on the array
    public void StartTurn()
    {
        //This checks if the integer to call the player in the array has extended equal to or more than the number of players
        if(currentPlayerTurn >= Players.Length) 
        {
            //If it does then go back to player one (which should be 0)
            currentPlayerTurn = 0;
        }

        //This provides the current player game object
        currentPlayer = Players[currentPlayerTurn];

        //This finds the state manager in the current player's turn to infrom the state manager to move to start state
        Players[currentPlayerTurn].GetComponent<playerStateManager>().StartPlayerTurn();

        //This increments to the next player; when the current player's turn ends it moves onto the next player
        currentPlayerTurn++;
    }
}
