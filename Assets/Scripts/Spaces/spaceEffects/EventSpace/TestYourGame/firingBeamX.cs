using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firingBeamX : eventSpace
{
    [SerializeField] private List<GameObject> targetPlayers;
    [SerializeField] private GameObject beam;
    private beamX beamX;
    private spaceManager spaceManager;
    private turnManager turnManager;


    void Start()
    {
        beamX = beam.GetComponent<beamX>();
        spaceManager = spaceManager.instance;
        turnManager = Singleton<turnManager>.Instance;
    }

    public override void ActivateEvent()
    {
        Debug.Log("I'm firing my Laser at: " + beam.name);
        targetPlayers = beamX.Players;
        foreach (GameObject player in targetPlayers)
        {
            playerController controller = player.GetComponent<playerController>();
            controller.ChangeHealth(-20);
        }
        targetPlayers = null;
        StartCoroutine(EndTurn());
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(2);
        playerStateManager currentPlayer = turnManager.CurrentPlayer.GetComponent<playerStateManager>();
        currentPlayer.ChangeState(currentPlayer.InactiveState);
    }
}
