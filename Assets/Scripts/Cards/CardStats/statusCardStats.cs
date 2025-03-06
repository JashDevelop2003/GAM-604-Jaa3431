using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// This script is the data of only status cards
/// this should provide suitable stats for status cards such as type of status effect
/// this should provide a suitable card name and effect
/// </summary>
/// 

[CreateAssetMenu(fileName = "New Status Card", menuName = "Create Card/Status", order = 4)]


public class statusCardStats : ScriptableObject
{
    public enum CharacterCard {Wielder, Reaper, Gambler, Superstar, All}
    public CharacterCard characterCard;
    public CardType cardType = CardType.Status;
    public CardRarity cardRarity;
    public targetEnum target;

    public effectEnum effect;
    public int cooldown;

    public string cardName;
    public string cardDescription;
    public GameObject effectPrefab;
    public int manaCost;
}
