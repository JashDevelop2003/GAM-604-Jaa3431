using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentBuffs : MonoBehaviour
{
    private playerController controller;

    [SerializeField] private bool isInvincible;
    private int invincibleCooldown;
    public bool IsInvincible
    {
        get { return isInvincible; }    
    }

    [SerializeField] private bool isHealthy;
    private int healthyCooldown;
    public bool IsHealthy
    {
        get { return isHealthy; }
    }

    [SerializeField] private bool isHasty;
    private int hastyCooldown;
    public bool IsHasty
    {
        get { return isHasty; }
    }

    [SerializeField] private bool isLucky;
    private int luckyCooldown;
    public bool IsLucky
    {
        get { return isLucky; }
    }

    [SerializeField] private bool isResistant;
    private int resistantCooldown;
    private float resistantValue;
    public bool IsResistant
    {
        get { return isResistant; }
    }

    [SerializeField] private bool isImpactful;
    private int impactfulCooldown;
    private float impactfulValue;
    public bool IsImpactful
    {
        get { return isImpactful; }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<playerController>();
    }

    //This method has similar procedure and implementation as the current effect's AddEffect
    //This slight difference being that there is a float value for some effects to use.
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
                controller.effectEndEvent += InvinciblePlayer;
            }
            Debug.Log("Player is Invincible for " + cooldown + " more turns. cooldown is now: " + invincibleCooldown);
        }

        else if (type == buffEnum.Healthy) 
        {
            isHealthy = true;
            if (healthyCooldown > 0)
            {
                healthyCooldown += cooldown;
            }
            else
            {
                healthyCooldown = cooldown;
                controller.effectEndEvent += HealthyPlayer;
            }
            Debug.Log("Player is Healthy for " + cooldown + " more turns. cooldown is now: " + healthyCooldown);
        }

        else if (type == buffEnum.Hasty)
        {
            isHasty = true;
            if (hastyCooldown > 0)
            {
                hastyCooldown += cooldown;
            }
            else
            {
                hastyCooldown = cooldown;
                controller.effectStartEvent += HastyPlayer;
            }
            Debug.Log("Player is Hasty for " + cooldown + " more turns cooldown is now " + hastyCooldown);
        }

        if (type == buffEnum.Lucky)
        {
            isLucky = true;
            if (luckyCooldown > 0)
            {
                luckyCooldown += cooldown;
            }
            else
            {
                luckyCooldown = cooldown;
                controller.effectEndEvent += LuckyPlayer;
            }

            Debug.Log("Player is Hasty for " + cooldown + " more turns cooldown is now " + hastyCooldown);
        }

        else if (type == buffEnum.Resistant)
        {
            isResistant = true;
            Debug.Log("Previous Resistant Value & Cooldown: " + resistantValue + " value " + resistantCooldown + "cooldown");
            //If the new Resistant value is higher than the current buff OR If the current value is 0
            //Change both the cooldown & value to the new value & cooldown
            if (resistantValue < value || resistantValue == 0)
            {
                resistantValue = value;

                //This is use to ensure that there's only one Resistant event occurring when starting the turn
                if (resistantCooldown > 0)
                {
                    resistantCooldown = cooldown;
                }
                else
                {
                    resistantCooldown = cooldown;
                    controller.effectStartEvent += ResistantPlayer;
                }
            }
            //otherwise if the values are the same value then check the cooldown
            else if (resistantValue == value)
            {
                //if there is a Resistant cooldown then increase the cooldown
                if (resistantCooldown > 0)
                {
                    resistantCooldown += cooldown;
                }

                //otherwise create new cooldown and add the event (in order to have the method being called)
                else
                {
                    resistantCooldown = cooldown;
                    controller.effectStartEvent += ResistantPlayer;
                }
            }

            else if (resistantValue > value)
            {
                Debug.LogWarning(value + " is less than current value");
            }

            Debug.Log("Current Resistant Value & Cooldown: " + resistantValue + " value " + resistantCooldown + "cooldown");

        }


        else if (type == buffEnum.Impactful)
        {
            isImpactful = true;
            Debug.Log("Previous Impactful Value & Cooldown: " + impactfulValue + " value " + impactfulCooldown + "cooldown");
            //If the new impact value is higher than the current buff OR If the current value is 0
            //Change both the cooldown & value to the new value & cooldown
            if (impactfulValue < value || impactfulValue == 0)
            {
                impactfulValue = value;

                //This is use to ensure that there's only one impactful event occurring when starting the turn
                if(impactfulCooldown > 0)
                {
                    impactfulCooldown = cooldown;
                }
                else
                {
                    impactfulCooldown = cooldown;
                    controller.effectStartEvent += ImpactfulPlayer;
                }
            }
            //otherwise if the values are the same value then check the cooldown
            else if(impactfulValue == value)
            {
                //if there is a impact cooldown then increase the cooldown
                if (impactfulCooldown > 0)
                {
                    impactfulCooldown += cooldown;
                }

                //otherwise create new cooldown and add the event (in order to have the method being called)
                else
                {
                    impactfulCooldown = cooldown;
                    controller.effectStartEvent += ImpactfulPlayer;
                }
            }

            else if(impactfulValue > value)
            {
                Debug.LogWarning(value + " is less than current value");
            }

            Debug.Log("Current Impactful Value & Cooldown: " + impactfulValue + " value " + impactfulCooldown + "cooldown");
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

    public void HealthyPlayer(object sender, EventArgs e)
    {
        healthyCooldown--;
        if (healthyCooldown <= 0)
        {
            isHealthy = false;
            controller.effectEndEvent -= HealthyPlayer;
        }
    }

    public void HastyPlayer(object sender, EventArgs e)
    {
        if (isHasty)
        {
            controller.GetModel.RollMultiplier *= 2;
            Debug.Log("RollMultiplier = " + controller.GetModel.RollMultiplier);
            hastyCooldown--;
            if (hastyCooldown <= 0)
            {
                isHasty = false;
                controller.effectStartEvent -= HastyPlayer;
            }
        }
    }

    public void LuckyPlayer(object sender, EventArgs e)
    {
        luckyCooldown--;
        if (luckyCooldown <= 0)
        {
            isLucky = false;
            controller.effectEndEvent -= LuckyPlayer;
        }
    }

    public void ResistantPlayer(object sender, EventArgs e)
    {
        if (isResistant)
        {
            controller.GetModel.GuardMultiplier += resistantValue;
            Debug.Log(resistantValue + " 1 = " + controller.GetModel.GuardMultiplier);
            resistantCooldown--;
            if (resistantCooldown <= 0)
            {
                isResistant = false;
                resistantValue = 0;
                controller.effectStartEvent -= ResistantPlayer;
            }
        }
    }

    public void ImpactfulPlayer(object sender, EventArgs e)
    {
        if (isImpactful)
        {
            controller.GetModel.ThrustMultiplier += impactfulValue;
            Debug.Log(impactfulValue + " 1.  Guard Value = " + controller.GetModel.ThrustMultiplier);
            impactfulCooldown--;
            if (impactfulCooldown <= 0)
            {
                isImpactful = false;
                impactfulValue = 0;
                controller.effectStartEvent -= ImpactfulPlayer;
            }
        }
    }

}
