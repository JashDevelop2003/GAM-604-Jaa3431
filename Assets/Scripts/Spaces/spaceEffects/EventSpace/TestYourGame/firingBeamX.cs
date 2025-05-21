using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class firingBeamX : eventSpace
{
    [SerializeField] private List<GameObject> targetPlayers;
    [SerializeField] private GameObject beam;
    private beamX beamX;
    private spaceManager spaceManager;
    private soundManager soundManager;
    private turnManager turnManager;

    [Header("User Interface")]
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip laserSound;




    void Start()
    {
        beamX = beam.GetComponent<beamX>();
        spaceManager = Singleton<spaceManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
        turnManager = Singleton<turnManager>.Instance;
    }

    public override void ActivateEvent()
    {
        eventText.SetText("I'm firing my Laser at: " + beam.name + " Anyone there loses 20 Health");
        soundManager.PlaySound(laserSound);
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
