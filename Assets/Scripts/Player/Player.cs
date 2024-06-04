using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System;

public class Player : Character
{
    #region Variables
    public static Player Instance { get; private set; }

    public const int MAX_SLOTS = 5;
    public static int money = 0;
    public static int favourability;
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
        foreach (var slot in inventory.slots)
        {
            if (slot.type.ToString() == item && slot.CanAddItem())
            {
                slot.count += quantity;
                return;
            }
        }

        foreach (var slot in inventory.slots)
        {
            if (slot.type == CollectableItemsType.NONE)
            {
                slot.type = (CollectableItemsType)Enum.Parse(typeof(CollectableItemsType), item);
                slot.count = quantity;
                return;
            }
        }
    }

    public void RemoveItem(string item, int quantity)
    {
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
                return;
            }
        }
    }

    public int GetItemCount(string item)
    {
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
        Debug.Log(Player.currentQuest);
    }

    public void completeQuest()
    {
        favourability += currentQuest.favourabilityReward;
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

[Serializable]
public class PlayerData
{
    public int money;
    public int favourability;
}
