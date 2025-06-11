using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// This should be called firing RookX
/// </summary>

public class firingBeamX : eventSpace
{
    [SerializeField] private List<GameObject> targetPlayers;
    [SerializeField] private GameObject beam;
    private beamX beamX;
    private spaceManager spaceManager;
    private soundManager soundManager;
    private turnManager turnManager;

    [SerializeField] private GameObject rook;
    private rookX moveRook;

    [Header("User Interface")]
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip laserSound;




    void Start()
    {
        moveRook = rook.GetComponent<rookX>();
        beamX = beam.GetComponent<beamX>();
        spaceManager = Singleton<spaceManager>.Instance;
        soundManager = Singleton<soundManager>.Instance;
        turnManager = Singleton<turnManager>.Instance;
    }

    public override void ActivateEvent()
    {
        eventText.SetText("Rook coming through! Anyone there loses 20 Health");
        soundManager.PlaySound(laserSound);
        targetPlayers = beamX.Players;
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
