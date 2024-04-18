using UnityEngine;

public class Player : MonoBehaviour
{
    // INSTANCE VARIABLES 
    public int gold;
    public int favourability;
    public Quest currentQuest;

    // CONSTRUCTOR
    public Player()
    {
        this.gold = 0;
        this.favourability = 0;
        this.currentQuest = null;
    }
    // METHODS

    // ignoreQuest method. Player can choose to decline quest, this will lower their favourability by the favourability value of the quest
    public void declineQuest()
    {
        if (currentQuest != null)
            favourability -= currentQuest.favourabilityReward;
    }

    // acceptQuest method. Player can accept the quest and will get a slight increase in their favourability
    public void acceptQuest()
    {
        if (currentQuest != null)
        {
            currentQuest.isActive = true;
            favourability += Quest.DEFAULT_ACCEPT_QUEST_FAVOURABILITY_REWARD;

            if (currentQuest.isActive == true && currentQuest is CollectionQuest)
            {
                ((CollectionQuest)currentQuest).collectItem();
                if (currentQuest.isReached())
                {
                    this.completeQuest();
                    currentQuest.complete();
                }
            }
        }
    }

    // Completing a quest will moderately increase your favourability
    private void completeQuest()
    {
        if (currentQuest != null)
            favourability += currentQuest.favourabilityReward;
    }
}