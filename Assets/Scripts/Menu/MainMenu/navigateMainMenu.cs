using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class navigateMainMenu : MonoBehaviour, IDecideUp, IDecideDown, IConfirm
{
    private mainMenuControls menuControls;
    private int currentChoice;
    public mainMenuControls MenuControls 
    { 
        get { return menuControls; }
    }

    [Header("Scene Management")]
    private sceneManager sceneManager;
    [SerializeField] private sceneEnum[] scene = new sceneEnum[5];
    
    [Header("User Interface")]
    [SerializeField] private Image[] choices = new Image[5];
    [SerializeField] private Color[] colourChoice = new Color[2];

    // Start is called before the first frame update
    void Awake()
    {
        currentChoice = 0;
        sceneManager = Singleton<sceneManager>.Instance;
        menuControls = GetComponent<mainMenuControls>();
        MenuControls.pressedUp += DecidingUp;
        MenuControls.pressedDown += DecidingDown;
        MenuControls.pressedConfirm += ConfirmingChoice;
        choices[currentChoice].color = colourChoice[1];
    }

    public void DecidingUp(object sender, EventArgs e)
    {
        currentChoice--;
        if (currentChoice < 0)
        {
            currentChoice = choices.Length - 1;
        }
        HighlightChoice();
    }

    public void DecidingDown(object sender, EventArgs e)
    {
        currentChoice++;
        if (currentChoice >= choices.Length)
        {
            currentChoice = 0;
        }
        HighlightChoice();
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        sceneManager.ChangeScene(scene[currentChoice]);
    }

    void HighlightChoice()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            if(i == currentChoice)
            {
                choices[i].color = colourChoice[1];
            }
            else
            {
                choices[i].color = colourChoice[0];
            }
        }
    }

    private void OnDisable()
    {
        MenuControls.pressedUp -= DecidingUp;
        MenuControls.pressedDown -= DecidingDown;
        MenuControls.pressedConfirm -= ConfirmingChoice;
    }
}
