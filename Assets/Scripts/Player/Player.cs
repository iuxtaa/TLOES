using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{

    #region Variables
    public static Player Instance { get; private set; }

    // CONSTANT VARIABLES
    public const int MAX_SLOTS = 5;
    // INSTANCE VARIABLES 
    public static int favourability;
    public static Dictionary<string, int> tempinventory2 = new Dictionary<string, int>();  // Initialize inventory
    [SerializeField] public static Quest currentQuest;
    public Quest[] questHistory = new Quest[3];
    public VectorValue startingPosition;
    public Inventory inventory;
    public InventoryUI inventoryUI;


    #endregion

    #region Constructor

    public Player(string name) : base(name)
    {
        SetFavourability(0);
        SetQuest(null);
    }

    public Player(string name, int currentLocation, int favourability, Quest currentQuest) : base(name, currentLocation)
    {
        SetFavourability(favourability);
        SetQuest(currentQuest);
    }

    #endregion 

    #region SpawnMethods

    public void Awake()
    {
        transform.position = startingPosition.changingValue;
        startingPosition.changingValue = startingPosition.initialValue;
    }


    #endregion

    #region SetAndGetMethods

    // METHODS
    public void SetFavourability(int favourability)
    {
        Player.favourability = favourability;
    }

    public int GetFavourability()
    {
        return favourability;
    }

    public void SetQuest(Quest quest)
    {
        Player.currentQuest = quest;
    }

    public Quest GetQuest()
    {
        return currentQuest;
    }

    #endregion

    #region QuestingMethods
    public void acceptQuest(Quest quest)
    {
        SetQuest(quest);
        currentQuest.isActive = true;
        Debug.Log(Player.currentQuest);
    }

    public void completeQuest()
    {
        favourability += currentQuest.favourabilityReward;
        // FOR YZA
        // goldCount ?? += currentQuest.goldReward;
        currentQuest.complete();
        SetQuest(null);
    }

    public void failQuest()
    {
        favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        SetQuest(null);
    }

    private Quest findActiveQuest()
    {
        for (int i = 0; i < questHistory.Length; i++)
        {
            if (questHistory[i].isActive)
                return questHistory[i];
        }
        return null;
    }

    public void setActiveQuest()
    {
        SetQuest(findActiveQuest());
    }
    #endregion
}