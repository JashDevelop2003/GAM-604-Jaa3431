using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is the data of the character
/// This should provide all the infromation required for the model to reference and use
/// This should also provide the capcity on how many cards to create for each deck
/// </summary> 

//This provides a menu to create a new chracter data object
[CreateAssetMenu(fileName = "New Character", menuName = "Create Character")]
public class characterData : ScriptableObject
{

    public characterEnum character;
    public int Health;
    public int Mana;
    public int startingCash;

    public int[] deckCapacity = new int [5];
    //Deck Capacity Integers:
    // - Offence is 0
    // - Defence is 1
    // - Movement is 2
    // - Status is 3
    // - Item is 4

    //These are the cards the player should start off with
    public List<offenceCardStats> startingOffenceCards;
    public List<defenceCardStats> startingDefenceCards;
    public List<movementCardStats> startingMovementCards;
    public List<statusCardStats> startingStatusCards;

    //These are the common cards that will appear when obtaining a uncommon card
    public List<offenceCardStats> possibleOffenceCards;
    public List<defenceCardStats> possibleDefenceCards;
    public List<movementCardStats> possibleMovementCards;
    public List <statusCardStats> possibleStatusCards;

    //This is the game object for the character in order to use passive and one use ability
    public GameObject characterObject;
}
