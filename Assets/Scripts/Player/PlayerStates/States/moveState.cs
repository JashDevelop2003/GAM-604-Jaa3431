using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveState : playerStateBase
{
    private int movement;
    private float speed = 2f;
    private Ray detectRay;
    private bool movementEnd;
    private playerController controller;
    private Coroutine movePlayer;

    private GameObject currentSpace;
    private spaceBehaviour spaceBehaviour;
    private spaceEnum currentSpaceType;
    private Transform targetSpace;
    private Vector3 spacePosition;
    
    public override void EnterState(playerStateManager player)
    {
        movementEnd = false;
        controller = GetComponent<playerController>();
        currentSpace = controller.Space;
        spaceBehaviour = currentSpace.GetComponent<spaceBehaviour>();
        currentSpaceType = spaceBehaviour.SpaceType;
        targetSpace = spaceBehaviour.NextSpace;
        detectRay = new Ray(transform.position, -transform.up);
        movePlayer = StartCoroutine(Moving());
    }

    public override void UpdateState(playerStateManager player)
    {
        
        if(Physics.Raycast(detectRay, out RaycastHit info))
        {
            if (detectRay.Equals(targetSpace))
            {
                ChangeTarget();
            }
        }

        if (movementEnd)
        {
            Debug.LogWarning("Needs Space Behaviour");
        }


    }

    public override void ExitState(playerStateManager player) 
    { 
        
    }

    void ChangeTarget()
    {
        currentSpace = targetSpace.gameObject;
        targetSpace = spaceBehaviour.NextSpace;
        movement--;
        Debug.Log("Target Reached, Current Movement: " + movement);
    }

    IEnumerator Moving()
    {
        while (movement > 0)
        {
            spacePosition = new Vector3(targetSpace.transform.position.x, 2f, targetSpace.transform.position.z);
            Vector3.MoveTowards(transform.position, spacePosition, speed);
            yield return null;
        }
        movementEnd = true;
    }
}
