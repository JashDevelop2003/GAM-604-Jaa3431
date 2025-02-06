using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInput : MonoBehaviour
{
    [SerializeField] private boardControls boardControls;
    
    // Start is called before the first frame update
    void Start()
    {
        boardControls.upPressed += ItWorks;
    }

    private void OnEnable()
    {
        boardControls.upPressed += ItWorks;
    }

    // Update is called once per frame
    public void ItWorks(object sender, EventArgs e)
    {
        Debug.Log("Test Complete");
    }

    private void OnDisable()
    {
        boardControls.upPressed -= ItWorks;
    }
}
