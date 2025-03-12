using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepComposureEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private defenceCard defenceCard;
    private combatSystem combatSystem;
    [SerializeField] private GameObject player;
    [SerializeField] private int attackValue;
    [SerializeField] private int defendValue;
    
    private void Awake()
    {
        defenceCard = GetComponentInParent<defenceCard>();
        combatSystem = combatSystem.instance;
        defenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent += KeepComposure;
        combatSystem.combatComplete += RemoveEffect;
    }

    public void KeepComposure(object sender, EventArgs e)
    {
        player = combatSystem.DefendingPlayer;
        attackValue = combatSystem.AttackValue;
        defendValue = combatSystem.DefendValue;
        if (defendValue > attackValue) 
        { 
            currentBuffs addBuff = player.GetComponent<currentBuffs>();
            addBuff.AddBuff(buffEnum.Resistant, 2, 0.05f);
            Debug.Log("Increase Resistant by 5%");
        }
        else
        {
            Debug.Log("No Increase");
        }
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.duringCombatEvent -= KeepComposure;
    }
}
