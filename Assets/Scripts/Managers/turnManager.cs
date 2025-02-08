using System;
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
    [SerializeField] private GameObject[] Players;
    public EventHandler playerTurn; 
    
    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        RandomiseBeginner(Random.Range(0, Players.Length));

    }

    void RandomiseBeginner(int startingPlayer)
    {
        StartTurn(startingPlayer);
    }


    public void StartTurn(int currentPlayerTurn)
    {
        
    }
}
