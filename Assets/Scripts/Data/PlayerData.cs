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
    public float[] buffValue = new float[6];

    public List<int> offenceCards;
    public List<int> defenceCards;
    public List<int> movementCards;
    public List<int> statusCards;
    public List<int> relics;
    public List<int> omens;


}
