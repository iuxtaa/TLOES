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

    public GameObject questOverlay;
    public void acceptQuest()
    {
        if (quest != null && player != null)
        {
            player.acceptQuest(quest);
            questOverlay.SetActive(true);
        }
    }

    public void completeQuest()
{
    if (quest != null && player != null)
    {
        quest.complete();
        questOverlay.SetActive(false);
    }
}

}
