using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private playerModel playerModel;
    [SerializeField] private characterData Data;
    [SerializeField] private GameObject currentSpace;

    public playerModel GetModel { get { return playerModel; } }
    public characterData GetData {  get { return Data; } }

    public GameObject Space { get { return currentSpace; } set { currentSpace = value; } }

    void Awake()
    {
        playerModel = new playerModel(Data);
        transform.position = new Vector3(currentSpace.transform.position.x, 2f, currentSpace.transform.position.z);
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
