using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soulSteal : MonoBehaviour
{
    //This is the controller which will need to reference the player object into the parent
    private playerController controller;

    //This reference the combat system in order to gain health from the difference * 0.25
    combatSystem combatSystem;

    //This is use to provide the player gaining health based on the differece * 25%
    private float healthPercentage = 0.25f; //25% in float
    private int healthValue;
    private int attackValue;
    private int defendValue;

    //When awake the class has to gather the controller component
    void Start()
    {
        controller = GetComponentInParent<playerController>();

        //The object is a prefab meaning that when instantiated won't have the combat system object
        //This means that the class requires to find the prefab of the combat system
        combatSystem = combatSystem.instance;
        combatSystem.duringCombatEvent += GainHealth;

        controller.DisplayAbility(controller.GetData.abilityIcon[0], controller.GetData.abilityColour[0]);
    }

    //This method is called when the passive ability in the controller is invoked
    //This heals the player's health based on the damage dealt * 25%
    public void GainHealth(object sender, EventArgs e)
    {
        //This gathers the attack and defend value from the combat system and apply the same as damage outcome
        //Except this then multiply by the health percentage
        //e.g if Attack Value is 12 & Defend Value is 4, then this will heal the player to 2 health
        //int values always round any float value down
        if (combatSystem.AttackValue > combatSystem.DefendValue) 
        {
            healthValue = (int)((combatSystem.AttackValue - combatSystem.DefendValue) * healthPercentage);
            controller.ChangeHealth(healthValue);
        }
    }

    private void OnDisable()
    {
        combatSystem.duringCombatEvent -= GainHealth;
    }
}
