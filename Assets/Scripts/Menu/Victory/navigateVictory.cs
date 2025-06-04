using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class navigateVictory : MonoBehaviour, IDecideLeft, IDecideRight, IConfirm
{
    private mainMenuControls menuControls;
    private int currentChoice;
    public mainMenuControls MenuControls
    {
        get { return menuControls; }
    }

    [Header("Scene Management")]
    private sceneManager sceneManager;
    [SerializeField] private sceneEnum[] scene = new sceneEnum[2];

    [Header("User Interface")]
    [SerializeField] private TMP_Text inputText;
    [SerializeField] private string[] displayText = new string[2];

    // Start is called before the first frame update
    void Awake()
    {
        currentChoice = 0;
        sceneManager = Singleton<sceneManager>.Instance;
        menuControls = GetComponent<mainMenuControls>();
        MenuControls.pressedLeft += DecidingLeft;
        MenuControls.pressedRight += DecidingRight;
        MenuControls.pressedConfirm += ConfirmingChoice;

        characterSystem.Remove();
        saveSystem.NewGame();
        stanceSystem.Remove();
        luckSystem.Remove();
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        currentChoice = 0;
        ChangeText();
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        currentChoice = 1;
        ChangeText();
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        sceneManager.ChangeScene(scene[currentChoice]);
    }

    void ChangeText()
    {
        inputText.SetText(displayText[currentChoice]);
    }

    private void OnDisable()
    {
        MenuControls.pressedLeft -= DecidingLeft;
        MenuControls.pressedRight -= DecidingRight;
        MenuControls.pressedConfirm -= ConfirmingChoice;
    }
}
