using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// The path selection is used for the multi path objects which when landed on allows the player to choose a path
/// This will check which paths are available for the player to choose from and restrict some path depending on the player's current directions
/// </summary>

//A struct is used to create multiple path that provide their own direction restriction from the direction enum
//Along with the path that is available for the player to choose from
//A Int is used to provide a suitable key press for the player to choose the path they want (e.g the player doesn't have to press down to move right)
//Int must be between 0-3 in order to provide the suitable key, the ints for the suitable key are as followed:
// Up = 0
// Right = 1
// Down = 2
// Left = 3

[System.Serializable]
public struct Paths
{
    public directionEnum directionRestriction;
    public GameObject pathChoice;
    public int directionInt;
}
public class pathSelection : MonoBehaviour
{
    //this provides the list of struct paths to create inside of this class
    [SerializeField] private List<Paths> pathChoices;
    //this list is to provide the paths the player can choose which will be use for the player's choosing state
    private List<GameObject> playerChoices = new List<GameObject>();
    public List<GameObject> PlayerChoices
    {
        get { return playerChoices; } 
    
    }
    
    private List<int> directionInts = new List<int>();
    public List<int> DirectionInts
    {
        get { return directionInts; }
    }

    //This checks if the player direction is the same as the direction restriction which is excluded from the player's choice
    public void PathSelections(directionEnum playerDirection)
    {
        //this clears the player choices to prevent any incorrect paths to be selected
        playerChoices.Clear();
        directionInts.Clear();
        
        //this checks for each path's direction restriction to see if it matches the player's current direction
        //if both the player direction and direction restriction aren't the same then add the path to the list
        //otherwise restrict the path from being chosen by the player
        foreach (Paths pathChoice in pathChoices) 
        { 
            if (pathChoice.directionRestriction != playerDirection)
            {
                playerChoices.Add(pathChoice.pathChoice);
                directionInts.Add(pathChoice.directionInt);
            }
        }
    }
}
