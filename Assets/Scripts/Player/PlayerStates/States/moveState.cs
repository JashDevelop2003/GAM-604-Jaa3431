using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class moveState : playerStateBase
{
    private int movement;
    private float speed = 2f;
    private Ray detectRay;
    private Vector3 rayPosition;
    private bool movementEnd;
    private playerController controller;
    private Coroutine movePlayer;

    private GameObject currentSpace;
    private spaceBehaviour spaceBehaviour;
    private spaceEnum currentSpaceType;
    private GameObject targetSpace;
    private Vector3 spacePosition;
    
    public override void EnterState(playerStateManager player)
    {
        movementEnd = false;

        controller = GetComponent<playerController>();
        movement = controller.GetModel.RollValue;

        currentSpace = controller.Space;
        spaceBehaviour = currentSpace.GetComponent<spaceBehaviour>();
        targetSpace = spaceBehaviour.NextSpace;
        spacePosition = new Vector3(targetSpace.transform.position.x, 2f, targetSpace.transform.position.z);


        movePlayer = StartCoroutine(Moving());
    }

    public override void UpdateState(playerStateManager player)
    {
        //var moveSpeed = speed * Time.deltaTime;
       // transform.position = Vector3.MoveTowards(transform.position, spacePosition, moveSpeed);

        if (movementEnd)
        {
            Debug.LogWarning("Needs Space Behaviour");
        }


    }

    private void FixedUpdate()
    {
        rayPosition = new Vector3(transform.position.x,  transform.position.y - 0.5f, transform.position.z);

        detectRay = new Ray(rayPosition, -transform.up);
    }

    public override void ExitState(playerStateManager player) 
    {
        Debug.Log("Will need to have an exit towards stopping the Coroutine if the movementEnd is still false");
    }

    void ChangeTarget()
    {
        currentSpace = targetSpace;
        spaceBehaviour = currentSpace.GetComponent<spaceBehaviour>();
        targetSpace = spaceBehaviour.NextSpace;
        spacePosition = new Vector3(targetSpace.transform.position.x, 2f, targetSpace.transform.position.z);
        movement--;
        Debug.Log("Target Reached, Current Movement: " + movement);
    }

    //TODO: Add a OnTriggerEnter or unique raycast to detect other player to engage combat

    IEnumerator Moving()
    {
        while (movement > 0)
        {
            spacePosition = new Vector3(targetSpace.transform.position.x, 2f, targetSpace.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, spacePosition, speed * Time.deltaTime);
            if (Physics.Raycast(detectRay, out RaycastHit info, 10f))
            {
                if (info.collider.gameObject == targetSpace.gameObject)
                {
                    ChangeTarget();
                }
                //TODO: Add directional choice where will change the object's direction                
            }
            yield return null;
        }
        movementEnd = true;
    }
}
