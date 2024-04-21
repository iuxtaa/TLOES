using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoingQuest : Quest
{
    // CONSTRUCTOR
    public DoingQuest()
    {
        this.questNumber = 0;

        this.title = string.Empty;
        this.description = string.Empty;
        this.favourabilityReward = 0;
        this.isActive = false;// Assuming the quest is active upon creation
    }

    // METHODS
}
