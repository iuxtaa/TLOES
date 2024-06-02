using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using Firebase;

public class Player : Character
{

    #region Variables

    // INSTANCE VARIABLES 
    public static int favourability;
    public static Dictionary<string, int> inventory = new Dictionary<string, int>();  // Initialize inventory
    [SerializeField] public static Quest currentQuest;
    public Quest[] questHistory = new Quest[3];
    public VectorValue startingPosition;
    private FirebaseAuth auth;
    private DatabaseReference databaseReference;
    private FirebaseUser user;

    #endregion 

    #region Constructor

    public Player(string name) : base(name)
    {
        InitializeFirebase();
        SetFavourability(0);
        SetQuest(null);
        //questHistory[0] = new SellingQuest(3,3, 0, "", "", 5);
        //questHistory[1] = KnightsLetter;
        //questHistory[2] = PriestsHolyWater;
    }

    public Player(string name, int currentLocation, int favourability, Quest currentQuest) : base(name, currentLocation)
    {
        InitializeFirebase();
        SetFavourability(favourability);
        SetQuest(currentQuest);
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        user = auth.CurrentUser;
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
        SavePlayerData();
    }

    public int GetFavourability()
    {
        return favourability;
    }

    public void SetQuest(Quest quest)
    {
        Player.currentQuest = quest;
        SavePlayerData();
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
        SavePlayerData();
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
        SavePlayerData();
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
        SavePlayerData();
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
        if (CanCompleteQuest())
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
            SavePlayerData();
        }
    }

    public void failQuest()
    {
        favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        SetQuest(null);
        SavePlayerData();
    }
    #endregion

    #region FirebaseMethods

    private async void SavePlayerData()
    {
        if (user == null)
            return;

        PlayerData playerData = new PlayerData
        {
            favourability = favourability,
            inventory = inventory,
            currentQuest = currentQuest != null ? new QuestData(currentQuest) : null,
            startingPosition = new PositionData(startingPosition.changingValue, startingPosition.initialValue),
            isQuestActive = currentQuest != null ? currentQuest.isActive : false,
            isQuestComplete = currentQuest != null ? currentQuest.isComplete : false
        };

        string json = JsonUtility.ToJson(playerData);
        await databaseReference.Child("players").Child(user.UserId).SetRawJsonValueAsync(json);
    }

    #endregion
}

[Serializable]
public class PlayerData
{
    public int favourability;
    public Dictionary<string, int> inventory;
    public QuestData currentQuest;
    public PositionData startingPosition;
    public bool isQuestActive;
    public bool isQuestComplete;
}

[Serializable]
public class QuestData
{
    public int questNumber;
    public string title;
    public string description;
    public int favourabilityReward;
    public bool isActive;
    public bool isComplete;

    public QuestData(Quest quest)
    {
        questNumber = quest.questNumber;
        title = quest.title;
        description = quest.description;
        favourabilityReward = quest.favourabilityReward;
        isActive = quest.isActive;
        isComplete = quest.isComplete;
    }
}

[Serializable]
public class PositionData
{
    public Vector3 changingValue;
    public Vector3 initialValue;

    public PositionData(Vector3 changingValue, Vector3 initialValue)
    {
        this.changingValue = changingValue;
        this.initialValue = initialValue;
    }
}
