using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    public characterInfo[] characters;
    [SerializeField] characterEnum[] playerChoice = new characterEnum[2];

    private int player;
    private int currentCharacter;
    private bool mainMenu;

    private mainMenuControls menuControls;
    public mainMenuControls MenuControls
    {
        get { return menuControls; }
    }

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

    void Awake()
    {
        player = 0;
        currentCharacter = 0;
        mainMenu = false;
        sceneManager = Singleton<sceneManager>.Instance;
        menuControls = GetComponent<mainMenuControls>();
        MenuControls.pressedLeft += DecidingLeft;
        MenuControls.pressedRight += DecidingRight;
        MenuControls.pressedDown += MainMenu;
        MenuControls.pressedConfirm += ConfirmingChoice;
        ChangeCharacter();
        infoText.SetText("Player " + (player + 1).ToString() + " Select a Character. You can confirm by pressing Space. S, then Space will send you back to the main menu");

        characterSystem.Remove();
        saveSystem.NewGame();
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        currentCharacter--;
        if (currentCharacter < 0)
        {
            currentCharacter = characters.Length - 1;
        }
        mainMenu = false;
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
        ChangeCharacter();
    }

    public void MainMenu(object sender, EventArgs e)
    {
        mainMenu = true;
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
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
                }
            }
            if (sameCharacter)
            {
                infoText.SetText("Player has chosen this character, selected a new character");
                Debug.Log(sameCharacter);
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

    IEnumerator StartGame()
    {
        SelectedData selectedData = new SelectedData
        {
            playerOne = (int)playerChoice[0],
            playerTwo = (int)playerChoice[1],
        };
        characterSystem.Store(selectedData);

        MenuControls.pressedLeft -= DecidingLeft;
        MenuControls.pressedRight -= DecidingRight;
        MenuControls.pressedDown -= MainMenu;
        MenuControls.pressedConfirm -= ConfirmingChoice;
        infoText.SetText("Beginning Game at: " + scenes[1] + " board");
        yield return new WaitForSeconds(3);
        sceneManager.ChangeScene(scenes[1]);
    }

    private void OnDisable()
    {
        MenuControls.pressedLeft -= DecidingLeft;
        MenuControls.pressedRight -= DecidingRight;
        MenuControls.pressedDown -= MainMenu;
        MenuControls.pressedConfirm -= ConfirmingChoice;
    }
}
