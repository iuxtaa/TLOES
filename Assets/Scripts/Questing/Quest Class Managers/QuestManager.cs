using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest[] quests;
    //public QuestObjectiveManager questObjectiveManager;
    // Start is called before the first frame update
    void Start()
    {
        initializeQuests();
        //questObjectiveManager.initializeObjectives();
    }

    // Update is called once per frame
    void Update()
    {
        updateQuests();
    }

    // At the start of a NEW game, all the quests will start inactive and incomplete
    private void initializeQuests()
    {
        foreach(Quest quest in quests)
        {
            quest.isActive = false;
            quest.completionStatus = false;
        }
    }
    // If the quest is not complete and can be completed, the quest will be marked as complete
    private void updateQuests()
    {
        foreach(Quest quest in quests)
        {
            if (!quest.completionStatus && quest.canComplete())
                quest.complete();
        }
    }

    // Method for updating the isActive status of a particular quest
    private void updateQuestIsActive(Quest quest, bool newStatus)
    {
        quest.isActive = newStatus;
    }

    // Method for updating the completion status of a particular quest
    private void updateQuestCompletionStatus(Quest quest, bool newStatus)
    {
        quest.completionStatus = newStatus;
    }

}
