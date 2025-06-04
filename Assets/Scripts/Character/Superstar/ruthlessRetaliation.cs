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
    private combatSystem combatSystem;
    private dataManager dataManager;
    private playerController playerController;
    private playerStateManager stateManager;
    private currentEffects effects;
    private playerController opponentController;
    private startState state;
    private bool active;


    // Start is called before the first frame update
    void Awake()
    {
        combatSystem = combatSystem.instance;
        dataManager = Singleton<dataManager>.Instance;
        playerController = GetComponentInParent<playerController>();
        effects = GetComponentInParent<currentEffects>();
        stateManager = GetComponentInParent<playerStateManager>();
        state = GetComponentInParent<startState>();
        dataManager.saveFiles += saveActive;
        StartCoroutine(LoadingData());
    }

    IEnumerator LoadingData()
    {
        yield return new WaitUntil(() => dataManager.LoadComplete == true);
        RuthlessData ruthlessData = ruthlessSystem.Retrieve();
        if (ruthlessData != null)
        {
            active = ruthlessData.retaliating;
        }
        else
        {
            Debug.LogError("Something went wrong with the ruthless data");
        }

        if (active)
        {
            combatSystem.beforeCombatEvent += Prepared;
            state.startItemEvents += OverPrepared; //Has to be in Start Item Events to prevent stacking the effects
            playerController.DisplayAbility(playerController.GetData.abilityIcon[1], playerController.GetData.abilityColour[1]);
            Debug.Log("Loaded as Prepared");
        }
        else if (playerController.GetModel.AbilityUsed)
        {
            playerController.oneUseEvent += RuthlessRetaliation;
            Debug.Log("Hasn't been prepared yet");
        }

    }

    public void saveActive(object sender, EventArgs e)
    {
        RuthlessData ruthlessData = new RuthlessData
        {
            retaliating = active
        };

        ruthlessSystem.Store(ruthlessData);
    }

    public void RuthlessRetaliation(object sender, EventArgs e)
    {
        active = true;
        playerController.oneUseEvent -= RuthlessRetaliation;
        combatSystem.beforeCombatEvent += Prepared;
        state.startItemEvents += OverPrepared; //Has to be in Start Item Events to prevent stacking the effects
        playerController.DisplayAbility(playerController.GetData.abilityIcon[1], playerController.GetData.abilityColour[1]);
        Debug.Log("He's Prepared");
    }

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

    public void OverPrepared(object sender, EventArgs e)
    {
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
