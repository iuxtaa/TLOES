using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestOverlay : MonoBehaviour
{
    public Quest quest; // the quest that the quest giver will give
    public Player player; // the player that will accept the quest

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void Update()
    {
        if (quest != null && player != null)
        {
            if(quest.isActive && !quest.isComplete)
            {
                openQuestOverlay();
            }
        }
    }
    private void openQuestOverlay()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
    }

}
