using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The space effect occurs when the player has ended their movement
/// The space have their own behaiour and apply unique state changes for the player
/// </summary>

public class spaceManager : MonoBehaviour
{
    public static spaceManager instance;
    private luckySpace lucky;


    //this is used to make this a singular instance of the component
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        lucky = GetComponent<luckySpace>();
    }

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

        //if the type is item then change their state to item state
        else if (type == spaceEnum.Item) 
        { 
            state.ChangeState(state.ItemState);
        }

        //if the type is lucky then apply 1 of the random 10 outcomes for the player to obtain
        else if(type == spaceEnum.Lucky)
        {
            //lucky.beginLucky(player, Random.Range(1, 11));
            lucky.beginLucky(player, 1);
        }
    }
}
