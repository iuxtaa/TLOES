using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Unity will serialize these fields so that they will show up in the Inspector
public abstract class Quest
{
    public int questNum;
    public bool isActive;
    public string title;
    public string description;
    public int favourabilityReward;

    public Quest()
    {
        questNum = 0;
        isActive = false;
        title = string.Empty;
        description = string.Empty;
        favourabilityReward = 0;
    }

    public Quest(int questNum, bool isActive, string title, string description, int favourabilityReward)
    {
        this.questNum = questNum;
        this.isActive = isActive;
        this.title = title;
        this.description = description;
        this.favourabilityReward = favourabilityReward;
    }

    public void complete()
    {
        isActive = false;
        Debug.Log(questNum + title + "is complete");
    }
}
