using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private playerModel playerModel;
    [SerializeField] private characterData Data;
    public characterData GetData {  get { return Data; } }
    public playerModel GetModel {  get { return playerModel; } }

    void Awake()
    {
        playerModel = new playerModel(Data);
    }


    public void RegainMana(object sender, EventArgs e)
    {
        playerModel.CurrentMana = playerModel.MaxMana;
        Debug.Log("Event Sent");
    }

    public void Roll(int rollCost, int value) 
    { 
        playerModel.CurrentMana -= rollCost;
        playerModel.RollValue = value;
        Debug.Log(playerModel.CurrentMana + "/" + playerModel.MaxMana);
        Debug.Log(playerModel.RollValue);
    }


}
