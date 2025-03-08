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
        //each move card integer is selected from 0 to the amount of cards in the list
        for (int i = 0; i < attackCards.Length; i++)
        {
            attackCards[i] = Random.Range(0, offenceCards.Count);
        }

        //if the second card's integer is the same as the first card integer then it needs to change
        //this loop occurs until the both the first and second card's integer are unique
        while (attackCards[1] == attackCards[0])
        {
            attackCards[1] = Random.Range(0, offenceCards.Count);
        }

        //if the third card's integer is the same as the first or second card's integer then it needs to change
        //this loop occurs until the third card's integer is unique to the first and second card's integer
        while (attackCards[2] == attackCards[1] || attackCards[2] == attackCards[0])
        {
            attackCards[2] = Random.Range(0, offenceCards.Count);
        }

        //if the fourth card's integer is the same as the first, second or third card's integer then it needs to change
        //this loop occurs until the fourth card's integer is unique to the first, second and third card's integer
        while (attackCards[3] == attackCards[2] || attackCards[3] == attackCards[1] || attackCards[3] == attackCards[0])
        {
            attackCards[3] = Random.Range(0, offenceCards.Count);
        }

        //once all integers are unique they add that card to the selcted card that will be used in the deciding state
        for (int i = 0; i < selectedCards.Length; i++)
        {
            selectedCards[i] = offenceCards[attackCards[i]];
        }
    }
}
