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

    private bool changeDirection;
    private bool movementEnd;

    private playerController controller;
    private Coroutine movePlayer;

    private directionEnum currentDirection;

    private GameObject currentPath;
    private int currentSpaceInt = 1;

    private GameObject currentSpace;
    private pathOrder pathOrder;
    private spaceEnum currentSpaceType;

    [SerializeField] private GameObject effectManager; //The effect manager is the board map
    private spaceEffects spaceEffects;

    private GameObject targetSpace;

    private Vector3 spacePosition;
    
    public override void EnterState(playerStateManager player)
    {
        if(player.PreviousState == player.ChoosingState)
        {
            currentSpaceInt = 0;
        }

        controller = GetComponent<playerController>();
        currentPath = controller.Path;
        pathOrder = currentPath.GetComponent<pathOrder>();
        currentDirection = pathOrder.Direction;

        currentSpace = pathOrder.SpaceOrder[currentSpaceInt];
        targetSpace = pathOrder.SpaceOrder[currentSpaceInt + 1];

        spaceEffects = effectManager.GetComponent<spaceEffects>();

        
        if (player.PreviousState == player.RollState)
        {
            movement = controller.GetModel.RollValue;
        }
        
        movementEnd = false;
        changeDirection = false;

        movePlayer = StartCoroutine(Moving());
    }

    public override void UpdateState(playerStateManager player)
    {
        var moveSpeed = speed * Time.deltaTime;

        if (movementEnd)
        {
            player.ChangeState(player.InactiveState);
            //Debug.LogWarning("Needs Space Behaviour");
        }

        if (changeDirection) 
        {
            choosingState choosing = player.ChoosingState.GetComponent<choosingState>();
            choosing.CollectCurrentPath(pathOrder.SpaceOrder[currentSpaceInt], currentDirection);
            player.ChangeState(player.ChoosingState);
        }


    }

    private void FixedUpdate()
    {
        rayPosition = new Vector3(transform.position.x,  transform.position.y - 0.5f, transform.position.z);

        detectRay = new Ray(rayPosition, -transform.up);
    }

    public override void ExitState(playerStateManager player) 
    {        
        StopCoroutine(movePlayer);
    }

    void ChangeTarget(int nextSpace)
    {
        currentSpace = pathOrder.SpaceOrder[currentSpaceInt];
        spaceBehaviour type = pathOrder.SpaceOrder[currentSpaceInt].transform.GetComponent<spaceBehaviour>();
        currentSpaceType = type.SpaceType;
        targetSpace = pathOrder.SpaceOrder[currentSpaceInt + 1];
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
                    currentSpaceInt++;
                    if(currentSpaceInt + 1 == pathOrder.SpaceOrder.Count)
                    {
                        changeDirection = true;
                    }
                    else
                    {
                        ChangeTarget(currentSpaceInt);
                    }
                    
                }
                               
            }
            yield return null;
        }

        spaceEffects.ActivateEffect(this.gameObject, currentSpaceType);

       // movementEnd = true;
    }
}
