using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// This is the player controller (presenter) that provides the logic for the model and changes to the view
/// This collect the character data and applies changes to the model.
/// </summary>
public class playerController : MonoBehaviour
{
    //this provide encapsulation of the player model, character data and the current path
    //this allows other classes to reference these to return the data
    //However the choosing state can set a new current path for the controller to collect and provide
    private playerModel playerModel;
    private playerView playerView;
    [SerializeField] private characterData Data;

    //This is to provide the current path for the move state to use
    [SerializeField] private GameObject currentPath;

    //These are the events to activate the passive and one use ability
    public event EventHandler passiveEvent;
    public event EventHandler oneUseEvent;

    //These are the events to activate the effects that occur
    public event EventHandler effectStartEvent;
    public event EventHandler effectEndEvent;

    public playerModel GetModel { get { return playerModel; } }
    public characterData GetData {  get { return Data; } }

    public GameObject Path { get { return currentPath; } set { currentPath = value; } }

    void Awake()
    {
        //this collects the path list for the player to start on
        pathOrder startingSpace = currentPath.GetComponent<pathOrder>();

        //this creates a new player model based on the character the player has chosen
        playerModel = new playerModel(Data);
        transform.position = new Vector3(startingSpace.SpaceOrder[1].transform.position.x, 2f, startingSpace.SpaceOrder[1].transform.position.z);

        //this collects the view for providing the interface of the statistics
        playerView = GetComponent<playerView>();

        Instantiate(GetData.characterObject, this.transform);
    }

    //This is to reset the multipliers from the effects of their previous turn
    //Thus also regains the mana for the player to use cards
    public void ResetStats(object sender, EventArgs e)
    {
        playerModel.CurrentMana = playerModel.MaxMana;
        playerView.ManaUI();
        ChangeThrust(1);
        ChangeGuard(1);
        ChangeRoll(1);
    }

    //ChangeCash is a method that when landing on a blue or red space will change the current cash to certain value
    public void ChangeCash(int value)
    {

        if (playerModel.CurrentCash + value < 0)
        {
            playerModel.CurrentCash = 0;
        }

        else
        {
            playerModel.CurrentCash += value;

        }

        playerView.CashUI();
    }

    public void ChangeMana(int cost)
    {
        playerModel.CurrentMana -= cost;
        playerView.ManaUI();
    }

    //Roll is a mathod that subtracts the mana based on mana cost (parameter is roll cost) and the value of the dice (parameter is value)
    public void Roll(int value) 
    { 
        currentEffects shockEffect = GetComponent<currentEffects>();

        playerModel.RollValue = value;
        if (shockEffect.Shocked) 
        {
            ChangeHealth(-value);
        }
        Debug.Log(playerModel.RollValue);
    }

    public void ChangeHealth(int value) 
    {
        //This gets the current buff component to see if the player is invincble from taking any damage

        currentBuffs buffs = GetComponent<currentBuffs>();
        //If the player is invincle & would take and damage this turn
        //Then turn the value to 0 to prevent any damage from occurring
        if(buffs.IsInvincible && value < 0)
        {
            value = 0;
        }

        //If the current health being subtracted from the value is less than 0, the player is defeated
        if (playerModel.CurrentHealth + value <= 0)
        {
            playerModel.IsAlive = false;
        }
        //Else if the current health being added from the the value is greater than the max health, the current health will maximise to only the maximum health
        else if (playerModel.CurrentHealth + value > playerModel.MaxHealth)
        {
            playerModel.CurrentHealth = playerModel.MaxHealth;
        }
        //otherwise the value adds (or subtract if the value is negative) to the new current health
        else
        {
            playerModel.CurrentHealth += value;
        }

        playerView.HealthUI();
    }

    //For any Multiplier Changes the procedure for the parameter must be:
    //Collecting the Multiplier from the mutliplier
    //Then Add/Substract/Divide/Multiply the multiplier
    //That paremeter float value is now the new multiplier
    public void ChangeThrust(float value)
    {
        if (value < 0) 
        {
            GetModel.ThrustMultiplier = 0;
        }
        else
        {
            GetModel.ThrustMultiplier = value;
        }
        playerView.ThrustUI();
    }

    public void ChangeGuard(float value)
    {
        if (value < 0)
        {
            GetModel.GuardMultiplier = 0;
        }
        else
        {
            GetModel.GuardMultiplier = value;
        }
        playerView.GuardUI();
    }

    public void ChangeRoll(float value)
    {
        if (value < 0)
        {
            GetModel.RollMultiplier = 0;
        }
        else
        {
            GetModel.RollMultiplier = value;
        }
        playerView.RollUI();
    }

    public void IncrementDeck(deckTypeEnum deck)
    {
        if (deck == deckTypeEnum.Offence) 
        {
            GetModel.OffenceCards++;
            playerView.OffenceUI();
        }

        else if(deck == deckTypeEnum.Defence)
        {
            GetModel.DefenceCards++;
            playerView.DefenceUI();
        }

        else if(deck == deckTypeEnum.Movement)
        {
            GetModel.MovementCards++;
            playerView.MovementUI();
        }

        else if(deck == deckTypeEnum.Status)
        {
            GetModel.StatusCards++;
            playerView.StatusUI();
        }

        else if(deck == deckTypeEnum.Item)
        {
            GetModel.ItemPile++;
            playerView.ItemUI();
        }

        else
        {
            Debug.LogError("There's a possible chance that the type is not suitable");
        }
    }

    //This is activated when the passive ability is triggered from the player's specifc character
    public void ActivatePassive()
    {
        passiveEvent?.Invoke(this, EventArgs.Empty);
    }

    //This is activated when the player wants to use their one use ability from the player's specifc character.
    public void ActivateOneUse()
    {
        oneUseEvent?.Invoke(this, EventArgs.Empty);
        GetModel.AbilityUsed = false;
        playerView.OneUseAbilityUI();
    }

    public void ActivateStartEffects(object sender, EventArgs e)
    {
        effectStartEvent?.Invoke(this, EventArgs.Empty);
    }

    public void ActivateEndEffect()
    {
        effectEndEvent?.Invoke(this, EventArgs.Empty);
    }

    public void DisplayEffect(int enumInt, bool display)
    {
        playerView.EffectUI(enumInt, display);
    }

    public void DisplayBuff(int enumInt, bool display)
    {
        playerView.BuffUI(enumInt, display);
    }

    public void DisplayAbility(Sprite icon, Color colour)
    {
        playerView.AbilityUI(icon, colour);
    }
}
