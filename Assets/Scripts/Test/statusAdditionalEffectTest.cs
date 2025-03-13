using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statusAdditionalEffectTest : MonoBehaviour
{
    private statusCard statusCard;
    private Transform playerTransform;
    private GameObject playerObject;


    // Start is called before the first frame update
    void Awake()
    {
        statusCard = GetComponentInParent<statusCard>();
        statusCard.additionalEvent += TestScript;
        //The transform is used to locate the player since the roll state doesn't mention the playrer's game object unlike the combat system
        playerTransform = this.transform.parent.parent.parent;
        playerObject = playerTransform.gameObject;
    }

    
    // Update is called once per frame
    public void TestScript(object sender, EventArgs e)
    {
        Debug.Log("This will be for Status Cards. These will be use ONLY for the current player");
    }

    void OnDisable()
    {
        statusCard.additionalEvent -= TestScript;
    }
}
