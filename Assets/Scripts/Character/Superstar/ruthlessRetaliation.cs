using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ruthless Retailiation is The Superstar's One Use Ability & Makes him prepared
/// When Prepared during combat as the defender will negate any attack & will deal 50 damage to opponent
/// However if the player never became the defender once it's their next turn they will half their Thrust, Guard & Roll as well as being applied with Feared, Exposed & Slowed for 4 turns
/// </summary>

public class ruthlessRetaliation : MonoBehaviour
{
    //Ruthless retaliation will need to access both the combat system to counter and the data manager to retrieve data for him being active
    private combatSystem combatSystem;
    private dataManager dataManager;

    //Player Controller is used to disable the ability once used and update the UI
    private playerController playerController;

    //State manager is used to check if the player is currently in the defend state
    private playerStateManager stateManager;

    //Effects is used to apply exposed, feared, slow and stun to self
    private currentEffects effects;

    //Opponent's player controller is used to deal damaage to the opponent
    private playerController opponentController;

    //The start state is used to check if the player is currently on the start state to debuff self
    private startState state;

    //Active boolean is used to identify the current status of the character when loading in the game
    private bool active;


    // Start is called before the first frame update
    void Awake()
    {
        //Static instance is used to access the combat system
        //Singleton instance is used to access the data manager
        combatSystem = combatSystem.instance;
        dataManager = Singleton<dataManager>.Instance;

        //Due to being a derived object to the player, the ability has to reference these components in the base object
        playerController = GetComponentInParent<playerController>();
        effects = GetComponentInParent<currentEffects>();
        stateManager = GetComponentInParent<playerStateManager>();
        state = GetComponentInParent<startState>();

        //When enabled the obseerver method for saving the active bool is listening to the save files from the data manager
        dataManager.saveFiles += saveActive;

        //when enabled a coroutine starts to wait until the players are loaded in which will then start loading in the active ability
        StartCoroutine(LoadingData());
    }

    //This corutine will check if the player is currently active towards using the ability
    IEnumerator LoadingData()
    {
        //Wait Until suspends the coroutine until loading is complete
        yield return new WaitUntil(() => dataManager.LoadComplete == true);

        //The coroutine will then retrieve the data to check if the player is currently retaliating
        RuthlessData ruthlessData = ruthlessSystem.Retrieve();
        if (ruthlessData != null)
        {
            active = ruthlessData.retaliating;
        }
        else
        {
            Debug.LogError("Something went wrong with the ruthless data");
        }

        //If the player is currently active then this will then carry on the ability to where it was left off
        if (active)
        {
            combatSystem.beforeCombatEvent += Prepared;
            state.startItemEvents += OverPrepared; //Has to be in Start Item Events to prevent stacking the effects
            playerController.DisplayAbility(playerController.GetData.abilityIcon[1], playerController.GetData.abilityColour[1]);
        }
        //Otherwise the one use ability observer method still listens to the player controller one use event subject
        else if (playerController.GetModel.AbilityUsed)
        {
            playerController.oneUseEvent += RuthlessRetaliation;
        }

    }

    //This observer method listens to the save files event from the data manager
    //The method saves the active boolean to check if the player is currently retaliating
    public void saveActive(object sender, EventArgs e)
    {
        RuthlessData ruthlessData = new RuthlessData
        {
            retaliating = active
        };

        ruthlessSystem.Store(ruthlessData);
    }

    //This observer method listens to the one use event in the player controller
    //The method makes the player's status of retaliating to true and makes 2 observer methods listen to 2 unique subjects
    //This also make this method stop listening to the one use event as well
    public void RuthlessRetaliation(object sender, EventArgs e)
    {
        active = true;
        playerController.oneUseEvent -= RuthlessRetaliation;
        combatSystem.beforeCombatEvent += Prepared;
        state.startItemEvents += OverPrepared; //Has to be in Start Item Events to prevent stacking the effects
        playerController.DisplayAbility(playerController.GetData.abilityIcon[1], playerController.GetData.abilityColour[1]);
        Debug.Log("He's Prepared");
    }

    //This observer method listens to the before combat event in the combat system
    //The method checks if the player is currently defending, if they are then they deal 50 damage to their opponent
    //This will then make both this method and OverPrepared observer method stop listening to their suitable subjects
    public void Prepared(object sender, EventArgs e)
    {
        if (stateManager.CurrentState == stateManager.DefendState)
        {
            active = false;
            combatSystem.AttackValue = 0;
            opponentController = combatSystem.AttackingPlayer.GetComponent<playerController>();
            opponentController.ChangeHealth(-50);
            combatSystem.beforeCombatEvent -= Prepared;
            state.startItemEvents -= OverPrepared;
            combatSystem.EventText.SetText("Ruthless Retaliation! Deal 50 Damage to Attacker");
            playerController.DisplayAbility(playerController.GetData.abilityIcon[0], playerController.GetData.abilityColour[0]);
        }
    }

    //This observer method listens to the start items event in the start state. This is due to a bug issue when listening to the start event subject.
    //The method will debuff himself by applying exposed, feared and slow for 5 turns and stun for 2 turns
    public void OverPrepared(object sender, EventArgs e)
    {
        //Stun is a ending effect, meaning that the effect will  decrement when they end their turn
        active = false;
        effects.AddEffect(effectEnum.Exposed, 5);
        effects.AddEffect(effectEnum.Feared, 5);
        effects.AddEffect(effectEnum.Slowed, 5);
        effects.AddEffect(effectEnum.Stunned, 3);
        combatSystem.beforeCombatEvent -= Prepared;
        state.startItemEvents -= OverPrepared;
        playerController.DisplayAbility(playerController.GetData.abilityIcon[0], playerController.GetData.abilityColour[0]);
    }

    private void OnDisable()
    {
        dataManager.saveFiles -= saveActive;
        playerController.oneUseEvent -= RuthlessRetaliation;
        combatSystem.beforeCombatEvent -= Prepared;
        state.startItemEvents -= OverPrepared;
    }
}
