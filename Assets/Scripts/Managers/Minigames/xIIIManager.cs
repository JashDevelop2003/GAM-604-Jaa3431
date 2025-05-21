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
    //This invokes the end of the minigame and returns back to the game
    public event EventHandler endEvent;
    
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
    private bool[] fruitPlaced = new bool[5];

    //This have an event that will change the placement of the players
    public event EventHandler changeTurn;
    [SerializeField] private GameObject[] startingPlayer = new GameObject[2];
    private int startingPlayerInt;
    [SerializeField] private Color[] playerColour = new Color[3];

    //This will store each players' prize cash
    private int[] prizeCash = new int[2];

    //The turn manager will be required to get the players
    private turnManager turnManager;

    //the minigame manager will be required to end the minigame and return back to the board map
    private minigameManager minigameManager;

    [Header ("User Interface")]
    [SerializeField] private Sprite[] fruitSprites = new Sprite[5];
    [SerializeField] private TMP_Text[] cashPrizeText = new TMP_Text[2];
    [SerializeField] private TMP_Text infoText;

    [Header ("Sound Effects")]
    [SerializeField] private AudioClip[] outcomeSounds = new AudioClip[2];
    private soundManager soundManager;
    private musicManager musicManager;

    // Start is called before the first frame update
    public void BeginMinigame()
    {
        turnManager = Singleton<turnManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
        minigameManager = Singleton<minigameManager>.Instance;
        musicManager = Singleton<musicManager>.Instance;
        endEvent += minigameManager.EndMinigame;
        endEvent += musicManager.MinigameOver;
        for (int i = 0; i < prizeCash.Length; i++) 
        {
            prizeCash[i] = 0;
        }

        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].frontCardColour.color = playerColour[2];
            cards[i].fruit = fruitEnum.Null;
            cards[i].backCard.SetActive(true);
            cards[i].isRevealed = false;
        }
        StartCoroutine(OrganiseCards());
    }

    //The coroutine places each fruit one by one to ensure that there are no issues towards 2 fruits being stored in 1 card
    IEnumerator OrganiseCards()
    {
        PlaceFruits(fruitEnum.Cherries, fruits.cherries);
        yield return new WaitUntil(() => fruitPlaced[0] == true);
        PlaceFruits(fruitEnum.Lemon, fruits.lemons);
        yield return new WaitUntil(() => fruitPlaced[1] == true);
        PlaceFruits(fruitEnum.Grapes, fruits.grapes);
        yield return new WaitUntil(() => fruitPlaced[2] == true);
        PlaceFruits(fruitEnum.Watermelon, fruits.watermelon);
        yield return new WaitUntil(() => fruitPlaced[3] == true);
        PlaceFruits(fruitEnum.Coconut, fruits.coconut);
        yield return new WaitUntil(() => fruitPlaced[4] == true);
        StartCoroutine(BeginGame());
    }

    //This waits until the players are ready, once the players are ready the game will decide randomly who goes first
    //This will call the boolean to change state when is true become set to true via encapsulation
    IEnumerator BeginGame()
    {
        startingPlayerInt = UnityEngine.Random.Range(0, 2);
        ruleState ruleState = startingPlayer[startingPlayerInt].GetComponentInChildren<ruleState>();
        yield return new WaitUntil(() => ruleState.IsReady == true);
        xIIIInactiveState changeState = startingPlayer[startingPlayerInt].GetComponentInChildren<xIIIInactiveState>();
        changeState.StartingGame = true;
    }

    //Place[Fruit/Coconut] uses a paramerter to store the amount of fruits equal to the struct's integer of that fruit/coocnut
    //The outcome parameter chooses a random card from the struct and checks if the enumeration of the fruit enum inside of the card is null
    //the outcome keeps choosing a random card until the chosen struct card is null which will change the card to the suitable fruit.
    //Once all the amount of fruits are stored onto a card struct the boolean will turn true and move onto the next fruit/coconut to place down
    void PlaceFruits(fruitEnum fruit, int amount)
    {
        int outcome;

        for (int i = 0; i < amount; i++)
        {
            do
            {
                outcome = UnityEngine.Random.Range(0, cards.Length);
            }
            while (cards[outcome].fruit != fruitEnum.Null);
            cards[outcome].fruit = fruit;
            cards[outcome].fruitImage.sprite = fruitSprites[(int)fruit];
        }
        fruitPlaced[(int)fruit] = true;
    }

    //This method is called when the player confirms and doesn't skips
    //The card is revealed to the player to identify what they won/possibly lost
    public void RevealCard(int selectedCard, int player)
    {
        //The parameter of selected card will reveal the card the player has chosen
        //The int player will identifty who owns the card
        cards[selectedCard].backCard.SetActive(false);
        cards[selectedCard].frontCardColour.color = playerColour[player - 1];
        cards[selectedCard].isRevealed = true;

        //A Conditional statement is used to check if the card was a coconut
        //If the card is a coconut then the game is over and the current player loses all their prize cash
        //Otherwise the game will change the turns around
        if(cards[selectedCard].fruit != fruitEnum.Coconut)
        {
            if (cards[selectedCard].fruit == fruitEnum.Cherries)
            {
                prizeCash[player-1] += 10;
            }
            else if (cards[selectedCard].fruit == fruitEnum.Lemon)
            {
                prizeCash[player - 1] += 25;
            }
            else if (cards[selectedCard].fruit == fruitEnum.Grapes)
            {
                prizeCash[player - 1] += 50;
            }
            else if (cards[selectedCard].fruit == fruitEnum.Watermelon)
            {
                prizeCash[player - 1] += 100;
            }
            ChangeTurn();
            soundManager.PlaySound(outcomeSounds[1]);
        }
        else if (cards[selectedCard].fruit == fruitEnum.Coconut)
        {
            prizeCash[player - 1] = 0;
            infoText.SetText("Game Over: Player " + player.ToString() + " loses all of their cash prize");
            soundManager.PlaySound(outcomeSounds[0]);
            StartCoroutine(GameOver());
        }

        cashPrizeText[player - 1].SetText(prizeCash[player - 1].ToString());
    }

    //This method is called when revealing a fruit card
    //This method will invoke the change turn event which will change the turn of the active and inactive player
    public void ChangeTurn()
    {
        changeTurn?.Invoke(this, EventArgs.Empty);
    }

    //This Coroutine begins when the player finds the coconut
    //The scene manager will change the scene to the board map
    IEnumerator GameOver()
    {
        for(int i = 0; i < turnManager.GetPlayers.Length; i++)
        {
            playerController player = turnManager.GetPlayers[i].GetComponent<playerController>();
            player.ChangeCash(prizeCash[i]);
        }
        yield return new WaitForSeconds(3);
        endEvent?.Invoke(this, EventArgs.Empty);
        endEvent -= minigameManager.EndMinigame;
        endEvent -= musicManager.MinigameOver;
    }
}
