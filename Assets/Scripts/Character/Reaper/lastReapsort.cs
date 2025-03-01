using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lastReapsort : MonoBehaviour
{
    //The boolean is used to prevent the player from using the ability again
    private bool passiveUsed = false;
    private bool lastReapsortActive = false;
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


    //When awake the class has to gather the controller component
    void Awake()
    {
        controller = GetComponentInParent<playerController>();
        stateManager = GetComponentInParent<playerStateManager>();
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
            abilityBegin = StartCoroutine(Reaping());

        }
        else
        {
            Debug.LogWarning("Cannot go to Last Reapsort again");
        }
    }

    public void CheckOpponentHealth(object sender, EventArgs e)
    {
        playerController opponent = combatSystem.DefendingPlayer.GetComponent<playerController>();
        if (!opponent.GetModel.IsAlive)
        {
            lastReapsortActive = false;
        }
    }

    private IEnumerator Reaping()
    {
        while(lastReapsortActive || stateManager.CurrentState != stateManager.InactiveState)
        {
            yield return null;
        }
        if (!lastReapsortActive)
        {
            controller.oneUseEvent -= CheckOpponentHealth;
            controller.oneUseEvent += BeginReaping;
            Debug.Log("Player Successfully Defeated Someone");
        }
        else if(stateManager.CurrentState == stateManager.InactiveState)
        {
            controller.GetModel.IsAlive = false;
            Debug.Log("Player has Failed to Defeat Someone");
        }
    }

    private void OnDisable()
    {
        controller.oneUseEvent -= BeginReaping;
        controller.oneUseEvent -= CheckOpponentHealth;
    }
}
