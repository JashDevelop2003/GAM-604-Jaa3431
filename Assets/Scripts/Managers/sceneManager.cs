using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This is the scene manager that will either change the scene or quit the game
/// </summary>

public class sceneManager : Singleton<sceneManager>
{
    public void ChangeScene(sceneEnum newScene)
    {
        Debug.Log("Changing Scene to: " + newScene);
        if(newScene != sceneEnum.Exit)
        {
            SceneManager.LoadScene((int)newScene);
        }
        else
        {
            Application.Quit();
        }
    }
}
