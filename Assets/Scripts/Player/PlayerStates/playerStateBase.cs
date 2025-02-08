using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script is the absract class for each of the states to use
/// Each state will override these states to behaving during runtime
/// </summary>

public abstract class playerStateBase
{
    //All of these states will be used in the playerStateManager which will provide each of these states to be used
    //Enter State will only play once and that's when the state is being swapped in
    public abstract void EnterState(playerStateManager player);

    //Update State will be playing constantly in which provides the code to keep on running
    //This state should also provide a condition to exit the state
    public abstract void UpdateState(playerStateManager player);

    //Exit State will only play once and that's when the state is being swapped out
    public abstract void ExitState(playerStateManager player);
}
