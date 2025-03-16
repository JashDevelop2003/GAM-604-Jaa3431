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


    ///Unlike the other additional effects, this only needs to be added into the status card's event
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += CursedCreation;
    }


    // Cursed Creation applies 3 random effects to an player for 4 turns
    public void CursedCreation(object sender, EventArgs e)
    {
        for (int i = 0; i < randomEffectInt.Length; i++)
        {
            //That is the amount of effects there are -1 (excludes the max integer)
            randomEffectInt[i] = UnityEngine.Random.Range(0, 12);
        }

        //This prevents similar effects occurring
        while (randomEffectInt[1] == randomEffectInt[0])
        {
            randomEffectInt[1] = UnityEngine.Random.Range(0, 12);
        }

        while (randomEffectInt[2] == randomEffectInt[1] || randomEffectInt[2] == randomEffectInt[0])
        {
            randomEffectInt[2] = UnityEngine.Random.Range(0, 12);
        }

        //This changes the effect into a new effect
        for(int i = 0;i < newEffects.Length; i++)
        {
            if (randomEffectInt[i] == 0)
            {
                newEffects[i] = effectEnum.Burned;
            }

            else if (randomEffectInt[i] == 1)
            {
                newEffects[i] = effectEnum.Shocked;
            }

            else if (randomEffectInt[i] == 2)
            {
                newEffects[i] = effectEnum.Exposed;
            }

            else if (randomEffectInt[i] == 3)
            {
                newEffects[i] = effectEnum.Bleeding;
            }

            else if (randomEffectInt[i] == 4)
            {
                newEffects[i] = effectEnum.Poison;
            }

            else if (randomEffectInt[i] == 5)
            {
                newEffects[i] = effectEnum.Blistered;
            }

            else if (randomEffectInt[i] == 6)
            {
                newEffects[i] = effectEnum.Unstabled;
            }

            else if (randomEffectInt[i] == 7)
            {
                newEffects[i] = effectEnum.Slowed;
            }

            else if (randomEffectInt[i] == 8)
            {
                newEffects[i] = effectEnum.Confused;
            }

            else if (randomEffectInt[i] == 9)
            {
                newEffects[i] = effectEnum.Feared;
            }

            else if (randomEffectInt[i] == 10)
            {
                newEffects[i] = effectEnum.Stunned;
            }

            else if (randomEffectInt[i] == 11)
            {
                newEffects[i] = effectEnum.Blind;
            }
        }

        //this changes the effect to the new effects
        statusCard.Effect = newEffects;
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= CursedCreation;
    }
}
