using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public int questNumber;
    public bool isActive;
    public bool completionStatus;
    public Quest dependentQuest;
    public string title;
    public string description;
    public int favourabilityReward;
    public int goldReward;

    public List<QuestObjective> objectives;

    public Quest()
    {
        this.questNumber = 0;
        this.title = string.Empty;
        this.description = string.Empty;
        this.favourabilityReward = 0;
        this.goldReward = 0;
        this.isActive = false;
        this.completionStatus = false;
        this.dependentQuest = null;
        this.objectives = new List<QuestObjective>();
    }

    public Quest(int questNumber, Quest dependentQuest, string title, string desc, int favourabilityReward, int goldReward)
    {
        this.questNumber = questNumber;
        this.dependentQuest = dependentQuest;
        this.title = title;
        this.description = desc;
        this.favourabilityReward = favourabilityReward;
        this.goldReward = goldReward;
        this.isActive = false;
        this.completionStatus = false;
        this.objectives = new List<QuestObjective>();
    }

    public bool canComplete()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (!objectives[i].completionStatus)
                return false;
        }
        return true;
    }

    public void complete()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowQuestCompletePopup(this);
        }
        this.completionStatus = true;
        this.isActive = false;
        Debug.Log(title + " quest is complete");
    }

    public string objectivesToString()
    {
        string output = "";
        for (int i = 0; i < objectives.Count; i++)
        {
            output += objectives[i].toString() + "\n";
        }
        return output;
    }

    public bool isDependent()
    {
        return (this.dependentQuest != null);
    }

    public bool isDependentQuestComplete()
    {
        if (this.dependentQuest != null)
            return this.dependentQuest.completionStatus;
        return true;
    }
}
public enum QuestIndex
{
    SellingEggs, KnightsLetter, PriestsHolyWater
}
