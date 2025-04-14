using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Stance Swap is the One Use Ability for the Double Wielder that during the deciding state can change the current stance to the other stance
/// This ignores the cooldown and swaps to the other stance
/// However this ability can only be used once per game
/// </summary>

public class StanceSwap : MonoBehaviour
{
    //The boolean is used to prevent the player from using the ability again
    private bool passiveUsed;
    
    //this needs the controller from the parent (player object) and the passive ability from the character object
    private playerController controller;
    private passiveAgression passiveAbility;

    [Header("User Interface")]
    private decidingState stateUI;

    //This gets the component the ability needs to be used
    void Awake()
    {
        controller = GetComponentInParent<playerController>();
        stateUI = GetComponentInParent<decidingState>();
        passiveAbility = GetComponent<passiveAgression>();
        controller.oneUseEvent += SwapStance;
    }

    //The Swap Stance can be use during the deciding state and can make the character change his current state
    public void SwapStance(object sender, EventArgs e)
    {
        if (!passiveUsed)
        {
            passiveAbility.ChangeStance();
            passiveUsed = true;
            stateUI.EventText.SetText("Ability Used - Stance Swap: You stance has now changed and is set the change timer back to 3");
        }
        else
        {
            Debug.LogWarning("Cannot swap stances again");
        }
    }

    public void OnDisable()
    {
        controller.oneUseEvent -= SwapStance;
    }
}
