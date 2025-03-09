using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// This is the current effects to check which effects are currently true
/// If an effect is true, then this will add an event to either the start or end effect depending on the effect
/// Some effects might need to have their boolean encapsulated in order to get the boolean value for other components
/// </summary>

public class currentEffects : MonoBehaviour
{
    private playerController controller;


    //Each effect has a bool to check if the effect is active on the player
    //Each effect also has a int for the cooldown that decrements either at the start or the end of their turn
    //Some effect also have a getter that other scripts can use to get the value of the boolean
    [SerializeField] private bool isBurned;
    private int burnCooldown;

    [SerializeField] private bool isSlowed;
    private int slowCooldown;

    [SerializeField] private bool isShocked;
    private int shockCooldown;
    public bool Shocked
    {
        get { return isShocked; }
    }

    [SerializeField] private bool isExposed;
    private int exposeCooldown;

    [SerializeField] private bool isBleeding;
    private int bleedCooldown;

    [SerializeField] private bool isPoisoned;
    private int poisonCooldown;

    [SerializeField] private bool isBlistered;
    private int blisterCooldown;

    [SerializeField] private bool isUnstabled;
    private int unstableCooldown;

    [SerializeField] private bool isStunned;
    private int stunCooldown;
    public bool Stunned
    {
        get { return isStunned; }
    }

    [SerializeField] private bool isFeared;
    private int fearCooldown;

    [SerializeField] private bool isConfused;
    private int confusedCooldown;
    public bool Confused
    {
        get { return isConfused; }
    }

    [SerializeField] private bool isBlind;
    private int blindCooldown;
    public bool Blind
    {
        get { return isBlind; }
    }

    private void Start()
    {
        controller = GetComponent<playerController>();
    }

    //When the current player has chosen this player
    //This method requires the type and cooldown of the status card that is effecting this player
    public void AddEffect(effectEnum type, int cooldown)
    {
        //This is the procedure of how to apply status effects
        //If the effect has a cooldown more than 0 then increase the cooldown
        if(type == effectEnum.Burned)
        {
            isBurned = true;
            if(burnCooldown > 0)
            {
                burnCooldown += cooldown;
            }
            else
            {
                burnCooldown = cooldown;

            }
            controller.effectEndEvent += BurnPlayer;
        }

        else if (type == effectEnum.Slowed)
        {
            isSlowed = true;
            if(slowCooldown > 0)
            {
                slowCooldown += cooldown;
            }
            else
            {
                slowCooldown = cooldown;

            }
            controller.effectStartEvent += SlowPlayer;
        }

        else if(type == effectEnum.Shocked)
        {
            isShocked = true;
            if (shockCooldown > 0)
            {
                shockCooldown += cooldown;
            }
            else
            {
                shockCooldown = cooldown;

            }
            controller.effectEndEvent += ShockPlayer;
        }

        else if (type == effectEnum.Exposed)
        {
            isExposed = true;
            if (exposeCooldown > 0)
            {
                exposeCooldown += cooldown;
            }
            else
            {
                exposeCooldown = cooldown;

            }
            controller.effectEndEvent += ExposePlayer;
        }

        else if (type == effectEnum.Bleeding)
        {
            isBleeding = true;
            if (bleedCooldown > 0)
            {
                bleedCooldown += cooldown;
            }
            else
            {
                bleedCooldown = cooldown;

            }
            controller.effectEndEvent += BleedPlayer;
        }

        else if (type == effectEnum.Poison)
        {
            isPoisoned = true;
            if (poisonCooldown > 0)
            {
                poisonCooldown += cooldown;
            }
            else
            {
                poisonCooldown = cooldown;

            }
            controller.effectEndEvent += PoisonPlayer;
        }

        else if (type == effectEnum.Blistered)
        {
            isBlistered = true;
            if (blisterCooldown > 0)
            {
                blisterCooldown += cooldown;
            }
            else
            {
                blisterCooldown = cooldown;

            }
            controller.effectEndEvent += BlisterPlayer;
        }

        else if (type == effectEnum.Unstabled)
        {
            isUnstabled = true;
            if (unstableCooldown > 0)
            {
                unstableCooldown += cooldown;
            }
            else
            {
                unstableCooldown = cooldown;

            }
            controller.effectEndEvent += UnstablePlayer;
        }

        else if (type == effectEnum.Stunned)
        {
            isStunned = true;
            if (stunCooldown > 0)
            {
                stunCooldown += cooldown;
            }
            else
            {
                stunCooldown = cooldown;

            }
            controller.effectEndEvent += StunPlayer;
        }

        else if (type == effectEnum.Feared)
        {
            isFeared = true;
            if (fearCooldown > 0)
            {
                fearCooldown += cooldown;
            }
            else
            {
                fearCooldown = cooldown;

            }
            controller.effectStartEvent += FearPlayer;
        }

        else if (type == effectEnum.Confused)
        {
            isConfused = true;
            if (confusedCooldown > 0)
            {
                confusedCooldown += cooldown;
            }
            else
            {
                confusedCooldown = cooldown;

            }
            controller.effectEndEvent += ConfusePlayer;
        }

        else if (type == effectEnum.Blind)
        {
            isBlind = true;
            if (blindCooldown > 0)
            {
                blindCooldown += cooldown;
            }
            else
            {
                blindCooldown = cooldown;

            }
            controller.effectEndEvent += BlindPlayer;
        }

        else
        {
            Debug.LogError("This effect doesn't exist");
        }
    }

    //If the player is burned, then the player will decrease health at the end of their turn based on the amount of offence cards they have
    public void BurnPlayer(object sender, EventArgs e)
    {
        if (isBurned) 
        {
            offenceDeckPile offenceCards = GetComponentInChildren<offenceDeckPile>();
            controller.ChangeHealth(-offenceCards.OffenceCards.Count);
            Debug.Log("Burn Deals: " + offenceCards.OffenceCards.Count + " Damage");


            burnCooldown--;
            if (burnCooldown <= 0) 
            { 
                isBurned = false;
                controller.effectEndEvent -= BurnPlayer;
            }

        }
    }

    //Slow decreases the roll value by 20%
    public void SlowPlayer(object sender, EventArgs e)
    {
        if (isSlowed) 
        {
            controller.GetModel.RollMultiplier = 0.8f;
            Debug.Log("RollMultiplier = " + controller.GetModel.RollMultiplier);
            slowCooldown--;
            if(slowCooldown <= 0)
            {
                isSlowed = false;
            }
        }
        else
        {
            controller.effectStartEvent -= SlowPlayer;
        }
    }

    //Shock deals damage equal to the roll value
    public void ShockPlayer(object sender, EventArgs e)
    {
        shockCooldown--;
        if(shockCooldown <= 0)
        {
            isShocked = false;
            controller.effectEndEvent -= ShockPlayer;
        }
    }

    //Expose Decrease Guard by 25%
    public void ExposePlayer(object sender, EventArgs e)
    {
        if (isExposed)
        {
            controller.GetModel.GuardMultiplier = 0.75f;
            Debug.Log("GuardMultiplier = " + controller.GetModel.GuardMultiplier);
            exposeCooldown--;
            if (exposeCooldown <= 0)
            {
                isExposed = false;
            }
        }
        else
        {
            controller.effectStartEvent -= ExposePlayer;
        }
    }

    //Bleed Deals damage at the end of the turn depending on the cooldown (e.g, cooldown is 2 meaning Damage = 2)
    public void BleedPlayer(object sender, EventArgs e)
    {
        if (isBleeding)
        {
            controller.ChangeHealth(-bleedCooldown);
            bleedCooldown--;
            if(bleedCooldown <= 0)
            {
                isBleeding = false;
                controller.effectEndEvent -= BleedPlayer;
            }
        }
    }

    //If the player is poison, then the player will decrease health at the end of their turn based on the amount of defence cards they have
    public void PoisonPlayer(object sender, EventArgs e)
    {
        if (isPoisoned)
        {
            defenceDeckPile defenceCards = GetComponentInChildren<defenceDeckPile>();
            controller.ChangeHealth(-defenceCards.DefenceCards.Count);
            Debug.Log("Poison Deals: " + defenceCards.DefenceCards.Count + " Damage");


            poisonCooldown--;
            if (poisonCooldown <= 0)
            {
                isPoisoned = false;
                controller.effectEndEvent -= PoisonPlayer;
            }

        }
    }

    //If the player is blistered, then the player will decrease health at the end of their turn based on the amount of movement cards they have
    public void BlisterPlayer(object sender, EventArgs e)
    {
        if (isBlistered)
        {
            movementDeckPile movementCards = GetComponentInChildren<movementDeckPile>();
            controller.ChangeHealth(-movementCards.MovementCards.Count);
            Debug.Log("Blister Deals: " + movementCards.MovementCards.Count + " Damage");


            blisterCooldown--;
            if (blisterCooldown <= 0)
            {
                isBlistered = false;
                controller.effectEndEvent -= BlisterPlayer;
            }

        }
    }

    //If the player is Unstabled, then the player will decrease health at the end of their turn based on the amount of status cards they have
    public void UnstablePlayer(object sender, EventArgs e)
    {
        if (isUnstabled)
        {
            statusDeckPile statusCards = GetComponentInChildren<statusDeckPile>();
            controller.ChangeHealth(-statusCards.StatusCards.Count);
            Debug.Log("Unstable Deals: " + statusCards.StatusCards.Count + " Damage");


            unstableCooldown--;
            if (unstableCooldown <= 0)
            {
                isUnstabled = false;
                controller.effectEndEvent -= UnstablePlayer;
            }

        }
    }

    //This ends the player's turn
    //In addition this prevents the player to defend
    public void StunPlayer(object sender, EventArgs e)
    {
        stunCooldown--;
        if (stunCooldown <= 0)
        {
            isStunned = false;
            controller.effectEndEvent -= StunPlayer;
        }
    }

    //Fear Decreases Thrust by 25%
    public void FearPlayer(object sender, EventArgs e)
    {
        if (isFeared)
        {
            controller.GetModel.ThrustMultiplier = 0.75f;
            Debug.Log("ThrustMultiplier = " + controller.GetModel.ThrustMultiplier);
            fearCooldown--;
            if (fearCooldown <= 0)
            {
                isFeared = false;
            }
        }
        else
        {
            controller.effectStartEvent -= FearPlayer;
        }
    }

    //Confuse randomly selects the player's movement, defence & offence card
    //The player cannot use abilities or status card whilst confused
    public void ConfusePlayer(object sender, EventArgs e)
    {
        confusedCooldown--;
        if (confusedCooldown <= 0)
        {
            isConfused = false;
            controller.effectEndEvent -= ConfusePlayer;
        }
    }

    //Blind randomly selects the player's direction
    public void BlindPlayer(object sender, EventArgs e)
    {
        blindCooldown--;
        if (blindCooldown <= 0)
        {
            isBlind = false;
            controller.effectEndEvent -= BlindPlayer;
        }
    }
}
