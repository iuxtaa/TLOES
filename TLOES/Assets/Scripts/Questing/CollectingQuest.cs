using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingQuest : Quest
{
    public int requiredAmount;
    public int currentAmount;

    public void Update()
    {
        // currentAmount = Player.inventory
    }

    public CollectingQuest(int requiredAmount, int currentAmount)
    {
        this.requiredAmount = requiredAmount;
        this.currentAmount = currentAmount;
    }

    public bool isReached()
    {
        return(currentAmount >= requiredAmount);
    }
}
