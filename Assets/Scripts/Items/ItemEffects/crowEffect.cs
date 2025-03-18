using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crowEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    private startState state;

    // Upon pickup crow does nothing
    void Awake()
    {
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        state = player.GetComponent<startState>();
        state.startEvent += Crow;
        Debug.Log("The crow is in the player's inventory");
    }

    // At the start of the player's turn the crow does nothing but it will start making sound
    public void Crow(object sender, EventArgs e)
    {
        Debug.Log("caw");
    }

    private void OnDestroy()
    {
        state.startEvent -= Crow;
    }
}
