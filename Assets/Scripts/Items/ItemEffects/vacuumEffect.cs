using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vacuumEffect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private turnManager turnManager;
    [SerializeField] private GameObject player;
    private playerController controller;
    [SerializeField] private GameObject opponent;
    private playerController opponentController;
    private startState state;
    private int randomInt;

    // Upon pickup Vacuum does nothing
    void Awake()
    {
        turnManager = Singleton<turnManager>.Instance;
        playerTransform = this.transform.parent.parent.parent;
        player = playerTransform.gameObject;
        controller = player.GetComponent<playerController>();
        state = player.GetComponent<startState>();
        state.startItemEvents += Vacuum;
        StartCoroutine(WaitforLoad());
    }

    // At the start of player's turn steal 1 to 5 mana from opponent
    public void Vacuum(object sender, EventArgs e)
    {
        randomInt = UnityEngine.Random.Range(1, 5);
        //Double Negative
        controller.ChangeMana(-randomInt);
        opponentController.ChangeMana(randomInt);
    }

    private void OnDestroy()
    {
        state.startItemEvents -= Vacuum;
    }

    //Coroutine is used to make sure that the turn manager is enabled in the scene
    IEnumerator WaitforLoad()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < turnManager.GetPlayers.Length; i++)
        {
            if (turnManager.GetPlayers[i].gameObject != player.gameObject)
            {
                opponent = turnManager.GetPlayers[i].gameObject;
                opponentController = opponent.GetComponent<playerController>();
            }
        }
    }
}
