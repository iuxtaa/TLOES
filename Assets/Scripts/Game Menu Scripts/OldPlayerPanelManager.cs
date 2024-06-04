using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OldPlayerPanelManager : MonoBehaviour
{
    [SerializeField] private OldPlayerPanel oldPlayerPanel;
    [SerializeField] private GameObject confirmationPanel;
    private FirebaseAuth auth;
    private DatabaseReference databaseReference;

    public void Awake()
    {
        if (oldPlayerPanel.enabled)
        {
            oldPlayerPanel.continueGameButton.onClick.AddListener(ContinueGameClicked);
            oldPlayerPanel.restartGameButton.onClick.AddListener(RestartGameClicked);
            oldPlayerPanel.quitGameButton.onClick.AddListener(QuitGameClicked);
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError($"Failed to initialize Firebase: {dependencyStatus}");
            }
        });
    }

    private void ContinueGameClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.Market);
        FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser != null)
        {
            databaseReference.Child("Playerdata").Child(currentUser.UserId).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        PlayerData playerData = JsonUtility.FromJson<PlayerData>(snapshot.GetRawJsonValue());
                        LoadPlayerDataCallback(playerData);

                        // Transition to the Farm scene
                        SceneManager.LoadScene((int)ScreenEnum.Farm);
                    }
                    else
                    {
                        Debug.LogWarning("Player data not found.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to retrieve player data: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("Current user is null. Make sure the user is authenticated.");
        }
    }

    private void LoadPlayerDataCallback(PlayerData playerData)
    {
        if (playerData != null)
        {
            // Set the loaded player data to the Player class
            Player.money = playerData.money;
            Player.favourability = playerData.favourability;
            Player.currentQuest = playerData.currentQuest;
        }
        else
        {
            Debug.LogWarning("Player data not found.");
        }
    }

    private void RestartGameClicked()
    {
        oldPlayerPanel.gameObject.SetActive(false);
        confirmationPanel.SetActive(true);
        FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser != null)
        {
            PlayerData resetData = new PlayerData(0, 0, null);
            string json = JsonUtility.ToJson(resetData);

            databaseReference.Child("Playerdata").Child(currentUser.UserId).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player data reset successfully.");
                    // Update the game to reflect the reset data
                    Player.money = 0;
                    Player.favourability = 0;
                    Player.currentQuest = null;

                    // Show confirmation panel

                }
                else
                {
                    Debug.LogError("Failed to reset player data: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("Current user is null. Make sure the user is authenticated.");
        }
    }

    private void QuitGameClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.StartingMenu);
    }

    [System.Serializable]
    public class PlayerData
    {
        public int money;
        public int favourability;
        public Quest currentQuest;

        public PlayerData(int money, int favourability, Quest currentQuest)
        {
            this.money = money;
            this.favourability = favourability;
            this.currentQuest = currentQuest;
        }
    }
}