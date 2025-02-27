using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The movement deck pile is use to collect all of the available cards that are set active to true
/// The movement deck pile is also use to draw cards for the player to decide on
/// </summary>
public class movementDeckPile : MonoBehaviour
{
    //the list provides all the movement cards
    private List<GameObject> movementCards = new List<GameObject>();

    //the array int is use to collect the list and provide that card into the selected cards
    [SerializeField] private int[] moveCards = new int[3];

    //the array game object provides all the cards selected from the integers
    [SerializeField] private GameObject[] selectedCards = new GameObject[3];
    public GameObject[] SelectedCards
    {
        get { return selectedCards; }
    }

    // This method adds the movement card into the list
    public void AddCard(GameObject card)
    {
        movementCards.Add(card);
    }

    // This method draws 3 movement cards and prevent the card from being used more than expected
    public void DrawCards()
    {
        //each move card integer is selected from 0 to the amount of cards in the list
        for (int i = 0; i < moveCards.Length; i++)
        {
            moveCards[i] = Random.Range(0, movementCards.Count);
        }

        //if the second card's integer is the same as the first card integer then it needs to change
        //this loop occurs until the both the first and second card's integer are unique
        while (moveCards[1] == moveCards[0])
        {
            moveCards[1] = Random.Range(0, movementCards.Count);
        }

        //if the third card's integer is the same as the first or second card's integer then it needs to change
        //this loop occurs until the third card's integer is unique to the first and second card's integer
        while (moveCards[2] == moveCards[1] || moveCards[2] == moveCards[0])
        {
            moveCards[2] = Random.Range(0, movementCards.Count);
        }

        //once all integers are unique they add that card to the selcted card that will be used in the deciding state
        for (int i = 0;i < selectedCards.Length;i++) 
        {
            selectedCards[i] = movementCards[moveCards[i]];
        }
    }
}
