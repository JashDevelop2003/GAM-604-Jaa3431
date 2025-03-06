using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentEffects : MonoBehaviour
{
    private playerController controller;
    
    [SerializeField] private bool isBurned;
    [SerializeField] private int burnCooldown;

    [SerializeField] private bool isSlowed;
    [SerializeField] private int slowCooldown;

    private void Start()
    {
        controller = GetComponent<playerController>();
    }

    public void AddEffect(effectEnum type, int cooldown)
    {
        if(type == effectEnum.Burned)
        {
            isBurned = true;
            burnCooldown = cooldown;
            controller.effectEvent += BurnPlayer;
        }

        else if (type == effectEnum.Slowed)
        {
            isSlowed = true;
            slowCooldown = cooldown;
            controller.effectEvent += SlowPlayer;
        }
    }

    public void BurnPlayer(object sender, EventArgs e)
    {
        if (isBurned) 
        {
            Debug.Log("Damage Player = Offence Cards");
            burnCooldown--;
            if (burnCooldown <= 0) 
            { 
                isBurned = false;
                controller.effectEvent -= BurnPlayer;
            }

        }
    }

    public void SlowPlayer(object sender, EventArgs e)
    {
        if (isSlowed) 
        {
            Debug.Log("Roll Multiplier = 0.8f");
            slowCooldown--;
            if(slowCooldown <= 0)
            {
                isSlowed = false;
                controller.effectEvent -= SlowPlayer;
            }
        }
    }

}
