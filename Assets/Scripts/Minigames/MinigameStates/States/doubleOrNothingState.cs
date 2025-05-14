using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class doubleOrNothingState : gameStateBase, IRules, IConfirm, IDecideLeft, IDecideRight
{
    private gameControls gameControls;
    public gameControls GameControls
    {
        get { return gameControls; } 
        set { gameControls = value; }   
    }
    
    private bool backToRules;
    private choiceEnum currentChoice;
    private doubleOrNothingManager gameManager;
    private soundManager soundManager;

    [Header("User Interface")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Image[] choicePanels = new Image[2];
    [SerializeField] private Color[] panelColor = new Color[2];

    [Header ("Sound Effects")]
    [SerializeField] private AudioClip inputSound;

    public override void EnterState(gameStateManager player)
    {
        backToRules = false;
        gamePanel.SetActive(true);
        currentChoice = choiceEnum.Null;
        gameManager = Singleton<doubleOrNothingManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
        gameControls = GetComponent<gameControls>();
        EnableControls();

    }

    public override void UpdateState(gameStateManager player)
    {
        if (backToRules) 
        { 
            player.ChangeState(player.RuleState);
        }
    }

    public override void ExitState(gameStateManager player)
    {
        gamePanel.SetActive(false);
        DisableControls();
    }

    public void Rules(object sender, EventArgs e)
    {
        backToRules = true;
    }

    public void ConfirmingChoice(object sender, EventArgs e)
    {
        gameManager.Outcome(currentChoice);
        StartCoroutine(WaitForResults());
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        currentChoice = choiceEnum.Keep;
        ChoiceUI((int)currentChoice);
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        currentChoice = choiceEnum.Double;
        ChoiceUI((int)currentChoice);
    }

    public void ChoiceUI(int choice)
    {
        for (int i = 0; i < choicePanels.Length; i++)
        {
            if (i == choice)
            {
                choicePanels[i].color = panelColor[1];
            }
            else
            {
                choicePanels[i].color = panelColor[0];
            }
        }
    }

    public void InputSound(object sender, EventArgs e)
    {
        soundManager.PlaySound(inputSound);
    }

    IEnumerator WaitForResults()
    {
        DisableControls();
        yield return new WaitForSeconds(2);
        EnableControls();
    }

    public void EnableControls()
    {
        GameControls.pressedLeft += DecidingLeft;
        GameControls.pressedRight += DecidingRight;
        GameControls.pressedLeft += InputSound;
        GameControls.pressedRight += InputSound;
        GameControls.pressedConfirm += ConfirmingChoice;
        GameControls.pressedRules += Rules;
    }

    void DisableControls()
    {
        GameControls.pressedLeft -= DecidingLeft;
        GameControls.pressedRight -= DecidingRight;
        GameControls.pressedLeft -= InputSound;
        GameControls.pressedRight -= InputSound;
        GameControls.pressedConfirm -= ConfirmingChoice;
        GameControls.pressedRules -= Rules;
    }

}
