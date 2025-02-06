using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is the data of only defence cards
/// this should provide suitable stats for defence cards such as defend value
/// this should provide a suitable card name and potential effects (ToDo During Playtesting Stage)
/// </summary>

[CreateAssetMenu(fileName = "New Defence Card", menuName = "Create Card/Defence", order = 2)]
public class defenceCardStats : ScriptableObject
{
    public enum CharacterCard { Wielder, Reaper, Gambler, Superstar, All }

    public CharacterCard characterCard;
    public CardType cardType = CardType.Defence;
    public CardRarity cardRarity;

    public string cardName;
    public string cardDescription;
    public int defendValue;
    public int manaCost;
}
