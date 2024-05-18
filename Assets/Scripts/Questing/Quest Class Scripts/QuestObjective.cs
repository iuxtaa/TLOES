using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public string description;
    public bool isComplete;

    public QuestObjective(string description)
    {
        this.description = description;
        this.isComplete = false;
    }
}

