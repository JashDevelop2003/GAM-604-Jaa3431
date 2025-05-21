using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class doubleOrNothingManager : Singleton<doubleOrNothingManager>
{
    public event EventHandler endEvent;

    private int cash;
    private int chance;

    private turnManager turnManager;
    private minigameManager minigameManager;

    [Header("User Interface")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text chanceText;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip[] soundOutcome = new AudioClip[3];
    private soundManager soundManager;

    // Start is called before the first frame update
    public void BeginMinigame()
    {
        cash = 10;
        chance = UnityEngine.Random.Range(90, 101);
        cashText.SetText(cash.ToString());
        chanceText.SetText(chance.ToString() + "%");
        soundManager = Singleton<soundManager>.Instance;
        turnManager = Singleton<turnManager>.Instance;
        minigameManager = Singleton<minigameManager>.Instance;
        endEvent += minigameManager.EndMinigame;
    }

    public void Outcome(choiceEnum choice)
    {
        if (choice == choiceEnum.Keep) 
        { 
            StartCoroutine(TakeCash());
        }
        else if(choice == choiceEnum.Double)
        {
            StartCoroutine(DoubleChance(UnityEngine.Random.Range(0, 101)));
        }
        else
        {
            Debug.LogError("You haven't chosen a choice, press A for keep or D to Double");
        }
    }

    void DisplayOutcome()
    {
        cashText.SetText(cash.ToString());
        chanceText.SetText(chance.ToString() + "%");
    }

    IEnumerator TakeCash()
    {
        playerController player = turnManager.CurrentPlayer.GetComponent<playerController>();
        player.ChangeCash(cash);
        soundManager.PlaySound(soundOutcome[0]);
        StartCoroutine(GameOver());
        yield return null;
    }

    IEnumerator DoubleChance(int outcome) 
    { 
        yield return new WaitForSeconds(1);
        if (outcome > chance) 
        {
            cash = 0;
            soundManager.PlaySound(soundOutcome[2]);
            infoText.SetText("Game Over, the player loses all their prize cash");
            DisplayOutcome();
            StartCoroutine(GameOver());
        }
        else
        {
            cash *= 2;
            soundManager.PlaySound(soundOutcome[1]);
            chance -= UnityEngine.Random.Range(1, 11);
            if(chance < 0)
            {
                chance = 0;
            }
            DisplayOutcome();
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5f);
        endEvent?.Invoke(this, EventArgs.Empty);
        endEvent -= minigameManager.EndMinigame;
    }
}
