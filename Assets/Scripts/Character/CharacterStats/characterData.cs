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
    public enum Character {Wielder, Reaper, Gambler, Superstar};

    public Character character;
    public int Health;
    public int Mana;
    public int startingCash;

    public int[] deckCapacity = new int [5];
    //Deck Capacity Integers:
    // - Offence is 0
    // - Defence is 1
    // - Movement is 2
    // - Status is 3
    // - Curse is 4

    public List<offenceCardStats> startingOffenceCards;
    public List<defenceCardStats> startingDefenceCards;
    public List<movementCardStats> startingMovementCards;
}
