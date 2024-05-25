using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestGiver : MonoBehaviour // this will be an NPC which 'is a' character
{
    public Quest quest; // the quest that the quest giver will give
    public QuestObjective questObjective;
    public Player player; // the player that will accept the quest
    public CollectableItems questItem;
    public GameObject questOverlay;

    public void acceptQuest()
    {
        if (quest != null && player != null)
        {
            player.acceptQuest(quest);
            if(questObjective != null)
            {
                if(questObjective is CollectingQuestObjective collectingQuestObjective)
                {
                    
                }
                
                if(questObjective is SellingQuestObjective sellingQuestObjective)
                {
                    if(questItem.tag.Equals(CollectableItemsType.EGG.ToString()))
                    {
                        Debug.Log("Player has received the egg");
                        for(int i = 0; i < sellingQuestObjective.requiredSellingAmount; i++)
                        {
                            player.inventory.Adding(questItem);
                        }
                    }
                }
            }
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
