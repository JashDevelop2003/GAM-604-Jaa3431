using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceEffects : MonoBehaviour
{
    public void ActivateEffect(GameObject player, spaceEnum type)
    {
        playerStateManager state = player.GetComponent<playerStateManager>();
        if (state == null)
        {
            Debug.LogError("There is no player state manager to be called up, Check what the method's parameters or the game object's components");
        }

        if (type == spaceEnum.Blue)
        {
            playerController controller = player.GetComponent<playerController>();
            if (controller == null)
            {
                Debug.LogError("There is no player controller to be called up, Check what the method's parameters or the game object's components");
            }

            controller.ChangeCash(3);
            state.ChangeState(state.InactiveState);
        }

        else if (type == spaceEnum.Red)
        {
            playerController controller = player.GetComponent<playerController>();
            if (controller == null)
            {
                Debug.LogError("There is no player controller to be called up, Check what the method's parameters or the game object's components");
            }

            controller.ChangeCash(-3);
            state.ChangeState(state.InactiveState);
        }

        else if (type == spaceEnum.Card)
        {
            state.ChangeState(state.PickingState);
        }
    }
}
