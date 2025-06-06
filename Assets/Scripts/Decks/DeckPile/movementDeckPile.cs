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

    public List<GameObject> MovementCards
    {
        get { return movementCards; }
    }

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
        bool sameCards;
        do
        {
            sameCards = false;
            //This makes each integer random
            for (int i = 0; i < moveCards.Length; i++)
            {
                moveCards[i] = Random.Range(0, movementCards.Count);

                //This for loop checks if there are two identical integers
                for (int j = 0; j < moveCards.Length; j++)
                {
                    //If there are 2 identical ints that isn't referring to itself then set the bool to true
                    if (j != i && moveCards[i] == moveCards[j])
                    {
                        sameCards = true;
                    }
                }
            }
        }
        while (sameCards);
    }
}
