using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Quest System/Quest Objective")]
public abstract class QuestObjective : ScriptableObject
{
    public QuestObjective dependentObjective;
    public string description;
    public bool completionStatus;

    public QuestObjective(QuestObjective dependentObjective, string description)
    {
        this.dependentObjective = dependentObjective;
        this.description = description;
        this.completionStatus = false;
    }

    public abstract bool checkCanComplete();
    public void complete()
    {
        this.completionStatus = true;
    }

    public bool isDependent()
    {
        return(this.dependentObjective != null);
    }

    public bool isDependentObjectiveComplete()
    {
        if(this.dependentObjective != null)
            return (this.dependentObjective.completionStatus);
        return true;
    }

    public abstract string toString();
}

