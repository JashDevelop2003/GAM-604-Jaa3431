using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Diagnostics.CodeAnalysis;
/// <summary>
/// Select Character is when the players chooses their characters
/// The component is required to prevent the player to choose the same character as the other player
/// The component is also required to create game data and suitable character data depending on character the player has selected.
/// </summary>

//A struct is used to store information about the character to provide detail on their name, stats and abilities.
[System.Serializable]
public struct characterInfo
{
    public characterEnum character;
    public string name;
    public characterData characterData;
    public string[] abilityNames;
    public string[] abilityDescriptions;
}
public class selectCharacter : MonoBehaviour, IDecideLeft, IDecideRight, IConfirm, IMenu
{
    //An array of character information is used to store all the playable characters in the game
    //The enums are the characters that the player have selected
    public characterInfo[] characters;
    [SerializeField] characterEnum[] playerChoice = new characterEnum[2];

    //the player int is used to identify which player is currently selecting
    //The current character int is used to identify the current character the player is currently on
    //The boolean is to check if the player wants to go back to main menu when confirming
    private int player;
    private int currentCharacter;
    private bool mainMenu;

    private mainMenuControls menuControls;
    public mainMenuControls MenuControls
    {
        get { return menuControls; }
    }

    //There are 2 scenes that character selection can transition to:
    // Array 0 = Main Menu
    // Arrau 1 = Test Your Board Map
    [Header("Scene Manager")]
    [SerializeField] private sceneEnum[] scenes = new sceneEnum[2];
    private sceneManager sceneManager;

    [Header("User Interface")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text characterName;
    //Stat Text Array Detail
    // 0 = Health
    // 1 = Cash
    // 2 = Mana
    // 3 = Offence
    // 4 = Defence
    // 5 = Movement
    // 6 = Status
    // 7 = Item
    [SerializeField] private TMP_Text[] statText = new TMP_Text[8];
    [SerializeField] private TMP_Text[] abilityName = new TMP_Text[2];
    [SerializeField] private TMP_Text[] abilityDescription = new TMP_Text[2];
    [SerializeField] private Image characterIcon;
    [SerializeField] private Image[] playerIcon = new Image[2];

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] characterSounds = new AudioClip[4];
    private soundManager soundManager;

    void Awake()
    {
        player = 0;
        currentCharacter = 0;
        mainMenu = false;
        sceneManager = Singleton<sceneManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
        menuControls = GetComponent<mainMenuControls>();
        MenuControls.pressedLeft += DecidingLeft;
        MenuControls.pressedRight += DecidingRight;
        MenuControls.pressedDown += MainMenu;
        MenuControls.pressedConfirm += ConfirmingChoice;
        ChangeCharacter();
        infoText.SetText("Player " + (player + 1).ToString() + " Select a Character. You can confirm by pressing Space. S, then Space will send you back to the main menu");

        //When starting a new game all existing data must be removed to start fresh in the game
        characterSystem.Remove();
        saveSystem.NewGame();
        stanceSystem.Remove();
        luckSystem.Remove();
        ruthlessSystem.Remove();
        luckOutcomeSystem.Remove();
    }

    //When pressing A or D, the player goes to the next or previous character
    public void DecidingLeft(object sender, EventArgs e)
    {
        currentCharacter--;
        if (currentCharacter < 0)
        {
            currentCharacter = characters.Length - 1;
        }
        mainMenu = false;
        PlaySound(currentCharacter);
        ChangeCharacter();
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        currentCharacter++;
        if (currentCharacter >= characters.Length)
        {
            currentCharacter = 0;
        }
        mainMenu = false;
        PlaySound(currentCharacter);
        ChangeCharacter();
    }

    //when pressing S, the player is currently choosing to go back to menu
    public void MainMenu(object sender, EventArgs e)
    {
        mainMenu = true;
    }

    //when confirming the player checks for 2 things
    // 1. If the player wants to leave and return back to main menu
    // 2. (Player 2 Only) checks if the other player has chosen that character
    public void ConfirmingChoice(object sender, EventArgs e)
    {
        //If the main menu boolean is true then return to main menu
        if (mainMenu)
        {
            sceneManager.ChangeScene(scenes[0]);
        }
        else
        {
            bool sameCharacter = false;
            for (int i = 0; i < playerChoice.Length; i++) 
            {
                if (playerChoice[i] == characters[currentCharacter].character) 
                { 
                    sameCharacter = true;
                    PlaySound(currentCharacter);
                }
            }
            if (sameCharacter)
            {
                infoText.SetText("Player has chosen this character, selected a new character");
                sameCharacter = false;
            }

            else
            {
                playerChoice[player] = characters[currentCharacter].character;
                playerIcon[player].sprite = characters[currentCharacter].characterData.abilityIcon[0];
                playerIcon[player].color = characters[currentCharacter].characterData.abilityColour[0];

                if (playerChoice[playerChoice.Length - 1] != characterEnum.Null)
                {
                    StartCoroutine(StartGame());
                }
                else
                {
                    player++;
                    infoText.SetText("Player " + (player + 1).ToString() + " Select a Character. You can confirm by pressing Space. S, then Space will send you back to the main menu");
                }
            }
        }
    }

    //Change chararacter method will update the current character the player is currently hovering on to provide the name, stats and abilities
    void ChangeCharacter()
    {
        characterName.SetText(characters[currentCharacter].name);
        for (int i = 0; i < statText.Length; i++) 
        {
            if (i == 0)
            {
                statText[i].SetText(characters[currentCharacter].characterData.Health.ToString());
            }
            else if (i == 1)
            {
                statText[i].SetText(characters[currentCharacter].characterData.startingCash.ToString());
            }
            else if (i == 2)
            {
                statText[i].SetText(characters[currentCharacter].characterData.startingCash.ToString());
            }
            else if(i >= 3 && i <= 7)
            {
                statText[i].SetText(characters[currentCharacter].characterData.deckCapacity[i - 3].ToString());
            }
        }

        for (int i = 0; i < abilityName.Length; i++) 
        {
            abilityName[i].SetText(characters[currentCharacter].abilityNames[i]);
            abilityDescription[i].SetText(characters[currentCharacter].abilityDescriptions[i]);
        }

        characterIcon.sprite = characters[currentCharacter].characterData.abilityIcon[0];
        characterIcon.color = characters[currentCharacter].characterData.abilityColour[0];

    }

    //The Coroutine starts once the last player has chosen their character, which will create suitable data files for the game
    IEnumerator StartGame()
    {
        //Selected Data will store the character the players have chosen and when loading in the game, the player controller will retrieve the suitable character data
        SelectedData selectedData = new SelectedData
        {
            playerOne = (int)playerChoice[0],
            playerTwo = (int)playerChoice[1],
        };
        characterSystem.Store(selectedData);

        //The coroutine then checks if the player is anyone besides the grim reaper to store suitable data

        //Playing as double wielder will require the game to store the current stance
        if (playerChoice[1] == characterEnum.Wielder || playerChoice[0] == characterEnum.Wielder) 
        {
            StanceData stanceData = new StanceData
            {
                stance = UnityEngine.Random.Range(0, (int)stanceEnum.Aggressive + 1),
                stanceCooldown = 3,
            };
            stanceSystem.Store(stanceData);
        }

        //Playing as Lucky Gambler will require the game to store the random mana and outcome
        else if (playerChoice[1] == characterEnum.Gambler || playerChoice[0] == characterEnum.Gambler)
        {
            int mana = UnityEngine.Random.Range(1, 31);

            LuckData luckData = new LuckData
            {
                storedMana = mana,
                randomisedMana = mana,
            };
            luckSystem.Store(luckData);

            int outcome =  UnityEngine.Random.Range(1, 7);
            LuckOutcomeData outcomeData = new LuckOutcomeData
            {
                luckOutcome = outcome
            };
            luckOutcomeSystem.Store(outcomeData);
        }

        //Playing as Robotic Superstar will require the game to store the active ability of being retaliating
        else if (playerChoice[1] == characterEnum.Superstar || playerChoice[0] == characterEnum.Superstar)
        {

            RuthlessData ruthlessData = new RuthlessData
            {
                retaliating = false,
            };

            ruthlessSystem.Store(ruthlessData);
        }

        //Once the data is stored, the coroutine will then start the game by calling the scene manager to transition to the board map
        MenuControls.pressedLeft -= DecidingLeft;
        MenuControls.pressedRight -= DecidingRight;
        MenuControls.pressedDown -= MainMenu;
        MenuControls.pressedConfirm -= ConfirmingChoice;
        infoText.SetText("Beginning Game at: " + scenes[1] + " board");
        yield return new WaitForSeconds(3);
        sceneManager.ChangeScene(scenes[1]);
    }

    void PlaySound(int character)
    {
        soundManager.PlaySound(characterSounds[character]);
    }

    private void OnDisable()
    {
        MenuControls.pressedLeft -= DecidingLeft;
        MenuControls.pressedRight -= DecidingRight;
        MenuControls.pressedDown -= MainMenu;
        MenuControls.pressedConfirm -= ConfirmingChoice;
    }
}
