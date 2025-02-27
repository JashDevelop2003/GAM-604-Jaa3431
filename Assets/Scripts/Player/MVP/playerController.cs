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
    [SerializeField] private GameObject currentPath;

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
    }

    //Regain mana is a method that during the start state will make the current mana equal to max mana
    public void RegainMana()
    {
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

        Debug.Log("Cash Changed to: " + playerModel.CurrentCash);
    }

    //Roll is a mathod that subtracts the mana based on mana cost (parameter is roll cost) and the value of the dice (parameter is value)
    public void Roll(int rollCost, int value) 
    { 
        playerModel.CurrentMana -= rollCost;
        playerModel.RollValue = value;
        Debug.Log(playerModel.CurrentMana + "/" + playerModel.MaxMana);
        Debug.Log(playerModel.RollValue);
    }

    public void ChangeHealth(int value) 
    {
        if(value >= playerModel.CurrentHealth)
        {
            Debug.Log("Game Over");
        }
        else
        {
            playerModel.CurrentHealth += value;
        }
    }


}
