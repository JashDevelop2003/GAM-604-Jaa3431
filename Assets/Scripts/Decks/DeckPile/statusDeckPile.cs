using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The status deck pile is use to collect all of the available cards that are set active to true
/// The status deck pile is also use to draw cards for the player to decide on
/// </summary>

public class statusDeckPile : MonoBehaviour
{
    private List<GameObject> statusCards = new List<GameObject>();
    [SerializeField] private int statusCardInt;
    [SerializeField] private GameObject selectedCard;

    public List<GameObject> StatusCards
    {
        get { return statusCards; }
    }
    public GameObject SelectedCard
    {
        get { return selectedCard; }
        set { selectedCard = value; }
    }

    // This method adds the offence card into the list
    public void AddCard(GameObject card)
    {
        statusCards.Add(card);
    }

    public void DrawCard()
    {
        statusCardInt = Random.Range(0, statusCards.Count);
        selectedCard = statusCards[statusCardInt];
    }
}
