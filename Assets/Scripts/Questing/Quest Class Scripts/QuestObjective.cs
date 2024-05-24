using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Quest System/Quest Objective")]
public abstract class QuestObjective : ScriptableObject
{
    public string description;
    public bool completionStatus;

    public QuestObjective(string description)
    {
        this.description = description;
        this.completionStatus = false;
    }

    public abstract bool checkCanComplete();
    public void complete()
    {
        this.completionStatus = true;
    }
    public abstract string toString();
}

