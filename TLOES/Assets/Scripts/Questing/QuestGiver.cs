using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest; // the quest that the quest giver will give
    public Player player; // the player that will accept the quest

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI favourabilityText;
    public TextMeshProUGUI goldText;

    public void openQuestUI()
    {
        if (quest != null && player != null)
        {
            questWindow.SetActive(true);
            titleText.text = quest.title;
            descriptionText.text = quest.description;
            goldText.text = quest.goldReward.ToString();
        }
    }

    public void acceptQuest()
    {
        if (quest != null && player != null)
        {
            questWindow.SetActive(false);
            quest.isActive = true;

            // give quest to player
            player.currentQuest = quest;
        }
    }
}
