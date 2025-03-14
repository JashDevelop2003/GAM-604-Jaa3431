using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statusCard : MonoBehaviour
{
    [SerializeField] private statusCardStats statCard;
    public statusCardStats StatusCard
    {
        get { return statCard; }
        set { statCard = value; }
    }

    [SerializeField] private targetEnum target;
    public targetEnum Target
    {
        get { return target; }
    }

    private effectEnum[] effect;
    public effectEnum[] Effect
    {
        get { return effect; }
    }

    private buffEnum[] buff;
    public buffEnum[] Buff
    {
        get { return buff; }
    }

    private float value;
    public float Value 
    { 
        get { return value; } 
    }

    private int effectCooldown;
    public int EffectCooldown
    {
        get { return effectCooldown; }
    }

    private int buffCooldown;
    public int BuffCooldown
    {
        get { return buffCooldown; }
    }

    [SerializeField] private int manaCost;
    public int ManaCost
    {
        get { return manaCost; }
        set { manaCost = value; }
    }

    public event EventHandler additionalEvent;

    //this method is called from either the picking state or the offence deck pool itself and provides the stats onto the object
    //This will then add the card into the offence deck to be used during the attack state
    public void CreateCard(statusCardStats newCard)
    {
        StatusCard = newCard;
        target = newCard.target;
        manaCost = statCard.manaCost;
        gameObject.name = statCard.name;
        target = newCard.target;
        effect = newCard.effect;
        buff = newCard.buff;
        effectCooldown = newCard.cooldown[0];
        buffCooldown = newCard.cooldown[1];
        value = newCard.value;
        if (newCard.additionalEffect != null)
        {
            Instantiate(newCard.additionalEffect, this.transform);
        }


        statusDeckPile deck = GetComponentInParent<statusDeckPile>();
        deck.AddCard(this.gameObject);
    }

    public void ActivateEffect(GameObject target)
    {
        //Add an event here for some non-status effects
        additionalEvent?.Invoke(this, EventArgs.Empty);

        foreach (buffEnum addBuff in buff)
        {
            if (addBuff != buffEnum.Null)
            {
                currentBuffs targetBuff = target.GetComponent<currentBuffs>();
                targetBuff.AddBuff(addBuff, buffCooldown, value);
                Debug.Log(target + " is " + addBuff);
            }
        }

        foreach (effectEnum addEffect in effect)
        {
            if (addEffect != effectEnum.Null)
            {
                currentEffects targetEffect = target.GetComponent<currentEffects>();
                targetEffect.AddEffect(addEffect, effectCooldown);
                Debug.Log(target + " is " + addEffect);
            }
        }
    }
}
