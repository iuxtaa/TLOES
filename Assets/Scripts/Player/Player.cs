using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using static Inventory;
using System.Linq;
using TMPro;

public class Player : Character
{

    #region Variables
    public static Player Instance { get; private set; }

  

    // CONSTANT VARIABLES
    public const int MAX_SLOTS = 5;
    // INSTANCE VARIABLES 
    public static int money = 0;
    public static int favourability;
    public GameEnding gameEnding;
    public GameObject endingPanel;  // Reference to the panel that contains the text
    public TMP_Text endingText;
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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return; // Ensure no further code execution happens on the destroyed object
        }

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

    #region FirebaseMethods
    public async Task SavePlayerData()
    {
        if (user == null)
            user = auth.CurrentUser;

        if (user != null)
        {
            PlayerData playerData = new PlayerData
            {
                money = Player.money,
                favourability = Player.favourability,
                inventory = inventory.slots,
                questData = new QuestData(currentQuest),
                questHistory = Array.ConvertAll(questHistory, quest => new QuestData(quest))
            };

            string json = JsonUtility.ToJson(playerData);
            await databaseReference.Child("users").Child(user.UserId).Child("playerData").SetRawJsonValueAsync(json);
        }
    }

    public async Task LoadPlayerData()
    {
        if (user == null)
            user = auth.CurrentUser;

        if (user != null)
        {
            var dataSnapshot = await databaseReference.Child("users").Child(user.UserId).Child("playerData").GetValueAsync();
            if (dataSnapshot.Exists)
            {
                string json = dataSnapshot.GetRawJsonValue();
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

                Player.money = playerData.money;
                Player.favourability = playerData.favourability;
                inventory.slots = playerData.inventory;
                currentQuest = new Quest(playerData.questData);
                questHistory = Array.ConvertAll(playerData.questHistory, data => new Quest(data));
            }
        }
    }
    #endregion
}

[Serializable]
public class PlayerData
{
    public int money;
    public int favourability;
    public List<InventorySlot> inventory;
    public QuestData questData;
    public QuestData[] questHistory;
}
