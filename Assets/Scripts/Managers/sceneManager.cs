using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneManager : Singleton<sceneManager>
{
    public void ChangeScene(sceneEnum newScene)
    {
        Debug.Log("Changing Scene to: " + newScene);
    }
}
