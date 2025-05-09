using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navigateCredits : MonoBehaviour, IMenu
{
    private mainMenuControls menuControls;
    private int currentChoice;
    public mainMenuControls MenuControls
    {
        get { return menuControls; }
    }

    [Header("Scene Management")]
    private sceneManager sceneManager;
    [SerializeField] private sceneEnum scene;

    // Start is called before the first frame update
    void Awake()
    {
        sceneManager = Singleton<sceneManager>.Instance;
        menuControls = GetComponent<mainMenuControls>();
        MenuControls.pressedConfirm += MainMenu;
    }

    public void MainMenu(object sender, EventArgs e)
    {
        sceneManager.ChangeScene(scene);
    }

    private void OnDisable()
    {
        MenuControls.pressedConfirm -= MainMenu;
    }
}
