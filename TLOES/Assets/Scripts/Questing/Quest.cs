using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest
{
    // PRIVATE INSTANCE VARIABLES

    private bool isActive;

    public static int DEFAULT_ACCEPT_QUEST_FAVOURABILITY_REWARD = 1;

    private string title;
    private string description;
    private int favourabilityReward;
    private int goldReward;

    #region GET & SET METHODS
    public bool getIsActive()
    {
        return isActive;
    }

    public void setIsActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public string getTitle() 
    {
        return title;
    }

    public void setTitle(string title)
    {
        this.title = title;
    }

    public string getDescription()
    {
        return description;
    }

    public void setDescription(string description)
    {
        this.description = description;
    }

    public int getFavourabilityReward()
    {
        return favourabilityReward;
    }

    public void setFavourabilityReward(int favourabilityReward)
    {
        this.favourabilityReward = favourabilityReward;
    }

    public int getGoldReward()
    {
        return goldReward;
    }

    public void setGoldReward(int goldReward)
    {
        this.goldReward = goldReward;
    }

    #endregion

    // METHODS

    public void complete()
    {
        isActive = false;
        Debug.Log(getTitle() + "quest is completed. ");
    }

    
}
