using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is the data of only offence cards
/// this should provide suitable stats for offence cards such as attack value
/// this should provide a suitable card name and potential effects (ToDo During Playtesting Stage)
/// </summary>

[CreateAssetMenu(fileName = "New Offence Card", menuName = "Create Card/Offence", order = 1)]
public class offenceCardStats : ScriptableObject
{
    public enum CharacterCard {Wielder, Reaper, Gambler, Superstar, All}

    public CharacterCard characterCard;
    public CardType cardType = CardType.Offence;
    public CardRarity cardRarity;

    public string cardName;
    public string cardDescription;
    public int attackValue;
    public int manaCost; 
}
