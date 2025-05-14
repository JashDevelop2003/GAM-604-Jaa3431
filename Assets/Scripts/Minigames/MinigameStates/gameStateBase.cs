using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class gameStateBase : MonoBehaviour
{
    //All of these states will be used in the playerStateManager which will provide each of these states to be used
    //Enter State will only play once and that's when the state is being swapped in
    public abstract void EnterState(gameStateManager player);

    //Update State will be playing constantly in which provides the code to keep on running
    //This state should also provide a condition to exit the state
    public abstract void UpdateState(gameStateManager player);

    //Exit State will only play once and that's when the state is being swapped out
    public abstract void ExitState(gameStateManager player);
}
