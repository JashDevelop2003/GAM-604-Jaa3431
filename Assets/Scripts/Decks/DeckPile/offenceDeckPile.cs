using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The offence deck pile is use to collect all of the available cards that are set active to true
/// The offence  deck pile is also use to draw cards for the player to decide on
/// </summary>
public class offenceDeckPile : MonoBehaviour
{
    private List<GameObject> offenceCards = new List<GameObject>();
    [SerializeField] private int[] attackCards = new int[4];
    [SerializeField] private GameObject[] selectedCards = new GameObject[4];
    
    public List<GameObject> OffenceCards
    {
        get { return offenceCards; }     
    }

    public GameObject[] SelectedCards
    {
        get { return selectedCards; }
    }

    // This method adds the offence card into the list
    public void AddCard(GameObject card)
    {
        offenceCards.Add(card);
    }

    // This method draws 4 offence cards and prevent the card from being used more than expected
    public void DrawCards()
    {
        bool sameCards;
        do
        {
            sameCards = false;
            //This makes each integer random
            for (int i = 0; i < attackCards.Length; i++)
            {
                attackCards[i] = Random.Range(0, offenceCards.Count);

                //This for loop checks if there are two identical integers
                for(int j = 0; j < attackCards.Length; j++)
                {
                    //If there are 2 identical ints that isn't referring to itself then set the bool to true
                    if(j != i && attackCards[i] == attackCards[j])
                    {
                        sameCards = true;
                    }
                }
            }
        }
        while (sameCards);
    }
}
