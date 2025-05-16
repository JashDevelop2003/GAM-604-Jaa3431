using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStateManager : MonoBehaviour
{
    [SerializeField] private gameStateBase currentState;
    [SerializeField] private gameStateBase minigameState;
    private ruleState ruleState;

    public gameStateBase MinigameState
    {
        get { return minigameState; }
        set { minigameState = value; }
    }

    public ruleState RuleState
    {
        get { return ruleState; }
    }

    // Start is called before the first frame update
    void Start()
    {
        minigameState = GetComponent<gameStateBase>();
        ruleState = GetComponent<ruleState>();
        currentState = RuleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(gameStateBase newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
