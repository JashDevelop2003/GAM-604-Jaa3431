using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class beamX : MonoBehaviour
{
    public static beamX instance;
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

    //Trigger Enter Method will be called when a collider from the collider has hit the beam's box collider
    //If it has then add that player to the list
    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            players.Add(player.gameObject);
        }
    }

    //Trigger Exit is the same method as enter, expecpt this time when a player leaves the beam's box collider
    //This will need to identify which player left and if it's the same then remove that player fro mthe list
    private void OnTriggerExit(Collider player)
    {

        for (int i = 0; i < players.Count; i++) 
        {
            if (player.gameObject == players[i])
            {
                players.Remove(players[i]);
            }
        }
    }
}
