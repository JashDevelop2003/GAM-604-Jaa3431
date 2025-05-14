using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class doubleOrNothingManager : Singleton<doubleOrNothingManager>
{
    private int cash;
    private int chance;

    [Header("User Interface")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text chanceText;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip[] soundOutcome = new AudioClip[3];
    private soundManager soundManager;

    [Header ("Scene Management")]
    [SerializeField] private sceneEnum scene;
    private sceneManager sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        cash = 10;
        chance = Random.Range(90, 101);
        cashText.SetText(cash.ToString());
        chanceText.SetText(chance.ToString());
        soundManager = Singleton<soundManager>.Instance;
        sceneManager = Singleton<sceneManager>.Instance;
    }

    // Update is called once per frame
    public void Outcome(choiceEnum choice)
    {
        if (choice == choiceEnum.Keep) 
        { 
            StartCoroutine(TakeCash());
        }
        else if(choice == choiceEnum.Double)
        {
            StartCoroutine(DoubleChance(Random.Range(0, 101)));
        }
        else
        {
            Debug.LogError("You haven't chosen a choice, press A for keep or D to Double");
        }
    }

    void DisplayOutcome()
    {
        cashText.SetText(cash.ToString());
        chanceText.SetText(chance.ToString());
    }

    IEnumerator TakeCash()
    {
        soundManager.PlaySound(soundOutcome[0]);
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
        }
        else
        {
            cash *= 2;
            soundManager.PlaySound(soundOutcome[1]);
            chance -= Random.Range(1, 11);
            if(chance < 0)
            {
                chance = 0;
            }
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(soundOutcome[0].samples + 1);
        sceneManager.ChangeScene(scene);
    }
}
