
using UnityEngine;

/// <summary>
/// This script is the model that handles the data from the MVP architecture
/// This should collect the character data and provide changes
/// </summary>
public class playerModel
{
    //This is the character data that should provide values for variables to change their values
    private characterData characterData;

    //Each variable is encapsulated, meaning that getters and setters are use to change the values
    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    private int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    private int currentMana;
    public int CurrentMana
    {
        get { return currentMana; }
        set { currentMana = value; }
    }

    private int maxMana;
    public int MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }

    private int currentCash;
    public int CurrentCash
    {
        get { return currentCash; }
        set { currentCash = value; }
    }

    private int rollValue;
    public int RollValue
    {
        get { return rollValue; }
        set { rollValue = value; }
    }

    private float thrustMultiplier;
    public float ThrustMultiplier
    {
        get { return thrustMultiplier; }
        set { thrustMultiplier = value; }
    }

    private float guardMultiplier;
    public float GuardMultiplier
    {
        get { return guardMultiplier; }
        set { guardMultiplier = value; }
    }

    private float rollMultiplier;
    public float RollMultiplier
    {
        get { return rollMultiplier; }
        set { rollMultiplier = value; }
    }

    private characterEnum character;
    public characterEnum Character
    {
        get { return character; }
    }

    private int offenceCards;
    public int OffenceCards
    {
        get { return offenceCards; }
        set {  offenceCards = value; }
    }

    private int defenceCards;
    public int DefenceCards
    {
        get { return defenceCards; }
        set { defenceCards = value; }
    }

    private int movementCards;
    public int MovementCards
    {
        get { return movementCards; }
        set {  movementCards = value; }
    }

    private int statusCards;
    public int StatusCards
    {
        get { return statusCards; }
        set { statusCards = value; }
    }

    private int itemPile;
    public int ItemPile
    {
        get { return itemPile; }
        set { itemPile = value; }
    }

    private bool isAlive;
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    //the player model will be created into the controller and provide the character data in the game
    public playerModel(characterData CharacterData)
    {
        characterData = CharacterData;

        currentHealth = characterData.Health;
        maxHealth = characterData.Health;
        CurrentMana = characterData.Mana;
        MaxMana = characterData.Mana;
        currentCash = characterData.startingCash;
        offenceCards = characterData.startingOffenceCards.Count;
        defenceCards = characterData.startingDefenceCards.Count;
        movementCards = characterData.startingMovementCards.Count;
        statusCards = characterData.startingStatusCards.Count;
        itemPile = characterData.startingItems.Count;
        rollValue = 0;
        thrustMultiplier = 1;
        guardMultiplier = 1;
        rollMultiplier = 1;
        character = characterData.character;
        isAlive = true;
    }
}
