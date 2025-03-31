using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<playerController>();
        characterName.SetText(controller.GetData.name);
        offenceValue.SetText(controller.GetModel.OffenceCards.ToString());
        defenceValue.SetText(controller.GetModel.DefenceCards.ToString());
        movementValue.SetText(controller.GetModel.MovementCards.ToString());
        statusValue.SetText(controller.GetModel.StatusCards.ToString());
        itemValue.SetText(controller.GetModel.ItemPile.ToString());

        healthPoints.SetText(controller.GetModel.CurrentHealth.ToString());
        manaPoints.SetText(controller.GetModel.CurrentMana.ToString());
        cashPoints.SetText(controller.GetModel.CurrentCash.ToString());
        thrustPercentage.SetText((controller.GetModel.ThrustMultiplier * 100).ToString() + "%" );
        guardPercentage.SetText((controller.GetModel.GuardMultiplier * 100).ToString() + "%");
        rollPercentage.SetText((controller.GetModel.RollMultiplier * 100).ToString() + "%");
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
    }
    public void DefenceUI()
    {
        defenceValue.SetText(controller.GetModel.DefenceCards.ToString());
    }
    public void MovementUI()
    {
        movementValue.SetText(controller.GetModel.MovementCards.ToString());
    }
    public void StatusUI()
    {
        statusValue.SetText(controller.GetModel.StatusCards.ToString());
    }

    public void ItemUI()
    {
        itemValue.SetText(controller.GetModel.ItemPile.ToString());
    }

    public void EffectUI (int effectInt, bool setActive)
    {
        effectDisplay[effectInt].SetActive(setActive);
    }

    public void BuffUI(int buffInt, bool setActive)
    {
        buffDisplay[buffInt].SetActive(setActive);
    }
}
