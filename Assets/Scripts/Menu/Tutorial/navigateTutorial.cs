using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class navigateTutorial : MonoBehaviour, IMenu, IDecideLeft, IDecideRight 
{
    private mainMenuControls menuControls;
    private int currentChoice;
    public mainMenuControls MenuControls
    {
        get { return menuControls; }
    }

    [SerializeField] private int currentStep;
    [SerializeField] int maxSteps;
    public event EventHandler stepEvent;

    //Changing the player's position
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spaces = new Transform[10];

    [Header("Scene Management")]
    private sceneManager sceneManager;
    [SerializeField] private sceneEnum scene;

    [Header("User Interface")]
    [SerializeField] private GameObject[] panels = new GameObject[5];
    [SerializeField] private TMP_Text description;
    [SerializeField] private string[] descriptions = new string[15];

    // Start is called before the first frame update
    void Awake()
    {
        currentStep = 0;

        sceneManager = Singleton<sceneManager>.Instance;
        menuControls = GetComponent<mainMenuControls>();
        MenuControls.pressedLeft += DecidingLeft;
        MenuControls.pressedRight += DecidingRight;
        MenuControls.pressedConfirm += MainMenu;

        stepEvent += ChangeDescription;
        stepEvent += ChangePanel;
        stepEvent += ChangePosition;
    }

    public void DecidingLeft(object sender, EventArgs e)
    {
        if(currentStep > 0)
        {
            currentStep--;
            stepEvent?.Invoke(this, e);
        }
    }

    public void DecidingRight(object sender, EventArgs e)
    {
        if (currentStep < maxSteps)
        {
            currentStep++;
            stepEvent?.Invoke(this, e);
        }
    }

    public void MainMenu(object sender, EventArgs e)
    {
        sceneManager.ChangeScene(scene);
    }

    public void ChangeDescription(object sender, EventArgs e)
    {
        if(currentStep <= descriptions.Length - 1)
        {
            description.SetText(descriptions[currentStep]);
        }
    }

    public void ChangePanel(object sender, EventArgs e)
    {
        for (int i = 0; i < panels.Length; i++) 
        {
            panels[i].SetActive(false);
            if (currentStep <= 14)
            {
                panels[0].SetActive(true);
            }
            else if (currentStep == 15)
            {
                panels[1].SetActive(true);
            }
            else if (currentStep == 16)
            {
                panels[2].SetActive(true);
            }
            else if (currentStep == 17)
            {
                panels[3].SetActive(true);
            }
            else if(currentStep == 18)
            {
                panels[4].SetActive(true);
            }
        }
    }

    public void ChangePosition(object sender, EventArgs e)
    {
        if(currentStep >= 3 && currentStep <= 12)
        {
            player.transform.position = new Vector3(spaces[currentStep - 3].position.x, 0.5f, 0f);
        }
    }

    private void OnDisable()
    {
        MenuControls.pressedLeft -= DecidingLeft;
        MenuControls.pressedRight -= DecidingRight;
        MenuControls.pressedConfirm -= MainMenu;
        stepEvent -= ChangeDescription;
        stepEvent -= ChangePanel;
        stepEvent -= ChangePosition;
    }
}
