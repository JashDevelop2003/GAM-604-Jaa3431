using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The defence deck pile is use to collect all of the available cards that are set active to true
/// The defence deck pile is also use to draw cards for the player to decide on
/// </summary>

public class defenceDeckPile : MonoBehaviour
{
    private List<GameObject> defenceCards = new List<GameObject>();
    [SerializeField] private int[] defendCards = new int[4];
    [SerializeField] private GameObject[] selectedCards = new GameObject[4];

    public List<GameObject> DefenceCards
    {
        get { return defenceCards; }
    }
    public GameObject[] SelectedCards
    {
        get { return selectedCards; }
    }

    // This method adds the defence card into the list
    public void AddCard(GameObject card)
    {
        defenceCards.Add(card);
    }

    // This method draws 4 offence cards and prevent the card from being used more than expected
    public void DrawCards()
    {
        bool sameCards;
        do
        {
            sameCards = false;
            //This makes each integer random
            for (int i = 0; i < defendCards.Length; i++)
            {
                defendCards[i] = Random.Range(0, defenceCards.Count);

                //This for loop checks if there are two identical integers
                for (int j = 0; j < defendCards.Length; j++)
                {
                    //If there are 2 identical ints that isn't referring to itself then set the bool to true
                    if (j != i && defendCards[i] == defendCards[j])
                    {
                        sameCards = true;
                    }
                }
            }
        }
        while (sameCards);

        //once all integers are unique they add that card to the selcted card that will be used in the deciding state
        for (int i = 0; i < selectedCards.Length; i++)
        {
            selectedCards[i] = defenceCards[defendCards[i]];
        }
    }
}
