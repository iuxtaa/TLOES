using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public Player player;

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI favourabilityText;
    public TextMeshProUGUI goldText;

    public void openQuestUI()
    {
        questWindow.SetActive(true);
        titleText.text = quest.getTitle();
        descriptionText.text = quest.getDescription();
        // favourabilityText.text = quest.favourabilityReward.ToString();
        goldText.text = quest.getGoldReward().ToString();
    }

    public void acceptQuest()
    {
        questWindow.SetActive(false);
        quest.setIsActive(true);

        // give quest to player
        player.setCurrentQuest(quest);
    }
}
