using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public struct Paths
{
    public directionEnum directionRestriction;
    public GameObject pathChoice;
}
public class pathSelection : MonoBehaviour
{
    [SerializeField] private List<Paths> pathChoices;
    private List<GameObject> playerChoices = new List<GameObject>();
    public List<GameObject> PlayerChoices
    {
        get { return playerChoices; } 
    
    }

    //This checks if the player direction
    public void PathSelections(directionEnum playerDirection)
    {
        playerChoices.Clear();
        
        foreach (Paths pathChoice in pathChoices) 
        { 
            if (pathChoice.directionRestriction != playerDirection)
            {
                playerChoices.Add(pathChoice.pathChoice);
            }
        }
    }
}
