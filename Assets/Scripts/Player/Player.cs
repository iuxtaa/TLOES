using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character {

    #region Variables

    // CONSTANT VARIABLES
    public const int MAX_SLOTS = 5;
    // INSTANCE VARIABLES 
    public static int favourability;
    public static Dictionary<string, int> inventory = new Dictionary<string, int>();  // Initialize inventory
    [SerializeField] public static Quest currentQuest;
    public Quest[] questHistory = new Quest[3];
    public VectorValue startingPosition;
   
    public Inventory tempInventory;// this is the temperary code for the inventory
    


    #endregion

    #region Constructor

    public Player(string name) : base(name)
    {
        SetFavourability(0);
        SetQuest(null);
        //questHistory[0] = new SellingQuest(3,3, 0, "", "", 5);
        //questHistory[1] = KnightsLetter;
        //questHistory[2] = PriestsHolyWater;
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
        tempInventory = new Inventory(MAX_SLOTS);//temperary inventory stuff might delete later
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

    #region InventoryMethods 
    public void AddItem(string item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            inventory.Add(item, quantity);
        }
    }

    public void RemoveItem(string item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= quantity;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }

    }

    public int GetItemCount(string item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }
    #endregion 

    #region QuestingMethods
    public void acceptQuest(Quest quest)
    {
        SetQuest(quest);
        currentQuest.isActive = true;
        Debug.Log(Player.currentQuest);
    }

    public bool CanCompleteQuest()
    {
        if (currentQuest != null)
        {
            if (currentQuest is SellingQuest sellingQuest)
            {
                return GetItemCount(sellingQuest.requiredItem.name) >= sellingQuest.requiredAmount;
            }
            if (currentQuest is DoingQuest)
            {
                return true;
            }
            if (currentQuest is CollectingQuest collectingQuest)
            {
                return GetItemCount(collectingQuest.requiredItem.name) >= collectingQuest.requiredAmount;
            }
        }
        return false;
    }

    public void completeQuest()
    {
        if(CanCompleteQuest())
        {
            favourability += currentQuest.favourabilityReward;
            if (currentQuest is CollectingQuest collectingQuest)
            {
                RemoveItem(collectingQuest.requiredItem.name, collectingQuest.requiredAmount);
            }
            else if (currentQuest is SellingQuest sellingQuest)
            {
                RemoveItem(sellingQuest.requiredItem.name, sellingQuest.requiredAmount);

            }
            currentQuest.complete();
            SetQuest(null);
        }
    }

    public void failQuest()
    {
        favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        SetQuest(null);
    }
    #endregion
}