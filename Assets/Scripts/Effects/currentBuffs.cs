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

    [Header("Sound Effect")]
    [SerializeField] private AudioClip buffSound;
    [SerializeField] private AudioClip[] effectSounds = new AudioClip[6];
    private soundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<playerController>();
        soundManager = Singleton<soundManager>.Instance;
    }

    public void SaveBuffs(object sender, EventArgs e)
    {
        if (controller.Player == 1)
        {
            playerOneData playerData = GetComponentInChildren<playerOneData>();
            playerData.storedBuffs[0] = invincibleCooldown;
            playerData.storedBuffs[1] = healthyCooldown;
            playerData.storedBuffs[2] = hastyCooldown;
            playerData.storedBuffs[3] = luckyCooldown;
            playerData.storedBuffs[4] = resistantCooldown;
            playerData.storedValues[4] = impactfulValue;
            playerData.storedBuffs[5] = impactfulCooldown;
            playerData.storedValues[5] = impactfulCooldown;
        }
        else if (controller.Player == 2) 
        {
            playerTwoData playerData = GetComponentInChildren<playerTwoData>();
            playerData.storedBuffs[0] = invincibleCooldown;
            playerData.storedBuffs[1] = healthyCooldown;
            playerData.storedBuffs[2] = hastyCooldown;
            playerData.storedBuffs[3] = luckyCooldown;
            playerData.storedBuffs[4] = resistantCooldown;
            playerData.storedValues[4] = impactfulValue;
            playerData.storedBuffs[5] = impactfulCooldown;
            playerData.storedValues[5] = impactfulCooldown;
        }
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
                controller.DisplayBuff((int)type, true);
            }
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
                controller.DisplayBuff((int)type, true);
            }
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
                controller.DisplayBuff((int)type, true);
            }
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
                controller.DisplayBuff((int)type, true);
            }

        }

        else if (type == buffEnum.Resistant)
        {
            isResistant = true;
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
                    controller.DisplayBuff((int)type, true);
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
                    controller.DisplayBuff((int)type, true);
                }
            }


        }


        else if (type == buffEnum.Impactful)
        {
            isImpactful = true;
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
                    controller.DisplayBuff((int)type, true);
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
                    controller.DisplayBuff((int)type, true);
                }
            }

        }

        soundManager.PlaySound(buffSound);
    }

    //Invincible prevents the player to take any damage
    public void InvinciblePlayer(object sender, EventArgs e)
    {
        invincibleCooldown--;
        EffectSound((int)buffEnum.Invincible);
        if (invincibleCooldown <= 0)
        {
            isInvincible = false;
            controller.effectEndEvent -= InvinciblePlayer;
            controller.DisplayBuff((int)buffEnum.Invincible, false);
        }
    }

    //Healthy prevents the player from gaining effects
    public void HealthyPlayer(object sender, EventArgs e)
    {
        healthyCooldown--;
        EffectSound((int)buffEnum.Healthy);
        if (healthyCooldown <= 0)
        {
            isHealthy = false;
            controller.effectEndEvent -= HealthyPlayer;
            controller.DisplayBuff((int)buffEnum.Healthy, false);
        }
    }

    //Hasty Doubles the roll value at the start of the turn
    public void HastyPlayer(object sender, EventArgs e)
    {
        if (isHasty)
        {
            controller.ChangeRoll(controller.GetModel.RollMultiplier * 2);
            hastyCooldown--;
            EffectSound((int)buffEnum.Hasty);
            if (hastyCooldown <= 0)
            {
                isHasty = false;
                controller.effectStartEvent -= HastyPlayer;
                controller.DisplayBuff((int)buffEnum.Hasty, false);
            }
        }
    }

    //Lucky increases the chance of obtaining rarer cards
    public void LuckyPlayer(object sender, EventArgs e)
    {
        luckyCooldown--;
        EffectSound((int)buffEnum.Lucky);
        if (luckyCooldown <= 0)
        {
            isLucky = false;
            controller.effectEndEvent -= LuckyPlayer;
            controller.DisplayBuff((int)buffEnum.Lucky, false);
        }
    }

    //Resistant Increase Guard by a certain percentage
    public void ResistantPlayer(object sender, EventArgs e)
    {
        if (isResistant)
        {
            controller.ChangeGuard(controller.GetModel.GuardMultiplier + resistantValue);
            resistantCooldown--;
            EffectSound((int)buffEnum.Resistant);
            if (resistantCooldown <= 0)
            {
                isResistant = false;
                resistantValue = 0;
                controller.effectStartEvent -= ResistantPlayer;
                controller.DisplayBuff((int)buffEnum.Resistant, false);
            }
        }
    }

    //Impactful Increases Thrust by a certain percentage
    public void ImpactfulPlayer(object sender, EventArgs e)
    {
        if (isImpactful)
        {
            controller.ChangeThrust(controller.GetModel.ThrustMultiplier + impactfulValue);
            impactfulCooldown--;
            EffectSound((int)buffEnum.Impactful);
            if (impactfulCooldown <= 0)
            {
                isImpactful = false;
                impactfulValue = 0;
                controller.effectStartEvent -= ImpactfulPlayer;
                controller.DisplayBuff((int)buffEnum.Impactful, false);
            }
        }
    }

    public void EffectSound(int sound)
    {
        soundManager.PlaySound(effectSounds[sound]);
    }

}
