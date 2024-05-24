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

    public void doObjective()
    {
        completionStatus = true;
    }

    public override bool checkCanComplete()
    {
        if(completionStatus)
        {
            complete();
            return true ;
        }
        return false;
    }

    public override string toString()
    {
        return description + " " + readableStatus();
    }

    private string readableStatus()
    {
        return (completionStatus ? "Done" : "");
    }
}
