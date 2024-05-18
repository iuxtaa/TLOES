using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Quest System/Doing Quest Objective")]
public class DoingQuestObjective : QuestObjective
{
    public DoingQuestObjective(string description) : base(description)
    {
        
    }

    public override bool checkCanComplete()
    {
        throw new System.NotImplementedException();
    }

    public override string toString()
    {
        return description + completionStatus;
    }
}
