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

        Instantiate(GetData.characterObject, this.transform);
    }

    //This is to reset the multipliers from the effects of their previous turn
    //Thus also regains the mana for the player to use cards
    public void ResetStats(object sender, EventArgs e)
    {
        playerModel.RollMultiplier = 1;
        playerModel.ThrustMultiplier = 1;
        playerModel.GuardMultiplier = 1;
        playerModel.CurrentMana = playerModel.MaxMana;
        Debug.Log("Mana Regain");
    }

    //ChangeCash is a method that when landing on a blue or red space will change the current cash to certain value
    public void ChangeCash(int value)
    {

        if (value >= playerModel.CurrentCash)
        {
            playerModel.CurrentCash = 0;
        }

        else
        {
            playerModel.CurrentCash += value;

        }

        Debug.Log(this.gameObject + "'s Cash Changed to: " + playerModel.CurrentCash);
    }

    public void ChangeMana(int cost)
    {
        playerModel.CurrentMana -= cost;
        Debug.Log(this.gameObject  + "'s Mana Changed to: " + playerModel.CurrentMana);
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
            Debug.Log("You take no damage due to being invincible");
        }

        //If the current health being subtracted from the value is less than 0, the player is defeated
        if (playerModel.CurrentHealth + value <= 0)
        {
            playerModel.IsAlive = false;
            Debug.Log("Game Over");
        }
        //Else if the current health being added from the the value is greater than the max health, the current health will maximise to only the maximum health
        else if (playerModel.CurrentHealth + value > playerModel.MaxHealth)
        {
            playerModel.CurrentHealth = playerModel.MaxHealth;
            Debug.Log("No Overhealth in this game, otherwise the game will be longer :-) ");
        }
        //otherwise the value adds (or subtract if the value is negative) to the new current health
        else
        {
            playerModel.CurrentHealth += value;
            Debug.Log(this.gameObject + "'s Health Changed " + playerModel.CurrentHealth + " / " + playerModel.MaxHealth);
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
    }

    public void ActivateStartEffects(object sender, EventArgs e)
    {
        effectStartEvent?.Invoke(this, EventArgs.Empty);
    }

    public void ActivateEndEffect()
    {
        effectEndEvent?.Invoke(this, EventArgs.Empty);
    }
}
