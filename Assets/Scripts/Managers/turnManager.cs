using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This is a generic singleton which makes this the only manager running in the board mao
/// This will organise the turn of the player and checks if the player is defeated
/// If the player is defeated then it must ignore the player's turn and move to the next player
/// </summary>

public class turnManager : Singleton<turnManager>
{
    //this provides an array to collect all of the players
    [SerializeField] private GameObject[] Players;
    [SerializeField] int currentPlayerTurn;
    
    // Start is called before the first frame update
    void Start()

    {
        //This gathers all of the players since each player object provides a tag called "Player"
        Players = GameObject.FindGameObjectsWithTag("Player");

        //To start the game the manager will need to see who starts first
        //Arrays always start with [0] and since the max is excluded this only provide a value of 0 or max player
        //Example: If there's 3 players then the randomiser will pick an integer between 0 to 2 with 0 representing the first player and 2 representing the third player
        currentPlayerTurn = Random.Range(0, Players.Length);
        StartTurn();

    }

    //this provides the 
    public void StartTurn()
    {
        if(currentPlayerTurn >= Players.Length) 
        {
            currentPlayerTurn = 0;
        }

        Players[currentPlayerTurn].GetComponent<playerStateManager>().StartPlayerTurn();

        currentPlayerTurn++;
    }
}
