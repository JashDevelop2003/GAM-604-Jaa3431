using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
/// <summary>
/// This is the player controller (presenter) that provides the logic for the model and changes to the view
/// This collect the character data and applies changes to the model.
/// </summary>
public class playerController : MonoBehaviour
{
    //this provide encapsulation of the player model, character data and the current path
    //this allows other classes to reference these to return the data
    //However the choosing state can set a new current path for the controller to collect and provide
    private playerModel playerModel;
    private playerView playerView;
    [SerializeField] private characterData Data;

    //This is to provide the current path for the move state to use
    [SerializeField] private GameObject currentPath;

    //These are the events to activate the passive and one use ability
    public event EventHandler oneUseEvent;

    //These are the events to activate the effects that occur
    public event EventHandler effectStartEvent;
    public event EventHandler effectEndEvent;

    public playerModel GetModel { get { return playerModel; } }
    public characterData GetData {  get { return Data; } }

    public GameObject Path { get { return currentPath; } set { currentPath = value; } }

    [Header("User Interface")]
    [SerializeField] private TMP_Text eventText;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip gainCashSound;
    [SerializeField] private AudioClip loseCashSound;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private AudioClip guardSound;
    [SerializeField] private AudioClip rollSound;
    [SerializeField] private AudioClip decreaseSound;
    [SerializeField] private AudioClip gameOverSound;
    private soundManager soundManager;

    [Header("Animation")]
    private stateAnimation animator;
    public event EventHandler takeDamageEvent;
    public event EventHandler endDamageEvent;
    public event EventHandler isDefeatedEvent;

    void Awake()
    {
        //this collects the path list for the player to start on
        pathOrder startingSpace = currentPath.GetComponent<pathOrder>();

        //this creates a new player model based on the character the player has chosen
        playerModel = new playerModel(Data);
        transform.position = new Vector3(startingSpace.SpaceOrder[1].transform.position.x, 2f, startingSpace.SpaceOrder[1].transform.position.z);

        //this collects the view for providing the interface of the statistics
        playerView = GetComponent<playerView>();

        Instantiate(GetData.characterObject, this.transform);
    }

    private void Start()
    {
        soundManager = Singleton<soundManager>.Instance;
        animator = GetComponentInChildren<stateAnimation>();
        takeDamageEvent += animator.DamageAnimation;
        endDamageEvent += animator.EndDamageAnimation;
        isDefeatedEvent += animator.DeadAnimation;
    }

    //This is to reset the multipliers from the effects of their previous turn
    //Thus also regains the mana for the player to use cards
    public void ResetStats(object sender, EventArgs e)
    {
        playerModel.CurrentMana = playerModel.MaxMana;
        playerView.ManaUI();
        ChangeThrust(1);
        ChangeGuard(1);
        ChangeRoll(1);
    }

    //ChangeCash is a method that when landing on a blue or red space will change the current cash to certain value
    public void ChangeCash(int value)
    {

        if (playerModel.CurrentCash + value < 0)
        {
            playerModel.CurrentCash = 0;
            soundManager.PlaySound(loseCashSound);
        }

        else
        {

            playerModel.CurrentCash += value;
            if (value > 0) 
            { 
                soundManager.PlaySound(gainCashSound);
            }
            else if(value < 0)
            {
                soundManager.PlaySound(loseCashSound);
            }
        }

        playerView.CashUI();
    }

    public void ChangeMana(int cost)
    {
        playerModel.CurrentMana -= cost;
        playerView.ManaUI();
    }

    //Roll is a mathod that subtracts the mana based on mana cost (parameter is roll cost) and the value of the dice (parameter is value)
    public void Roll(int value) 
    { 
        currentEffects shockEffect = GetComponent<currentEffects>();

        playerModel.RollValue = value;
        if (shockEffect.Shocked) 
        {
            ChangeHealth(-value);
        }
        Debug.Log(playerModel.RollValue);
    }

    public void ChangeHealth(int value) 
    {
        //This gets the current buff component to see if the player is invincble from taking any damage

        currentBuffs buffs = GetComponent<currentBuffs>();
        //If the player is invincle & would take and damage this turn
        //Then turn the value to 0 to prevent any damage from occurring
        if(buffs.IsInvincible && value < 0)
        {
            value = 0;
        }

        //If the current health being subtracted from the value is less than 0, the player is defeated
        if (playerModel.CurrentHealth + value <= 0)
        {
            playerModel.IsAlive = false;
            playerModel.CurrentHealth = 0;
            soundManager.PlaySound(gameOverSound);
            eventText.SetText(this.gameObject.name + " is Defeated");
            StartCoroutine(TakingDamage());
            StartCoroutine(GameOver());
        }
        //Else if the current health being added from the the value is greater than the max health, the current health will maximise to only the maximum health
        else if (playerModel.CurrentHealth + value > playerModel.MaxHealth)
        {
            playerModel.CurrentHealth = playerModel.MaxHealth;
            soundManager.PlaySound(healSound);
        }
        //otherwise the value adds (or subtract if the value is negative) to the new current health
        else
        {
            playerModel.CurrentHealth += value;
            if (value > 0) 
            { 
                soundManager.PlaySound(healSound);
            }
            else if(value < 0)
            {
                soundManager.PlaySound(damageSound);
                StartCoroutine(TakingDamage());
            }
        }

        playerView.HealthUI();
    }

    //For any Multiplier Changes the procedure for the parameter must be:
    //Collecting the Multiplier from the controller's model
    //Then Add/Substract/Divide/Multiply the multiplier
    //That paremeter float value is now the new multiplier
    public void ChangeThrust(float value)
    {
        if (value < 0) 
        {
            GetModel.ThrustMultiplier = 0;
            soundManager.PlaySound(decreaseSound);
        }
        else
        {
            if (value < GetModel.ThrustMultiplier) 
            {
                soundManager.PlaySound(decreaseSound);
            }
            else if(value > GetModel.ThrustMultiplier)
            {
                soundManager.PlaySound(thrustSound);
            }
            GetModel.ThrustMultiplier = value;
        }
        playerView.ThrustUI();
    }

    public void ChangeGuard(float value)
    {
        if (value < 0)
        {
            GetModel.GuardMultiplier = 0;
            soundManager.PlaySound(decreaseSound);
        }
        else
        {
            if (value < GetModel.GuardMultiplier)
            {
                soundManager.PlaySound(decreaseSound);
            }
            else if (value > GetModel.GuardMultiplier)
            {
                soundManager.PlaySound(guardSound);
            }
            GetModel.GuardMultiplier = value;
        }
        playerView.GuardUI();
    }

    public void ChangeRoll(float value)
    {
        if (value < 0)
        {
            GetModel.RollMultiplier = 0;
            soundManager.PlaySound(decreaseSound);
        }
        else
        {
            if (value < GetModel.RollMultiplier)
            {
                soundManager.PlaySound(decreaseSound);
            }
            else if (value > GetModel.RollMultiplier)
            {
                soundManager.PlaySound(rollSound);
            }
            GetModel.RollMultiplier = value;
        }
        playerView.RollUI();
    }

    public void IncrementDeck(deckTypeEnum deck)
    {
        if (deck == deckTypeEnum.Offence) 
        {
            GetModel.OffenceCards++;
            playerView.OffenceUI();
        }

        else if(deck == deckTypeEnum.Defence)
        {
            GetModel.DefenceCards++;
            playerView.DefenceUI();
        }

        else if(deck == deckTypeEnum.Movement)
        {
            GetModel.MovementCards++;
            playerView.MovementUI();
        }

        else if(deck == deckTypeEnum.Status)
        {
            GetModel.StatusCards++;
            playerView.StatusUI();
        }

        else if(deck == deckTypeEnum.Item)
        {
            GetModel.ItemPile++;
            playerView.ItemUI();
        }

        else
        {
            Debug.LogError("There's a possible chance that the type is not suitable");
        }
    }

    //This is activated when the player wants to use their one use ability from the player's specifc character.
    public void ActivateOneUse()
    {
        oneUseEvent?.Invoke(this, EventArgs.Empty);
        GetModel.AbilityUsed = false;
        playerView.OneUseAbilityUI();
    }

    public void ActivateStartEffects(object sender, EventArgs e)
    {
        effectStartEvent?.Invoke(this, EventArgs.Empty);
    }

    public void ActivateEndEffect()
    {
        effectEndEvent?.Invoke(this, EventArgs.Empty);
    }

    public void DisplayEffect(int enumInt, bool display)
    {
        playerView.EffectUI(enumInt, display);
    }

    public void DisplayBuff(int enumInt, bool display)
    {
        playerView.BuffUI(enumInt, display);
    }

    public void DisplayAbility(Sprite icon, Color colour)
    {
        playerView.AbilityUI(icon, colour);
    }

    IEnumerator TakingDamage()
    {
        takeDamageEvent?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(2);
        if (GetModel.IsAlive)
        {
            endDamageEvent?.Invoke(this, EventArgs.Empty);
        }
        else 
        { 
            isDefeatedEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5);
        sceneManager sceneManager = Singleton<sceneManager>.Instance;
        sceneManager.ChangeScene(sceneEnum.Victory);
    }

    private void OnDisable()
    {
        takeDamageEvent -= animator.DamageAnimation;
        endDamageEvent -= animator.EndDamageAnimation;
        isDefeatedEvent -= animator.DeadAnimation;
    }
}
