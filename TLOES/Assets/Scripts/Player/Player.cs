using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // INSTANCE VARIABLES 

    private int gold;
    private int favourability;
    // private Scene currentLocation;
    private Quest currentQuest;

    // CONSTRUCTOR
    public Player()
    {
        this.gold = 0;
        this.favourability = 0;
        // this.currentLocation = ;
        this.currentQuest = null;
    }

    #region GET & SET METHODS
    public int getGold()
    {
        return gold;
    }

    public void setGold(int gold)
    { this.gold = gold;}

    public int getFavourability()
    {
        return favourability;
    }

    public void setFavourability(int favourability)
    {  this.favourability = favourability;}

    public Quest getCurrentQuest()
    {
        return currentQuest;
    }

    public void setCurrentQuest(Quest quest)
    {
        currentQuest = quest;
    }

    #endregion

    // METHODS

    // ignoreQuest method. Player can choose to decline quest, this will lower their favourability by the favourability value of the quest
    public void declineQuest()
    {
        setFavourability(getFavourability() - currentQuest.getFavourabilityReward());
    }

    // acceptQuest method. Player can accept the quest and will get a slight increase in their favourability
    public void acceptQuest()
    {
        setFavourability(getFavourability() + Quest.DEFAULT_ACCEPT_QUEST_FAVOURABILITY_REWARD);

        if(currentQuest.getIsActive())
        {

        }
    }
}
