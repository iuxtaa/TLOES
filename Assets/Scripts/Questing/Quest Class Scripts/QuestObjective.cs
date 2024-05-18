using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public string description;
    public bool isActive;
    public bool isComplete;
    public State state;

    public QuestObjective(string description, State state)
    {
        this.description = description;
        this.isComplete = false;
        this.state = state;
    }

    public string objectiveToString()
    {
        if(state == State.NUMBERABLE)
        {
            if (Player.currentQuest is SellingQuest sellingQuest)
                return description + " " + sellingQuest.progress();
            if (Player.currentQuest is CollectingQuest collectingQuest)
                return description + " " + collectingQuest.progress();
            else
                return description;
        }
        else 
        {
            return description + " " + isComplete;
        }
    }

    public enum State
    {
        NUMBERABLE,
        SIMPLE
    }

}

