using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestGiver : MonoBehaviour // this will be an NPC which 'is a' character
{
    public Quest quest; // the quest that the quest giver will give
    public Player player; // the player that will accept the quest

    public GameObject questWindow;
    public GameObject questOverlay;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void openQuestUI()
    {
        if (quest != null && player != null)
        {
            questWindow.SetActive(true);
            titleText.text = quest.title;
            descriptionText.text = quest.description;
            //goldText.text = quest.goldReward.ToString();
        }
    }

    public void acceptQuest()
    {
        if (quest != null && player != null)
        {
            questWindow.SetActive(false);
            player.acceptQuest(quest);
            questOverlay.SetActive(true) ;
        }
    }
}
