using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private playerModel playerModel;
    private playerStateManager stateManager;
    [SerializeField] private characterData Data;
    public characterData GetData {  get { return Data; } }

    // Start is called before the first frame update
    private void Start()
    {
        stateManager = GetComponent<playerStateManager>();
        stateManager.startTurn += RegainMana;
    }

    void Awake()
    {
        playerModel = new playerModel(Data);
        stateManager = new playerStateManager();
    }

    private void OnEnable()
    {
        stateManager.startTurn += RegainMana;
    }


    public void RegainMana(object sender, EventArgs e)
    {
        playerModel.CurrentMana = playerModel.MaxMana;
    }

    public void OnDisable()
    {
        stateManager.startTurn -= RegainMana;
    }


}
