using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPrefabGithub : MonoBehaviour
{
    [SerializeField] private characterData testData;
    
    // Start is called before the first frame update
    void Start()
    {
        if (testData != null)
        {
            Debug.Log("No Issues with github.ignore");

        }

        else
        {
            Debug.LogError("Something is wrong with the github.ignore");
        }
    }

}
