using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sevenSevenSevenEffect : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject player;
    /// <summary>
    /// hasCard[0] = Godmother's Finale
    /// hasCard[1] = Miss Fortunate
    /// hasCard[2] = Money Dice
    /// </summary>
    [SerializeField] private bool[] hasCard = new bool [3];


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += SevenSevenSeven;
        //The transform is used to locate the player since the additional effects for status cards must only apply to the player
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
    }


    // Green & Whealthy Increases Health by 10% of Current Cash currently the player has
    public void SevenSevenSeven(object sender, EventArgs e)
    {
        if (!hasCard[0]) 
        { 
            offenceDeckPool offenceDeck = player.GetComponentInChildren<offenceDeckPool>();
            for (int i = 0; i < offenceDeck.OffenceCard.Count; i++) 
            {
                if (offenceDeck.OffenceCard[i].name == "Godmother's Finale")
                {
                    hasCard[0] = true;
                }
            }
        }

        if (!hasCard[1]) 
        { 
            defenceDeckPool defenceDeck = player.GetComponentInChildren<defenceDeckPool>();
            for(int i = 0; i < defenceDeck.DefenceCard.Count; i++)
            {
                if (defenceDeck.DefenceCard[i].name == "Miss Fortunate")
                {
                    hasCard[1] = true;
                }
            }
        }

        if (!hasCard[2])
        {
            movementDeckPool movementDeck = player.GetComponentInChildren<movementDeckPool>();
            for (int i = 0; i < movementDeck.MovementCard.Count; i++)
            {
                if (movementDeck.MovementCard[i].name == "Money Dice")
                {
                    hasCard[2] = true;
                }
            }
        }

        if (hasCard[0] && hasCard[1] && hasCard[2]) 
        { 
            currentBuffs buffs = player.GetComponent<currentBuffs>();
            playerController controller = GetComponent<playerController>();
            buffs.AddBuff(buffEnum.Resistant, 7, 0.77f);
            buffs.AddBuff(buffEnum.Impactful, 7, 0.77f);
        }
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= SevenSevenSeven;
    }
}
