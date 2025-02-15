using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementDeckPile : MonoBehaviour
{
    public List<GameObject> movementCards = new List<GameObject>();
    [SerializeField] private int[] moveCards = new int[3];

    // Update is called once per frame
    public void AddCard(GameObject card)
    {
        movementCards.Add(card);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DrawCards();
        }
    }

    public void DrawCards()
    {

    }
}
