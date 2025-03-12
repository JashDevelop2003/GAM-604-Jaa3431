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
    //This is for both buff and effect cards
    public enum CharacterCard {Wielder, Reaper, Gambler, Superstar, All}
    public CharacterCard characterCard;
    public CardType cardType = CardType.Status;
    public CardRarity cardRarity;
    
    //This is for identifying for the target state to provide the suitable behaviour
    public targetEnum target;

    // effect and buff Enum are in an array since there can be more than 1 effect or buff from one card
    public effectEnum[] effect;
    public buffEnum[] buff;
    
    // 0 = Effect Cooldown
    // 1 = Buff Cooldown
    public int[] cooldown = new int[2];
    
    //Value is ONLY for Buffs and all buffs must be the same value
    public float value;

    public string cardName;
    public string cardDescription;
    public int manaCost;

    //This is used in case the player needs additional effects such as healing, increasing a value for only that turn
    public GameObject additionalEffect;
}
