using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO Tomorrow: Improve the States in order to be tested
public class startState : playerStateBase
{
    public override void EnterState(playerStateManager player)
    {
        Debug.LogWarning("Requires Movement Cards to be Draw");
    }

    public override void UpdateState(playerStateManager player)
    {
        player.ChangeState(player.decidingState);
    }

    public override void ExitState(playerStateManager player)
    {
        Debug.Log("Requires Status Effect (TODO: Next Stage)");
    }
}
