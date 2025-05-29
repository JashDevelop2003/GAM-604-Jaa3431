using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTwoData : MonoBehaviour
{
    private dataManager dataManager;
    private Transform locatePlayer;
    private GameObject player;

    public int healthCurrent;
    public int healthMax;
    public int manaCurrent;
    public int manaMax;
    public int cashCurrent;
    public bool usedAbility;

    public int currentPath;
    public int spaceInt;

    public List<int> storedOffence;
    public List<int> storedDefence;
    public List<int> storedMovement;
    public List<int> storedStatus;
    public List<int> storedRelics;
    public List<int> storedOmens;

    public int[] storedEffects;
    public int[] storedBuffs;
    public float[] storedValues;

    private bool controllerComplete = false;
    private bool decksComplete = false;

    private List<effectEnum> effectEnums = new List<effectEnum>()
    {
        effectEnum.Burned,
        effectEnum.Shocked,
        effectEnum.Exposed,
        effectEnum.Bleeding,
        effectEnum.Poison,
        effectEnum.Blistered,
        effectEnum.Unstabled,
        effectEnum.Slowed,
        effectEnum.Confused,
        effectEnum.Feared,
        effectEnum.Stunned,
        effectEnum.Blind
    };

    private List<buffEnum> buffEnums = new List<buffEnum>()
    {
        buffEnum.Invincible,
        buffEnum.Healthy,
        buffEnum.Hasty,
        buffEnum.Lucky,
        buffEnum.Resistant,
        buffEnum.Impactful,
    };

    public bool ControllerComplete
    {
        get { return controllerComplete; }
        set { controllerComplete = value; }
    }

    public bool DecksComplete
    {
        get { return decksComplete; }
        set { decksComplete = value; }
    }

    private playerController controller;
    private currentEffects effects;
    private currentBuffs buffs;

    public event EventHandler saveData;

    void Awake()
    {
        locatePlayer = this.transform.parent;
        player = locatePlayer.gameObject;
        dataManager = Singleton<dataManager>.Instance;
        dataManager.loadFiles += LoadPlayer;
        dataManager.saveFiles += SavePlayer;

        controller = player.GetComponent<playerController>();
        effects = player.GetComponent<currentEffects>();
        buffs = player.GetComponent<currentBuffs>();

        saveData += controller.StoreStats;
        saveData += effects.SaveEffects;
        saveData += buffs.SaveBuffs;
    }

    public void LoadPlayer(object sender, EventArgs e)
    {
        PlayerData playerTwoData = saveSystem.LoadTwo();
        if (playerTwoData != null)
        {
            healthCurrent = playerTwoData.currentHealth;
            healthMax = playerTwoData.maxHealth;
            manaCurrent = playerTwoData.currentMana;
            manaMax = playerTwoData.maxMana;
            cashCurrent = playerTwoData.currentCash;
            usedAbility = playerTwoData.abilityUsed;
            currentPath = playerTwoData.path;
            spaceInt = playerTwoData.currentSpace;

            storedOffence = playerTwoData.offenceCards;
            storedDefence = playerTwoData.defenceCards;
            storedMovement = playerTwoData.movementCards;
            storedStatus = playerTwoData.statusCards;
            storedRelics = playerTwoData.relics;
            storedOmens = playerTwoData.omens;

            storedEffects = playerTwoData.effectCooldown;
            storedBuffs = playerTwoData.buffCooldown;
            storedValues = playerTwoData.buffValue;
        }

        controller.ChangeStats();
        StartCoroutine(LoadingData());
    }

    public void SavePlayer(object sender, EventArgs e)
    {
        saveData?.Invoke(this, EventArgs.Empty);

        PlayerData playerTwoData = new PlayerData
        {
            currentHealth = healthCurrent,
            maxHealth = healthMax,
            currentMana = manaCurrent,
            maxMana = manaMax,
            currentCash = cashCurrent,
            abilityUsed = usedAbility,
            path = currentPath,
            currentSpace = spaceInt,
            effectCooldown = storedEffects,
            buffCooldown = storedBuffs,
            buffValue = storedValues,
            offenceCards = storedOffence,
            defenceCards = storedDefence,
            movementCards = storedMovement,
            statusCards = storedStatus,
            relics = storedRelics,
            omens = storedOmens,
        };

        saveSystem.SaveTwo(playerTwoData);
    }

    //TODO - Change return null to yield return new WaitUntil( Suitable Boolean becomes true)
    IEnumerator LoadingData()
    {
        yield return new WaitUntil(() => controllerComplete == true);
        //This loads all the stored cards that were obtained where the game left off
        offenceDeckPool offenceDeck = player.GetComponentInChildren<offenceDeckPool>();
        for (int i = 0; i < storedOffence.Count; i++)
        {
            offenceDeck.LoadOffenceCards(storedOffence[i]);
        }

        defenceDeckPool defenceDeck = player.GetComponentInChildren<defenceDeckPool>();
        for (int i = 0; i < storedDefence.Count; i++)
        {
            defenceDeck.LoadDefenceCards(storedDefence[i]);
        }

        movementDeckPool movementDeck = player.GetComponentInChildren<movementDeckPool>();
        for (int i = 0; i < storedMovement.Count; i++)
        {
            movementDeck.LoadMovementCards(storedMovement[i]);
        }

        statusDeckPool statusDeck = player.GetComponentInChildren<statusDeckPool>();
        for (int i = 0; i < storedStatus.Count; i++)
        {
            statusDeck.LoadStatusCards(storedStatus[i]);
        }

        itemDeckPool itemDeck = player.GetComponentInChildren<itemDeckPool>();
        for (int i = 0; i < storedRelics.Count; i++)
        {
            itemDeck.LoadRelics(storedRelics[i]);
        }

        for (int i = 0; i < storedOmens.Count; i++)
        {
            itemDeck.LoadOmens(storedOmens[i]);
        }
        decksComplete = true;
        yield return new WaitUntil(() => decksComplete == true);

        for (int i = 0; i < storedEffects.Length; i++)
        {
            if (storedEffects[i] > 0)
            {
                effects.AddEffect(effectEnums[i], storedEffects[i]);
            }
        }

        for (int i = 0; i < storedBuffs.Length; i++)
        {
            if (storedBuffs[i] > 0)
            {
                buffs.AddBuff(buffEnums[i], storedBuffs[i], storedValues[i]);
            }
        }
    }

    private void OnDisable()
    {
        dataManager.loadFiles -= LoadPlayer;
        dataManager.saveFiles -= SavePlayer;
        saveData -= controller.StoreStats;
        saveData -= effects.SaveEffects;
        saveData -= buffs.SaveBuffs;
    }
}
