
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

    
    public playerModel(characterData CharacterData)
    {
        characterData = CharacterData;

        currentHealth = characterData.Health;
        maxHealth = characterData.Health;
        CurrentMana = characterData.Mana;
        MaxMana = characterData.Mana;
        currentCash = characterData.startingCash;
        rollValue = 0;
        thrustMultiplier = 1;
        guardMultiplier = 1;
    }
}
