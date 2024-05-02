using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestOverlay : MonoBehaviour
{
    public Player player; // the player that will accept the quest

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void Update()
    {
        if (player != null && player.currentQuest != null)
        {
            if(player.currentQuest.isActive && !player.currentQuest.isComplete)
            {
                openQuestOverlay();
            }
        }
    }
    private void openQuestOverlay()
    {
        questWindow.SetActive(true);
        titleText.text = player.currentQuest.title;
        descriptionText.text = player.currentQuest.description;
    }

}
