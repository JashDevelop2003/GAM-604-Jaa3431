using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class cursedCreation : MonoBehaviour
{
    private statusCard statusCard;
    private effectEnum[] newEffects = new effectEnum[3];
    private int[] randomEffectInt = new int[3];

    private List<effectEnum> possibleEffects = new List<effectEnum>()
    {
        effectEnum.Burned,
        effectEnum.Shocked,
        effectEnum.Exposed,
        effectEnum.Bleeding,
        effectEnum.Poison,
        effectEnum.Blistered,
        effectEnum.Unstabled,
        effectEnum.Slowed,
        effectEnum.Confused,
        effectEnum.Feared,
        effectEnum.Stunned,
        effectEnum.Blind,
    };


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += CursedCreation;
    }


    // Cursed Creation applies 3 random effects to an player for 4 turns
    public void CursedCreation(object sender, EventArgs e)
    {
        bool sameEffects;
        do
        {
            sameEffects = false;
            for (int i = 0; i < randomEffectInt.Length; i++)
            {
                //That is the amount of effects there are -1 (excludes the max integer)
                randomEffectInt[i] = UnityEngine.Random.Range(0, possibleEffects.Count);

                for (int j = 0; j < randomEffectInt.Length; j++)
                {
                    if (j != i && randomEffectInt[i] == randomEffectInt[j])
                    {
                        sameEffects = true;
                    }
                }
            }
        }
        while (sameEffects);

        //This changes the effect into a new effect
        for(int i = 0;i < newEffects.Length; i++)
        {
            newEffects[i] = possibleEffects[randomEffectInt[i]];
        }

        //this changes the effect to the new effects
        statusCard.Effect = newEffects;
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= CursedCreation;
    }
}
