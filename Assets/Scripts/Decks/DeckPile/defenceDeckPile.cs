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
        //each move card integer is selected from 0 to the amount of cards in the list
        for (int i = 0; i < defendCards.Length; i++)
        {
            defendCards[i] = Random.Range(0, defenceCards.Count);
        }

        //if the second card's integer is the same as the first card integer then it needs to change
        //this loop occurs until the both the first and second card's integer are unique
        while (defendCards[1] == defendCards[0])
        {
            defendCards[1] = Random.Range(0, defenceCards.Count);
        }

        //if the third card's integer is the same as the first or second card's integer then it needs to change
        //this loop occurs until the third card's integer is unique to the first and second card's integer
        while (defendCards[2] == defendCards[1] || defendCards[2] == defendCards[0])
        {
            defendCards[2] = Random.Range(0, defenceCards.Count);
        }

        //if the fourth card's integer is the same as the first, second or third card's integer then it needs to change
        //this loop occurs until the fourth card's integer is unique to the first, second and third card's integer
        while (defendCards[3] == defendCards[2] || defendCards[3] == defendCards[1] || defendCards[3] == defendCards[0])
        {
            defendCards[3] = Random.Range(0, defenceCards.Count);
        }

        //once all integers are unique they add that card to the selcted card that will be used in the deciding state
        for (int i = 0; i < selectedCards.Length; i++)
        {
            selectedCards[i] = defenceCards[defendCards[i]];
        }
    }
}
