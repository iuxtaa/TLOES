using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // Unity will serialize these fields so that they will show up in the Inspector
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public abstract class Quest : ScriptableObject
{
    // PRIVATE INSTANCE VARIABLES
    public int questNumber;

    public bool isActive; // boolean for quest status
    public bool isComplete;

    public string title;
    public string description;
    public int favourabilityReward;

    // CONSTRUCTOR
    public Quest()
    {
        this.questNumber = 0;

        this.title = string.Empty;
        this.description = string.Empty;
        this.favourabilityReward = 0;
        this.isActive = false;// Assuming the quest is active upon creation
        this.isComplete = false;
    }

    public Quest(int questNumber, string title, string desc, int favourabilityReward)
    {
        this.questNumber=questNumber;
        this.title = title;
        this.description = desc;
        this.favourabilityReward=favourabilityReward;
        this.isActive = false;
        this.isComplete = false;
    }


    // METHODS

    public void complete()
    {
        isComplete = true;
        isActive = false;
        Debug.Log(title + " quest is completed. ");
    }
}
