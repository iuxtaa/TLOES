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
    public GameObject questOverlay;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void OpenQuestUI()
    {
        if (quest != null && player != null)
        {
            questWindow.SetActive(true);
            titleText.text = quest.title;
            descriptionText.text = quest.description;
        }
    }

    public void AcceptQuest()
    {
        if (quest != null && player != null)
        {
            questWindow.SetActive(false);
            player.AcceptQuest(quest); // Call the correctly named method
            questOverlay.SetActive(true);
        }
    }
}
