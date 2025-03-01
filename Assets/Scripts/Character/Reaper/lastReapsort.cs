using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lastReapsort : MonoBehaviour
{
    //The boolean is used to prevent the player from using the ability again
    private bool passiveUsed = false;
    [SerializeField] private bool lastReapsortActive = false;
    public bool LastReapsortActive
    {
        get { return lastReapsortActive; }
    }

    private Coroutine abilityBegin;

    //This is the controller which will need to reference the player object into the parent
    private playerController controller;
    private playerStateManager stateManager;

    [SerializeField] private GameObject combatManager;
    combatSystem combatSystem;

    [SerializeField] private GameObject opponentObject;
    public GameObject OpponentObject
    {
        get { return opponentObject; }
        set { opponentObject = value; }
    }



    //When awake the class has to gather the controller component
    void Awake()
    {
        controller = GetComponentInParent<playerController>();
        stateManager = GetComponentInParent<playerStateManager>();
        combatManager = GameObject.Find("CombatSystem");
        combatSystem = combatManager.GetComponent<combatSystem>();

        controller.oneUseEvent += BeginReaping;
    }

    public void BeginReaping(object sender, EventArgs e)
    {
        if (!passiveUsed)
        {
            passiveUsed = true;
            lastReapsortActive = true;
            controller.oneUseEvent += CheckOpponentHealth;
            controller.oneUseEvent -= BeginReaping;
        }
        else
        {
            Debug.LogWarning("Cannot go to Last Reapsort again");
        }
    }

    public void CheckOpponentHealth(object sender, EventArgs e)
    {
        playerController opponentController = opponentObject.GetComponent<playerController>();
        if (!opponentController.GetModel.IsAlive)
        {
            lastReapsortActive = false;
        }
        else
        {
            Debug.Log("Still alive");
        }
    }

    private void OnDisable()
    {
        controller.oneUseEvent -= BeginReaping;
        controller.oneUseEvent -= CheckOpponentHealth;
    }
}
