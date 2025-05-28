using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTwoData : MonoBehaviour
{
    private dataManager dataManager;
    private Transform locatePlayer;
    private GameObject player;

    public int healthCurrent;
    public int healthMax;
    public int manaCurrent;
    public int manaMax;
    public bool usedAbility;
    public int cashCurrent;

    public int currentPath;
    public int spaceInt;

    public List<int> storedOffence;
    public List<int> storedDefence;
    public List<int> storedMovement;
    public List<int> storedStatus;
    public List<int> storedItems;

    public int[] effectCooldown;
    public int[] buffCooldown;

    void Awake()
    {
        locatePlayer = this.transform.parent;
        player = locatePlayer.gameObject;
        dataManager = Singleton<dataManager>.Instance;
        dataManager.loadFiles += LoadPlayer;
        dataManager.saveFiles += SavePlayer;
    }

    public void LoadPlayer(object sender, EventArgs e)
    {

    }

    public void SavePlayer(object sender, EventArgs e)
    {

    }

    //TODO - Change return null to yield return new WaitUntil( Suitable Boolean becomes true)
    IEnumerator LoadingData()
    {
        return null;
    }

    private void OnDisable()
    {
        dataManager.loadFiles -= LoadPlayer;
        dataManager.saveFiles -= SavePlayer;
    }
}
