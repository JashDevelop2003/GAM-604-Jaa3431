using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentBuffs : MonoBehaviour
{
    private playerController controller;

    [SerializeField] private bool isInvincible;
    [SerializeField] private int invincibleCooldown;
    public bool IsInvincible
    {
        get { return isInvincible; }    
    }

    [SerializeField] private bool isHealthy;
    [SerializeField] private int healthyCooldown;

    [SerializeField] private bool isHasty;
    [SerializeField] private int hastyCooldown;

    [SerializeField] private bool isLucky;
    [SerializeField] private int luckyCooldown;

    [SerializeField] private bool isResistant;
    [SerializeField] private int resistantCooldown;
    [SerializeField] private float resistantValue;

    [SerializeField] private bool isImpactful;
    [SerializeField] private int impactfulCooldown;
    [SerializeField] private float impactfulValue;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<playerController>();
    }

    public void AddBuff(buffEnum type, int cooldown, float value)
    {
        if (type == buffEnum.Invincible)
        {
            isInvincible = true;
            if (invincibleCooldown > 0)
            {
                invincibleCooldown += cooldown;
            }
            else
            {
                invincibleCooldown = cooldown;                
            }
            controller.effectEndEvent += InvinciblePlayer;
        }
    }

    public void InvinciblePlayer(object sender, EventArgs e)
    {
        invincibleCooldown--;
        if (invincibleCooldown <= 0)
        {
            isInvincible = false;
            controller.effectEndEvent -= InvinciblePlayer;
        }
    }


}
