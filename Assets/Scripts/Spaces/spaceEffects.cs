using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The space effect occurs when the player has ended their movement
/// The space have their own behaiour and apply unique state changes for the player
/// </summary>

public class spaceEffects : MonoBehaviour
{
    //this method is only used when the player has ended their turn
    //the parameters are the current player and the current type they landed on when their movement has ended
    public void ActivateEffect(GameObject player, spaceEnum type)
    {
        //this reference the player's state manager and checks if the reference occurred
        playerStateManager state = player.GetComponent<playerStateManager>();
        if (state == null)
        {
            Debug.LogError("There is no player state manager to be called up, Check what the method's parameters or the game object's components");
        }

        //the behaviour occurs depending on the player's space type
        //if the type is blue then add 3 cash to the current cash in the player controller
        //change the player's current state to inactive state to end their turn
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

        //if the type is red then subtract 3 cash to the current cash in the player controller
        //change the player's current state to inactive state to end their turn
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

        //if the type is card then change their state to picking state
        else if (type == spaceEnum.Card)
        {
            state.ChangeState(state.PickingState);
        }

        else if (type == spaceEnum.Item) 
        { 
            state.ChangeState(state.ItemState);
        }
    }
}
