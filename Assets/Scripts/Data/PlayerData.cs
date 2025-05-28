using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int character;

    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;
    public int currentCash;
    public bool abilityUsed;

    public int path;
    public int currentSpace;

    public int[] effectCooldown = new int[12];
    public int[] buffCooldown = new int[6];

    public int[] offenceCards;
    public int[] defenceCards;
    public int[] movementCards;
    public int[] statusCards;
    public int[] items;


}
