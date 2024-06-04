using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using static Inventory;

public class Player : Character
{

    #region Variables
    public static Player Instance { get; private set; }

    // CONSTANT VARIABLES
    public const int MAX_SLOTS = 5;
    // INSTANCE VARIABLES 
    public static int money = 0;
    public static int favourability;
    public static Dictionary<string, int> tempinventory2 = new Dictionary<string, int>();  // Initialize inventory
    [SerializeField] public static Quest currentQuest;
    public Quest[] questHistory = new Quest[3];
    public PlayerVectorValue startingPosition;
    public Inventory inventory;
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
        // Adjust this logic based on your new Inventory class
        foreach (var slot in inventory.slots)
        {
            if (slot.type.ToString() == item && slot.CanAddItem())
            {
                slot.count += quantity;
                SavePlayerData();
                return;
            }
        }

        foreach (var slot in inventory.slots)
        {
            if (slot.type == CollectableItemsType.NONE)
            {
                slot.type = (CollectableItemsType)Enum.Parse(typeof(CollectableItemsType), item);
                slot.count = quantity;
                SavePlayerData();
                return;
            }
        }
    }

    public void RemoveItem(string item, int quantity)
    {
        // Adjust this logic based on your new Inventory class
        foreach (var slot in inventory.slots)
        {
            if (slot.type.ToString() == item)
            {
                slot.count -= quantity;
                if (slot.count <= 0)
                {
                    slot.type = CollectableItemsType.NONE;
                    slot.count = 0;
                    slot.Icon = null;
                }
                SavePlayerData();
                return;
            }
        }
    }

    public int GetItemCount(string item)
    {
        // Adjust this logic based on your new Inventory class
        foreach (var slot in inventory.slots)
        {
            if (slot.type.ToString() == item)
            {
                return slot.count;
            }
        }
        return 0;
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

    public void completeQuest()
    {
        favourability += currentQuest.favourabilityReward;
        // FOR YZA
        // goldCount ?? += currentQuest.goldReward;
        currentQuest.complete();
        SetQuest(null);
        SavePlayerData();
    }

    public void failQuest()
    {
        favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        SetQuest(null);
        SavePlayerData();
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

    #region FirebaseMethods

    private async void SavePlayerData()
    {
        if (user == null)
            return;

        PlayerData playerData = new PlayerData
        {
            favourability = favourability,
            inventory = inventory.slots, // Save slots
            currentQuest = currentQuest != null ? new QuestData(currentQuest) : null,
            startingPosition = new PositionData(startingPosition.changingValue, startingPosition.initialValue),
            isQuestActive = currentQuest != null ? currentQuest.isActive : false,
            isQuestComplete = currentQuest != null ? currentQuest.completionStatus : false
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
    public List<Inventory.Slot> inventory; // Changed to List<Slot>
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
    public int goldReward;
    public List<QuestObjectiveData> objectives; // Include objectives

    public QuestData(Quest quest)
    {
        questNumber = quest.questNumber;
        title = quest.title;
        description = quest.description;
        favourabilityReward = quest.favourabilityReward;
        isActive = quest.isActive;
        isComplete = quest.completionStatus;
        goldReward = quest.goldReward;
        objectives = new List<QuestObjectiveData>();

        foreach (var objective in quest.objectives)
        {
            objectives.Add(new QuestObjectiveData(objective));
        }
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

[Serializable]
public class QuestObjectiveData
{
    public string description;
    public bool completionStatus;

    public QuestObjectiveData(QuestObjective objective)
    {
        description = objective.description;
        completionStatus = objective.completionStatus;
    }
}