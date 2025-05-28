using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allPaths : MonoBehaviour
{
    public static allPaths instance;

    [SerializeField] private GameObject[] paths;
    public GameObject[] Paths
    {
        get { return paths; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
