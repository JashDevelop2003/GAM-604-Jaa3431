using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitThrowEffect : MonoBehaviour
{
    //This requires the offence card to apply the effect to the suitable combat system event
    private offenceCard offenceCard;
    private combatSystem combatSystem;
    private GameObject player;

    ///This should be used for all additional effects
    void Awake()
    {
        offenceCard = GetComponentInParent<offenceCard>();
        combatSystem = combatSystem.instance;
        offenceCard.additionalEvent += AddEffect;
    }

    ///This should be used for all additional effects
    public void AddEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent += FruitThrow;
        combatSystem.combatComplete += RemoveEffect;
    }

    //Fruit Throw Heals 5 Health to Player
    public void FruitThrow(object sender, EventArgs e)
    {
        player = combatSystem.AttackingPlayer;
        playerController controller = player.GetComponent<playerController>();
        controller.ChangeHealth(5);
        combatSystem.OffenceValue.SetText("Player heals 5 Health");
    }

    ///This should be used for all additional effects
    public void RemoveEffect(object sender, EventArgs e)
    {
        combatSystem.afterCombatEvent -= FruitThrow;
        combatSystem.combatComplete -= RemoveEffect;
    }
}
