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

    public bool isActive; // boolean for quest activity status
    public bool isComplete; // boolean for quest completion status

    public string title;
    public string description;
    public int favourabilityReward;

    public List<QuestObjective> objectives; // List of objectives for the quest

    // CONSTRUCTOR
    public Quest()
    {
        this.questNumber = 0;

        this.title = string.Empty;
        this.description = string.Empty;
        this.favourabilityReward = 0;
        this.isActive = false;
        this.isComplete = false;

        this.objectives = new List<QuestObjective>(); // Initialize the objectives list
    }

    
    public Quest(int questNumber, string title, string desc, int favourabilityReward)
    {
        this.questNumber = questNumber;
        this.title = title;
        this.description = desc;
        this.favourabilityReward = favourabilityReward;
        this.isActive = false;
        this.isComplete = false;

        this.objectives = new List<QuestObjective>(); // Initialize the objectives list
    }

    // METHODS

    public virtual void complete()
    {
        isComplete = true;
        Debug.Log("Completed Quest ");
    }

    public string objectivesToString()
    {
        string output = "";
        for (int i = 0; i < objectives.Count; i++)
        {
            output += "   " + objectives[i].objectiveToString() + "\n";
        }
        return output;
    }
}
