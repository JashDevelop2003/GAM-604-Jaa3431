using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementDeckPile : MonoBehaviour
{
    private List<GameObject> movementCards = new List<GameObject>();
    [SerializeField] private int[] moveCards = new int[3];
    [SerializeField] private GameObject[] selectedCards = new GameObject[3];
    public GameObject[] SelectedCards
    {
        get { return selectedCards; }
    }

    // Update is called once per frame
    public void AddCard(GameObject card)
    {
        movementCards.Add(card);
    }

    public void DrawCards()
    {
        for (int i = 0; i < moveCards.Length; i++)
        {
            moveCards[i] = Random.Range(0, movementCards.Count);
        }

        while (moveCards[1] == moveCards[0])
        {
            moveCards[1] = Random.Range(0, movementCards.Count);
        }

        while (moveCards[2] == moveCards[1] || moveCards[2] == moveCards[0])
        {
            moveCards[2] = Random.Range(0, movementCards.Count);
        }

        for (int i = 0;i < selectedCards.Length;i++) 
        {
            selectedCards[i] = movementCards[moveCards[i]];
        }
    }
}
