using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is the data of only movement cards
/// this should provide suitable stats for movement cards such as move value
/// this should provide a suitable card name and potential effects (ToDo During Playtesting Stage)
/// </summary>

[CreateAssetMenu(fileName = "New Movement Card", menuName = "Create Card/Movement", order = 3)]
public class movementCardStats : ScriptableObject
{
    public enum CharacterCard { Wielder, Reaper, Gambler, Superstar, All }

    public CharacterCard characterCard;
    public CardType cardType = CardType.Movement;
    public CardRarity cardRarity;

    public string cardName;
    public string cardDescription;
    public int minimumMoveValue;
    public int maximumMoveValue;
    public int manaCost;
}
