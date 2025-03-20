using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamZ : MonoBehaviour
{
    public static beamZ instance;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    public List<GameObject> Players 
    { 
        get { return players; } 
    }

    //this is used to make this a singular instance of the component
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void OnTriggerEnter(Collider player)
    {
        if(player.gameObject.tag == "Player")
        {
            players.Add(player.gameObject);
        }
    }

    private void OnTriggerExit(Collider player)
    {
        foreach (GameObject removePlayer in players) 
        {
            if (player.gameObject == removePlayer) 
            { 
                players.Remove(removePlayer);
            }
        }
    }
}
