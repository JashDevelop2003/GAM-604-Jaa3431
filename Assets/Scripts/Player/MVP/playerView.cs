using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerView : MonoBehaviour
{
    //The controller will be use to update the view by calling suitable methods
    //The view is used to collect the changes of the player from the controller
    private playerController controller;

    //The character name will be towards the character data's name
    [SerializeField] private TMP_Text characterName;
    
    //The decks will be towards the amount of cards in the decks from the model
    [SerializeField] private TMP_Text offenceValue;
    [SerializeField] private TMP_Text defenceValue;
    [SerializeField] private TMP_Text movementValue;
    [SerializeField] private TMP_Text statusValue;
    [SerializeField] private TMP_Text itemValue;

    //these stats are towards values on the player itself instead of the decks
    [SerializeField] private TMP_Text healthPoints;
    [SerializeField] private TMP_Text manaPoints;
    [SerializeField] private TMP_Text cashPoints;
    [SerializeField] private TMP_Text thrustPercentage;
    [SerializeField] private TMP_Text guardPercentage;
    [SerializeField] private TMP_Text rollPercentage;

    [SerializeField] private List<GameObject> effectDisplay;
    [SerializeField] private List<GameObject> buffDisplay;

    //These stats are towards character's ability
    [SerializeField] private GameObject oneUseAbility;
    [SerializeField] private Image abilityIcon;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<playerController>();
        DisplayUI();
    }

    public void DisplayUI()
    {
        characterName.SetText(controller.GetData.name);
        OffenceUI();
        DefenceUI();
        MovementUI();
        StatusUI();
        ItemUI();

        HealthUI();
        ManaUI();
        CashUI();
        ThrustUI();
        guardPercentage.SetText((controller.GetModel.GuardMultiplier * 100).ToString() + "%");
        rollPercentage.SetText((controller.GetModel.RollMultiplier * 100).ToString() + "%");

        if (controller.GetModel.AbilityUsed)
        {
            oneUseAbility.SetActive(true);
        }
        else
        {
            oneUseAbility.SetActive(false);
        }
    }


    public void HealthUI()
    {
        healthPoints.SetText(controller.GetModel.CurrentHealth.ToString());
    }

    public void ManaUI()
    {
        manaPoints.SetText(controller.GetModel.CurrentMana.ToString());
    }

    public void CashUI()
    {
        cashPoints.SetText(controller.GetModel.CurrentCash.ToString());
    }

    public void ThrustUI()
    {
        thrustPercentage.SetText((controller.GetModel.ThrustMultiplier * 100).ToString() + "%");
    }

    public void GuardUI()
    {
        guardPercentage.SetText((controller.GetModel.GuardMultiplier * 100).ToString() + "%");
    }
    public void RollUI()
    {
        rollPercentage.SetText((controller.GetModel.RollMultiplier * 100).ToString() + "%");
    }

    public void OffenceUI()
    {
        offenceValue.SetText(controller.GetModel.OffenceCards.ToString());
        CheckLimit(offenceValue, controller.GetModel.OffenceCards, 0);
    }
    public void DefenceUI()
    {
        defenceValue.SetText(controller.GetModel.DefenceCards.ToString());
        CheckLimit(defenceValue, controller.GetModel.DefenceCards, 1);
    }
    public void MovementUI()
    {
        movementValue.SetText(controller.GetModel.MovementCards.ToString());
        CheckLimit(movementValue, controller.GetModel.MovementCards, 2);
    }
    public void StatusUI()
    {
        statusValue.SetText(controller.GetModel.StatusCards.ToString());
        CheckLimit(statusValue, controller.GetModel.StatusCards, 3);
    }

    public void ItemUI()
    {
        itemValue.SetText(controller.GetModel.ItemPile.ToString());
        CheckLimit(itemValue, controller.GetModel.ItemPile, 4);
    }

    private void CheckLimit(TMP_Text text, int value, int deckCapacity)
    {
        if (value == controller.GetData.deckCapacity[deckCapacity])
        {
            text.color = Color.red;
        }
        else if (value == controller.GetData.deckCapacity[deckCapacity] - 1)
        {
            text.color = Color.yellow;
        }
    }

    public void EffectUI (int effectInt, bool setActive)
    {
        effectDisplay[effectInt].SetActive(setActive);
    }

    public void BuffUI(int buffInt, bool setActive)
    {
        buffDisplay[buffInt].SetActive(setActive);
    }

    public void AbilityUI(Sprite icon, Color colour)
    {
        abilityIcon.sprite = icon;
        abilityIcon.color = colour;
    }

    public void OneUseAbilityUI()
    {
        oneUseAbility.SetActive(false);
    }
}
