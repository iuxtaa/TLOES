using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestGiver : MonoBehaviour // this will be an NPC which 'is a' character
{
    // VARIABLES
    public Quest quest; // the quest that the quest giver will give
    public QuestObjective questObjective; // the quest objective that the quest giver may have
    public Player player; // the player that will accept the quest
    public CollectableItems questItem;
    // public GameObject questOverlay;

    // METHODS
    public void acceptQuest()
    {
        if (quest != null && player != null)
        {
            player.acceptQuest(quest);

            if(questObjective != null)
            {
                if(questObjective is CollectingQuestObjective collectingQuestObjective)
                {
                    if(questItem.tag.Equals(CollectableItemsType.EMPTYBOTTLE.ToString()))
                    {
                        for(int i = 0; i < collectingQuestObjective.requiredAmount; i++)
                        {
                            player.inventory.Adding(questItem);
                        }
                    }
                }

                if(questObjective is SellingQuestObjective sellingQuestObjective)
                {
                    if(questItem.tag.Equals(CollectableItemsType.EGG.ToString()))
                    {
                        for(int i = 0; i < sellingQuestObjective.requiredSellingAmount; i++)
                        {
                            player.inventory.Adding(questItem);
                        }
                    }
                }
            }
            //questOverlay.SetActive(true);
        }
    }

    public void completeQuest()
    {
        if (quest != null && player != null)
        {
            quest.complete();
            //questOverlay.SetActive(false);
        }
    }
}
