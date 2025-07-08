using System;
using UnityEngine;
/// <summary>
/// Last Resport is the Reaper's One Use Ability which provides a major risk for the player when using the ability
/// The pro of using this ability is that roll and thrust are triple their value
/// In Addition defeating an opponent will turn back to normal which Heals 50% of Max Health & Increases Max Health by 25%
/// The risk of using the ability is that the player must defeat an opponent before the end of their turn (enter state of inactive state)
/// Failing to defeat an opponent before ending their turn will cause the player to be instantly defeated
/// </summary>

public class lastReapsort : MonoBehaviour
{
    //The boolean is used to prevent the player from using the ability again
    private bool passiveUsed = false;
    [SerializeField] private bool lastReapsortActive = false;
    public bool LastReapsortActive
    {
        get { return lastReapsortActive; }
        set { lastReapsortActive = value; }
    }

    private Coroutine abilityBegin;

    //This is the controller which will need to reference the player object into the parent
    private playerController controller;
    private moveState moveState;
    private inactiveState inactiveState;

    combatSystem combatSystem;

    private GameObject opponentObject;

    [Header("User Interface")]
    private decidingState stateUI;

    //When awake the class has to gather the controller component
    void Start()
    {
        controller = GetComponentInParent<playerController>();
        //This will be used for events
        moveState = GetComponentInParent<moveState>();
        inactiveState = GetComponentInParent<inactiveState>();
        stateUI = GetComponentInParent<decidingState>();

        //The object is a prefab meaning that when instantiated won't have the combat system object
        //This means that the class requires to find the prefab of the combat system
        //The combat system  will be used for events
        combatSystem = combatSystem.instance;

        //The one use ability waits until the player uses their one use ability to begin the last reapsort
        if (!passiveUsed)
        {
            controller.oneUseEvent += BeginReaping;
        }
    }

    //This method is invoked when using the ability
    //When the ability is called and the player hasn't use the ability yet in game will change the event to CheckOpponentHealth
    //In addition the thrust in the combat system will triple and the roll in roll state will triple the value
    public void BeginReaping(object sender, EventArgs e)
    {
        if (!passiveUsed)
        {
            passiveUsed = true;
            lastReapsortActive = true;
            //controller.oneUseEvent += CheckOpponentHealth;
            controller.oneUseEvent -= BeginReaping;
            moveState.beginMoveEvent += Chase;
            inactiveState.endEvents += FailedReaping;
            combatSystem.beforeCombatEvent += Reap;
            combatSystem.afterCombatEvent += DefeatedOpponent;
            controller.DisplayAbility(controller.GetData.abilityIcon[1], controller.GetData.abilityColour[1]);
            stateUI.EventText.SetText("Ability Used - Last Reapsort: Triple Thrust & Roll. You must defeat someone to stay in the game");
        }
    }

    public void Chase(object sender, EventArgs e)
    {
        controller.GetModel.RollValue *= 3;
    }
    
    public void Reap(object sender, EventArgs e)
    {
        combatSystem.AttackValue *= 3;
    }
    
    //This method is invoked when the player has dealt damage to an opponent
    //This checks if they have defeated the opponent
    public void DefeatedOpponent(object sender, EventArgs e)
    {
        //This reference the opponent object from the combat system to see if the IsAlive boolean is false
        playerController opponentController = combatSystem.DefendingPlayer.GetComponent<playerController>();

        //If the opponent is defeated then set the form to false, heal 50% of Max Health & Gain 25% Max Health
        if (!opponentController.GetModel.IsAlive)
        {
            combatSystem.EventText.SetText("Reaper has Defeated Someone");
            controller.DisplayAbility(controller.GetData.abilityIcon[0], controller.GetData.abilityColour[0]);
            EndReaping();
        }
        //otherwise the player is still in last reapsort
        else
        {
            combatSystem.EventText.SetText("Still alive");
        }
    }

    public void FailedReaping(object sender, EventArgs e) 
    {
        controller.ChangeHealth(-999);
        controller.GetModel.IsAlive = false;
        moveState.beginMoveEvent -= Chase;
        inactiveState.endEvents -= FailedReaping;
        combatSystem.beforeCombatEvent -= Reap;
        combatSystem.duringCombatEvent -= DefeatedOpponent;
    }

    public void EndReaping()
    {
        controller.ChangeHealth(controller.GetModel.MaxHealth / 4);
        controller.GetModel.MaxHealth *= (int)1.25;
        lastReapsortActive = false;
        moveState.beginMoveEvent -= Chase;
        inactiveState.endEvents -= FailedReaping;
        combatSystem.beforeCombatEvent -= Reap;
        combatSystem.duringCombatEvent -= DefeatedOpponent;

    }

    private void OnDisable()
    {
        controller.oneUseEvent -= BeginReaping;
        moveState.beginMoveEvent -= Chase;
        inactiveState.endEvents -= FailedReaping;
        combatSystem.beforeCombatEvent -= Reap;
        combatSystem.afterCombatEvent -= DefeatedOpponent;
    }
}
