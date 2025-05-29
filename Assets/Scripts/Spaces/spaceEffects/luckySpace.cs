using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// This is the lucky space component that once called
/// </summary>

public class luckySpace : MonoBehaviour
{
    //this is to obtain the player and their state
    [SerializeField] private GameObject luckyPlayer;
    private playerStateManager playerState;
    private playerController playerController;

    //The possible and selected card/relic are use to provide a random card/relic for the player to obtain

    private List<movementCardStats> possibleMovementCards;
    private movementCardStats selectedMovementCard;

    private List<offenceCardStats> possibleOffenceCards;
    private offenceCardStats selectedOffenceCard;

    private List<defenceCardStats> possibleDefenceCards;
    private defenceCardStats selectedDefenceCard;

    private List<statusCardStats> possibleStatusCards;
    private statusCardStats selectedStatusCard;

    private List<itemStats> possibleRelics;
    private itemStats selectedRelic;

    private int selectedInt;

    [SerializeField] private TMP_Text eventText;

    public void beginLucky(GameObject player, int outcome)
    {
        //lucky player turns to player in order to have the variable use for obtaining specific resources
        luckyPlayer = player;
        playerState = player.GetComponent<playerStateManager>();
        playerController = player.GetComponent<playerController>();

        //If the outcome is 1 then obtain a relic
        if (outcome == 1)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Item));
            eventText.SetText("Outcome: Obtain Relic");
        }

        //if the outcome is 2 then obtain a legendary offence card
        else if (outcome == 2)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Offence));
            eventText.SetText("Outcome: Obtain Legendary Offence Card");
        }

        //if the outcome is 3 then obtain a legendary defence card
        else if (outcome == 3) 
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Defence));
            eventText.SetText("Outcome: Obtain Legendary Defence Card");
        }

        //if the outcome is 4 then obtain a legendary movement card
        else if(outcome == 4)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Movement));
            eventText.SetText("Outcome: Obtain Legendary Movement Card");
        }

        //if the outcome is 5 then obtain a legendary status card
        else if (outcome == 5)
        {
            StartCoroutine(ObtainingResource(deckTypeEnum.Status));
            eventText.SetText("Outcome: Obtain Legendary Status Card");
        }

        //if the outcome is 6 then gain 100 cash
        else if (outcome == 6)
        {
            playerController.ChangeCash(100);
            eventText.SetText("Outcome: Gain 100 Cash");
            StartCoroutine(EndLucky());
        }

        //if the outcome is 7 then gain 25% Health
        else if(outcome == 7)
        {
            playerController.ChangeHealth((int)(playerController.GetModel.MaxHealth * 0.25f));
            eventText.SetText("Outcome: Heal 25% of Max Health");
            StartCoroutine(EndLucky());
        }


        /// Invincibility & Lucky has to have a cooldown of 4 since this occurs at the end
        
        //if the outcome is 8 then gain Invincibility for 3 turns
        else if(outcome == 8)
        {
            currentBuffs buffs = player.GetComponent<currentBuffs>();
            buffs.AddBuff(buffEnum.Invincible, 4, 0);
            eventText.SetText("Outcome: Gain Invincibility for 3 turns");
            StartCoroutine(EndLucky());
        }

        //if the outcome is 9 then gain lucky for 3 turns
        else if(outcome == 9)
        {
            currentBuffs buffs = player.GetComponent<currentBuffs>();
            buffs.AddBuff(buffEnum.Lucky, 4, 0);
            eventText.SetText("Outcome: Gain Lucky for 3 turns");
            StartCoroutine(EndLucky());
        }

        //if the outcome is 10 then gain hasty for 3 turns
        else if(outcome == 10)
        {
            currentBuffs buffs = player.GetComponent<currentBuffs>();
            buffs.AddBuff(buffEnum.Hasty, 3, 0);
            eventText.SetText("Outcome: Gain Hasty for 3 turns");
            StartCoroutine(EndLucky());
        }
    }

    IEnumerator ObtainingResource(deckTypeEnum type)
    {
        yield return new WaitForSeconds(2);
        if (type == deckTypeEnum.Offence)
        {
            ObtainOffence();
        }
        else if (type == deckTypeEnum.Defence)
        {
            ObtainDefence();
        }
        else if (type == deckTypeEnum.Movement)
        {
            ObtainMovement();
        }
        else if (type == deckTypeEnum.Status) 
        { 
            ObtainStatus();
        }
        else if(type == deckTypeEnum.Item)
        {
            ObtainRelic();
        }
        else
        {
            Debug.LogError("Something went wrong with the Obtaining Resource Coroutine");
        }
    }

    private void ObtainOffence()
    {
        //This obtains the character data of the possible offence cards the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        playerController controller = luckyPlayer.GetComponent<playerController>();
        possibleOffenceCards = controller.GetData.possibleOffenceCards;
        selectedInt = Random.Range(0, possibleOffenceCards.Count);
        selectedOffenceCard = possibleOffenceCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedOffenceCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = Random.Range(0, possibleOffenceCards.Count);
            selectedOffenceCard = possibleOffenceCards[selectedInt];
        }

        //This section checks if the player can obtain the offence card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        offenceDeckPool offencePool = luckyPlayer.GetComponentInChildren<offenceDeckPool>();
        GameObject offenceCard = offencePool.GetAvailableOffence();
        if (offenceCard != null)
        {
            offenceCard.SetActive(true);
            offenceCard offence = offenceCard.AddComponent<offenceCard>();
            offence.CreateCard(selectedOffenceCard);
            eventText.SetText("Card Obtained: " + offence.AttackCard.cardName);
            playerController.IncrementDeck(deckTypeEnum.Offence);
            if (controller.Player == 1)
            {
                playerOneData playerData = luckyPlayer.GetComponentInChildren<playerOneData>();
                playerData.storedOffence.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = luckyPlayer.GetComponentInChildren<playerTwoData>();
                playerData.storedOffence.Add(selectedInt);
            }
        }

        else if (offenceCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Offence cards");
        }

        //This then clears the list of possible cards and turns the selected card to null and ends the player's turn
        possibleOffenceCards = null;
        selectedOffenceCard = null;
        StartCoroutine(EndLucky());
    }

    private void ObtainDefence()
    {
        //This obtains the character data of the possible defence cards the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        playerController controller = luckyPlayer.GetComponent<playerController>();
        possibleDefenceCards = controller.GetData.possibleDefenceCards;
        selectedInt = Random.Range(0, possibleDefenceCards.Count);
        selectedDefenceCard = possibleDefenceCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedDefenceCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = Random.Range(0, possibleDefenceCards.Count);
            selectedDefenceCard = possibleDefenceCards[selectedInt];
        }

        //This section checks if the player can obtain the defence card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        defenceDeckPool defencePool = luckyPlayer.GetComponentInChildren<defenceDeckPool>();
        GameObject defenceCard = defencePool.GetAvailableDefence();
        if (defenceCard != null)
        {
            defenceCard.SetActive(true);
            defenceCard defence = defenceCard.AddComponent<defenceCard>();
            defence.CreateCard(selectedDefenceCard);
            eventText.SetText("Card Obtained: " + defence.DefendCard.cardName);
            playerController.IncrementDeck(deckTypeEnum.Defence);
            if (controller.Player == 1)
            {
                playerOneData playerData = luckyPlayer.GetComponentInChildren<playerOneData>();
                playerData.storedDefence.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = luckyPlayer.GetComponentInChildren<playerTwoData>();
                playerData.storedDefence.Add(selectedInt);
            }
        }

        else if (defenceCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Defence cards");
        }

        //This then clears the list of possible cards and turns the selected card to null and ends the player's turn
        possibleDefenceCards = null;
        selectedDefenceCard = null;
        StartCoroutine(EndLucky());
    }

    private void ObtainMovement()
    {
        //This obtains the character data of the possible movement cards the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        playerController controller = luckyPlayer.GetComponent<playerController>();
        possibleMovementCards = controller.GetData.possibleMovementCards;
        selectedInt = Random.Range(0, possibleMovementCards.Count);
        selectedMovementCard = possibleMovementCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedMovementCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = Random.Range(0, possibleMovementCards.Count);
            selectedMovementCard = possibleMovementCards[selectedInt];
        }

        //This section checks if the player can obtain the movement card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        movementDeckPool movePool = luckyPlayer.GetComponentInChildren<movementDeckPool>();
        GameObject moveCard = movePool.GetAvailableMovement();
        if (moveCard != null)
        {
            moveCard.SetActive(true);
            movementCard movement = moveCard.AddComponent<movementCard>();
            movement.CreateCard(selectedMovementCard);
            eventText.SetText("Card Obtained: " + movement.MoveCard.cardName);
            playerController.IncrementDeck(deckTypeEnum.Movement);
            if (controller.Player == 1)
            {
                playerOneData playerData = luckyPlayer.GetComponentInChildren<playerOneData>();
                playerData.storedMovement.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = luckyPlayer.GetComponentInChildren<playerTwoData>();
                playerData.storedMovement.Add(selectedInt);
            }
        }

        else if (moveCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Movement cards");
        }

        //This then clears the list of possible cards and turns the selected card to null and ends the player's turn
        possibleMovementCards = null;
        selectedMovementCard = null;
        StartCoroutine(EndLucky());
    }

    private void ObtainStatus()
    {
        //This obtains the character data of the possible status cards the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        playerController controller = luckyPlayer.GetComponent<playerController>();
        possibleStatusCards = controller.GetData.possibleStatusCards;
        selectedInt = Random.Range(0, possibleStatusCards.Count);
        selectedStatusCard = possibleStatusCards[selectedInt];

        //This while loops keep changing the card until the rarity of the chosen card is Legendary
        while (selectedStatusCard.cardRarity != CardRarity.Legendary)
        {
            selectedInt = Random.Range(0, possibleStatusCards.Count);
            selectedStatusCard = possibleStatusCards[selectedInt];
        }

        //This section checks if the player can obtain the status card in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the card
        //Otherwise they get nothing
        statusDeckPool statusPool = luckyPlayer.GetComponentInChildren<statusDeckPool>();
        GameObject statusCard = statusPool.GetAvailableStatus();
        if (statusCard != null)
        {
            statusCard.SetActive(true);
            statusCard status = statusCard.AddComponent<statusCard>();
            status.CreateCard(selectedStatusCard);
            eventText.SetText("Card Obtained: " + status.StatusCard.cardName);
            playerController.IncrementDeck(deckTypeEnum.Status);
            if (controller.Player == 1)
            {
                playerOneData playerData = luckyPlayer.GetComponentInChildren<playerOneData>();
                playerData.storedStatus.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = luckyPlayer.GetComponentInChildren<playerTwoData>();
                playerData.storedStatus.Add(selectedInt);
            }
        }

        else if (statusCard == null)
        {
            eventText.SetText("Unlucky, You don't have enough Status cards");
        }

        //This then clears the list of possible cards and turns the selected card to null and ends the player's turn
        possibleStatusCards = null;
        selectedStatusCard = null;
        StartCoroutine(EndLucky());
    }

    private void ObtainRelic()
    {
        //This obtains the character data of the possible relics the player can obtain
        //Then a random int occurs to apply the specifc relic for the player to obtain
        playerController controller = luckyPlayer.GetComponent<playerController>();
        possibleRelics = controller.GetData.possibleRelics;
        selectedInt = Random.Range(0, possibleRelics.Count);
        selectedRelic = possibleRelics[selectedInt];

        //This section checks if the player can obtain the relic in the first place
        //If the method to get an available item slot turns out not to be null then they obtain the relic
        //Otherwise they get nothing
        itemDeckPool itemDeck = luckyPlayer.GetComponentInChildren<itemDeckPool>();
        GameObject relic = itemDeck.GetAvailableItem();
        if(relic != null)
        {
            relic.SetActive(true);
            itemBehaviour item = relic.AddComponent<itemBehaviour>();
            item.CreateItem(selectedRelic);
            playerController.IncrementDeck(deckTypeEnum.Item);
            if (controller.Player == 1)
            {
                playerOneData playerData = luckyPlayer.GetComponentInChildren<playerOneData>();
                playerData.storedRelics.Add(selectedInt);
            }
            else if (controller.Player == 2)
            {
                playerTwoData playerData = luckyPlayer.GetComponentInChildren<playerTwoData>();
                playerData.storedRelics.Add(selectedInt);
            }
        }

        else if(relic == null)
        {
            eventText.SetText("Unlucky, You don't have any room for more relics");
        }

        //This then clears the list of possible relics and turns the selected relic to null and ends the player's turn
        selectedRelic = null;
        possibleRelics = null;
        StartCoroutine(EndLucky());
    }

    IEnumerator EndLucky()
    {
        yield return new WaitForSeconds(2);
        playerState.ChangeState(playerState.InactiveState);
        
    }
}
