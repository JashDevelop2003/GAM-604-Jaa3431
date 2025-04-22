using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// The state animation is a component that uses the Animator component
/// This is to change the boolean parameters to true and false during the player's turn
/// There are 7 parameter booleans to change:
/// - isStarting is used to change the animation state from idle to deciding
/// - isSelecting occurs to change the animation from deciding to selected (roll state) and back. The boolean is also used to selected in the target state
/// - isWaling occurs to change the animation during the move state. This either transitions to the idle state or picking state depedning on the space landed. This can transition from the idle to walking if choosing a path
/// - isPicking occurs during the picking or item state, this then transitions to the idle state
/// - isAttacking occurs when the player is attacking
/// - isDamaged occurs when loosing health
/// - isDead occurs when health is below 0
/// </summary>

public class stateAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    private playerStateManager stateManager;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        stateManager = GetComponentInParent<playerStateManager>();
    }

    public void ChangeAnimationState(playerStateBase newState)
    {
        playerAnimator.SetBool("isDeciding", false);
        playerAnimator.SetBool("isSelecting", false);
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isPicking", false);
        if (newState == stateManager.DecidingState) 
        {
            playerAnimator.SetBool("isDeciding", true);
        }
        else if(newState == stateManager.RollState || newState == stateManager.TargetState)
        {
            playerAnimator.SetBool("isSelecting", true);
        }
        else if(newState == stateManager.MoveState)
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else if(newState == stateManager.PickingState || newState == stateManager.ItemState)
        {
            playerAnimator.SetBool("isPicking", true);
        }
    }

    //This occurs when the player ends their start state
    //public void DecidingAnimation(object sender, EventArgs e)
    //{
    //    playerAnimator.SetBool("isDeciding", true);
    //    playerAnimator.SetBool("isSelecting", false);
    //}

    //This occurs when confirming a selected card in the deciding state
    //public void SelectedAnimation(object sender, EventArgs e)
    //{
    //  playerAnimator.SetBool("isDeciding", false);
    //  playerAnimator.SetBool("isSelecting", true);
    //}

    //This occurs when starting the move state
    //public void WalkingAnimation(object sender, EventArgs e)
    //{
    //    playerAnimator.SetBool("isSelecting", false);
    //    playerAnimator.SetBool("isWalking", true);
    //}

    //This occurs when ending the move state and changing to the choosing state
    //public void StopWakingAnimation(object sender, EventArgs e)
    //{
    //    playerAnimator.SetBool("isWalking", false);
    //}

    //This occurs when confirming a route
    //public void ContinueWakingAnimation(object sender, EventArgs e)
    //{
    //    playerAnimator.SetBool("isWalking", true);
    //}

    //This occurs when ending the move state and changing to the picking or item state
    //public void PickingAnimation(object sender, EventArgs e)
    //{
    //    playerAnimator.SetBool("isPicking", true);
    //}

    //this occurs when ending the item state
    //public void EndPickingAnimation(object sender, EventArgs e) 
    //{ 
    //    playerAnimator.SetBool("isPicking", false);
    //}

    //This occurs during combat
    public void StopWalking(object sender, EventArgs e)
    {
        playerAnimator.SetBool("isWalking", false);
    }

    public void AttackingAnimation(object sender, EventArgs e)
    {
        playerAnimator.SetBool("isAttacking", true);
    }

    //This occurs after combat
    public void EndAttackingAnimation(object sender, EventArgs e)
    {
        playerAnimator.SetBool("isAttacking", false);
    }

    //This occurs when taking damage
    public void DamageAnimation(object sender, EventArgs e) 
    {
        playerAnimator.SetBool("isDamaged", true);
    }

    //This occurs after taking damage
    public void EndDamageAnimation(object sender, EventArgs e)
    {
        playerAnimator.SetBool("isDamaged", false);
    }

    //This occurs if health is equal to or below 0
    public void DeadAnimation(object sender, EventArgs e)
    {
        playerAnimator.SetBool("isDead", true);
    }
}
