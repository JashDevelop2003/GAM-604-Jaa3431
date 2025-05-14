using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// XIII is a minigame that involves both players to pick cards to collect cash
/// The player gets certain amount of cash depending on the fruit they revealed:
/// - Cherries = 10 Cash
/// - Lemon = 25 Cash
/// - Grapes = 50 Cash
/// - Watermelon = 100 Cash
/// </summary>

//The card structs are used to display the fruit enum to check which fruit was revealed with a boolean to check if the bool to move onto the next card when selecting
[System.Serializable]
public struct Cards
{
    public fruitEnum fruit;
    public bool isRevealed;
    public Image fruitImage;
}

//The fruits are to display the amount of fruits in the game
[System.Serializable]
public struct Fruits
{
    public int cherries;
    public int lemons;
    public int grapes;
    public int watermelon;
    public int coconut;
}

public class xIIIManager : Singleton<xIIIManager>
{
    [SerializeField] private Cards[] cards = new Cards[13];
    [SerializeField] private Fruits fruits;
    private bool[] fruitPlaced = new bool[4];

    [Header ("User Interface")]
    [SerializeField] Sprite[] fruitSprites = new Sprite[5];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].fruit = fruitEnum.Null;
        }
        StartCoroutine(OrganiseCards());
    }

    
    IEnumerator OrganiseCards()
    {
        PlaceCherries(fruits.cherries);
        yield return new WaitUntil(() => fruitPlaced[0] == true);
        PlaceLemon(fruits.lemons);
        yield return new WaitUntil(() => fruitPlaced[1] == true);
        PlaceGrapes(fruits.grapes);
        yield return new WaitUntil(() => fruitPlaced[2] == true);
        PlaceWatermelon(fruits.watermelon);
        yield return new WaitUntil(() => fruitPlaced[3] == true);
        PlaceCoconut(fruits.coconut);
    }

    void PlaceCherries(int cherries)
    {
        int outcome;

        for(int i = 0; i < cherries; i++)
        {
            do
            {
                outcome = Random.Range(0, cards.Length);
            }
            while (cards[outcome].fruit != fruitEnum.Null);
            cards[outcome].fruit = fruitEnum.Cherries;
            cards[outcome].fruitImage.sprite = fruitSprites[(int)fruitEnum.Cherries];
        }
        fruitPlaced[0] = true;
    }

    void PlaceLemon(int lemon)
    {
        int outcome;

        for (int i = 0; i < lemon; i++)
        {
            do
            {
                outcome = Random.Range(0, cards.Length);
            }
            while (cards[outcome].fruit != fruitEnum.Null);
            cards[outcome].fruit = fruitEnum.Lemon;
            cards[outcome].fruitImage.sprite = fruitSprites[(int)fruitEnum.Lemon];
        }
        fruitPlaced[1] = true;
    }

    void PlaceGrapes(int grapes)
    {
        int outcome;

        for (int i = 0; i < grapes; i++)
        {
            do
            {
                outcome = Random.Range(0, cards.Length);
            }
            while (cards[outcome].fruit != fruitEnum.Null);
            cards[outcome].fruit = fruitEnum.Grapes;
            cards[outcome].fruitImage.sprite = fruitSprites[(int)fruitEnum.Grapes];
        }
        fruitPlaced[2] = true;
    }

    void PlaceWatermelon(int watermelon)
    {
        int outcome;

        for (int i = 0; i < watermelon; i++)
        {
            do
            {
                outcome = Random.Range(0, cards.Length);
            }
            while (cards[outcome].fruit != fruitEnum.Null);
            cards[outcome].fruit = fruitEnum.Watermelon;
            cards[outcome].fruitImage.sprite = fruitSprites[(int)fruitEnum.Watermelon];
        }
        fruitPlaced[3] = true;
    }

    void PlaceCoconut(int coconut) 
    {
        int outcome;

        for (int i = 0; i < coconut; i++)
        {
            do
            {
                outcome = Random.Range(0, cards.Length);
            }
            while (cards[outcome].fruit != fruitEnum.Null);
            cards[outcome].fruit = fruitEnum.Watermelon;
            cards[outcome].fruitImage.sprite = fruitSprites[(int)fruitEnum.Coconut];
        }
    }
}
