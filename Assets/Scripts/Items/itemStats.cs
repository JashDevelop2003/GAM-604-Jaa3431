using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object is for only items
/// The script is use to store certain relics and omens onto all or specific players
/// The procedure is almost similar to the cards except there are no certain values and only applies the values from the game object item effects
/// </summary>

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item", order = 1)]
public class itemStats : ScriptableObject
{
    public enum CharacterItem { Wielder, Reaper, Gambler, Superstar, All }

    public CharacterItem character;
    public itemEnum itemType;

    public string itemName;
    public string itemDescription;

    public GameObject itemEffects;
    
}
