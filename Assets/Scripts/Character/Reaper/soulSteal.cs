using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulSteal : MonoBehaviour
{
    //This is the controller which will need to reference the player object into the parent
    private playerController controller;

    //This reference the combat system in order to gain health from the difference * 0.25
    [SerializeField] private GameObject combatManager;
    combatSystem combatSystem;

    private float healthPercentage = 0.25f; //25% in float
    [SerializeField] private int healthValue;
    [SerializeField] private int attackValue;
    [SerializeField] private int defendValue;

    //When awake the class has to gather the controller component
    void Awake()
    {
        controller = GetComponentInParent<playerController>();
        combatManager = GameObject.Find("CombatSystem");
        combatSystem = combatManager.GetComponent<combatSystem>();
        controller.passiveEvent += GainHealth;
    }

    public void GainHealth(object sender, EventArgs e)
    {
        attackValue = combatSystem.AttackValue;
        defendValue = combatSystem.DefendValue;
        healthValue = (int)((attackValue - defendValue) * healthPercentage);
        controller.ChangeHealth(healthValue);
        Debug.Log("Heal Value: " + healthValue);
    }

    private void OnDisable()
    {
        controller.passiveEvent -= GainHealth;
    }
}
