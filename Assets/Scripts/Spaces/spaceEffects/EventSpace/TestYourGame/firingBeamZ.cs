using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// This should be called firing RookZ
/// </summary>

public class firingBeamZ : eventSpace
{
    [SerializeField] private List<GameObject> targetPlayers;
    [SerializeField] private GameObject beam;
    private beamZ beamZ;
    private spaceManager spaceManager;
    private soundManager soundManager;
    private turnManager turnManager;

    [SerializeField] private GameObject rook;
    private rookZ moveRook;

    [Header("User Interface")]
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip laserSound;

    void Start()
    {
        moveRook = rook.GetComponent<rookZ>();
        beamZ = beam.GetComponent<beamZ>();
        spaceManager = Singleton<spaceManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
        turnManager = Singleton<turnManager>.Instance;
    }

    public override void ActivateEvent()
    {
        eventText.SetText("Rook coming through! " + beam.name + " Anyone there loses 20 Health");
        targetPlayers = beamZ.Players;
        soundManager.PlaySound(laserSound);
        foreach (GameObject player in targetPlayers)
        { 
            playerController controller = player.GetComponent<playerController>();
            controller.ChangeHealth(-20);
        }

        targetPlayers = null;
        moveRook.MoveRook();
        StartCoroutine(EndTurn());
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(2);
        playerStateManager currentPlayer = turnManager.CurrentPlayer.GetComponent<playerStateManager>();
        currentPlayer.ChangeState(currentPlayer.InactiveState);
    }
}
