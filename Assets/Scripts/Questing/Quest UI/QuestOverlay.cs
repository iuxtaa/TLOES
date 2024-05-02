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
        if (player != null && Player.currentQuest != null)
        {
            if(Player.currentQuest.isActive && !Player.currentQuest.isComplete)
            {
                openQuestOverlay();
            }
        }
    }
    private void openQuestOverlay()
    {
        questWindow.SetActive(true);
        titleText.text = Player.currentQuest.title;
        descriptionText.text = Player.currentQuest.description;
    }

}
