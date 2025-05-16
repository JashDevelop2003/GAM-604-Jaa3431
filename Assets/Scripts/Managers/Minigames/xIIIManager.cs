using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject backCard;
    public Image backCardColour;
    public Image frontCardColour;
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
    //Struct is used to store data of the cards & fruits to provide for each card
    [SerializeField] private Cards[] cards = new Cards[13];
    public Cards[] Cards
    {
        get { return cards; }
        set { cards = value; }
    }
    
    [SerializeField] private Fruits fruits;
    //The boolean is used to make the coroutine wait until the boolean in the array is set to true
    // 0 = Cherries are placed
    // 1 = Lemons are placed
    // 2 = Grapes are placed
    // 3 = Watermelon are placed
    private bool[] fruitPlaced = new bool[4];

    //This have an event that will change the placement of the players
    public event EventHandler changeTurn;
    [SerializeField] private GameObject[] startingPlayer = new GameObject[2];
    private int startingPlayerInt;
    [SerializeField] private Color[] playerColour = new Color[2];

    [Header ("User Interface")]
    [SerializeField] private Sprite[] fruitSprites = new Sprite[5];
    [SerializeField] private TMP_Text infoText;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].fruit = fruitEnum.Null;
            cards[i].backCard.SetActive(true);
            cards[i].isRevealed = false;
        }
        StartCoroutine(OrganiseCards());
    }

    //The coroutine places each fruit one by one to ensure that there are no issues towards 2 fruits being stored in 1 card
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
        StartCoroutine(BeginGame());
    }

    //This waits until the players are ready, once the players are ready the game will decide randomly who goes first
    IEnumerator BeginGame()
    {
        startingPlayerInt = UnityEngine.Random.Range(0, 2);
        ruleState ruleState = startingPlayer[startingPlayerInt].GetComponent<ruleState>();
        yield return new WaitUntil(() => ruleState.IsReady == true);
        xIIIInactiveState changeState = startingPlayer[startingPlayerInt].GetComponent<xIIIInactiveState>();
        changeState.StartingGame = true;
    }

    //Place[Fruit/Coconut] uses a paramerter to store the amount of fruits equal to the struct's integer of that fruit/coocnut
    //The outcome parameter chooses a random card from the struct and checks if the enumeration of the fruit enum inside of the card is null
    //the outcome keeps choosing a random card until the chosen struct card is null which will change the card to the suitable fruit.
    //Once all the amount of fruits are stored onto a card struct the boolean will turn true and move onto the next fruit/coconut to place down
    void PlaceCherries(int cherries)
    {
        int outcome;

        for(int i = 0; i < cherries; i++)
        {
            do
            {
                outcome = UnityEngine.Random.Range(0, cards.Length);
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
                outcome = UnityEngine.Random.Range(0, cards.Length);
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
                outcome = UnityEngine.Random.Range(0, cards.Length);
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
                outcome = UnityEngine.Random.Range(0, cards.Length);
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
                outcome = UnityEngine.Random.Range(0, cards.Length);
            }
            while (cards[outcome].fruit != fruitEnum.Null);
            cards[outcome].fruit = fruitEnum.Watermelon;
            cards[outcome].fruitImage.sprite = fruitSprites[(int)fruitEnum.Coconut];
        }
    }

    public void RevealCard(int selectedCard, int player)
    {
        cards[selectedCard].backCard.SetActive(false);
        cards[selectedCard].frontCardColour.color = playerColour[player - 1];
        cards[selectedCard].isRevealed = true;
        if(cards[selectedCard].fruit != fruitEnum.Coconut)
        {
            ChangeTurn();
        }
        else if (cards[selectedCard].fruit == fruitEnum.Coconut)
        {
            infoText.SetText("Game Over: Player " + player.ToString() + "loses all of their cash prize");
        }
    }

    public void ChangeTurn()
    {
        changeTurn?.Invoke(this, EventArgs.Empty);
    }
}
