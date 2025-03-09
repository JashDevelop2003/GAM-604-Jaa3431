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

    private effectEnum effect;
    public effectEnum Effect
    {
        get { return effect; }
    }

    private buffEnum buff;
    public buffEnum Buff
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

    [SerializeField] private int manaCost;
    public int ManaCost
    {
        get { return manaCost; }
        set { manaCost = value; }
    }

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
        effectCooldown = newCard.cooldown;
        value = newCard.value;
        

        statusDeckPile deck = GetComponentInParent<statusDeckPile>();
        deck.AddCard(this.gameObject);
    }

    public void ActivateEffect(GameObject target)
    {
        if (effect != effectEnum.Null) 
        {
            currentEffects targetEffect = target.GetComponent<currentEffects>();
            targetEffect.AddEffect(effect, effectCooldown);
        }

        if(buff != buffEnum.Null)
        {
            currentBuffs targetBuff = target.GetComponent<currentBuffs>();
            targetBuff.AddBuff(buff, effectCooldown, value);
        }

        //Add an event here for some non-status effects

        Debug.Log(target + " is " +  effect );
    }
}
