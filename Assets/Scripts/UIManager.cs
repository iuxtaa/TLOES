using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;

    [Header("Keybinds Panel")]
    [SerializeField] private UserKeybindsPanel userKeybindsPanel;

    [Header("Quest")]
    [SerializeField] private GameObject questOverlay;
    [SerializeField] private GameObject questCompletePopup;
    private FirebaseAuth auth;
    private DatabaseReference databaseReference;

    public void Awake()
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        questOverlay.SetActive(false);
        questCompletePopup.SetActive(false);

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If pause screen is already active, unpause
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            // If pause screen not active, pause.
            else
                PauseGame(true);
        }
        if (Player.currentQuest != null && Player.currentQuest.isActive)
        {
            questOverlay.SetActive(true);
        }
    }

    #region Pause
    public void PauseGame(bool status)
    {
        if (userKeybindsPanel.gameObject.activeInHierarchy)
        {
            pauseScreen.SetActive(!status);
        }
        else
        {
            pauseScreen.SetActive(status);
        }

        pauseButton.SetActive(!status);

        if (status) // If status is true, pause the game
            Time.timeScale = 0;
        else // If status is false, unpause the game
            Time.timeScale = 1;
    }

    // PAUSE MENU FUNCTIONS

    // Pause button
    public void PauseButton()
    {
        PauseGame(true);
    }

    // Resume button
    public void ResumeButton()
    {
        PauseGame(false);
    }

    // Show Keybinds button
    public void ShowKeybindsButton()
    {
        userKeybindsPanel.exitButton.onClick.AddListener(DeactivateKeybindsPanel);
        userKeybindsPanel.gameObject.SetActive(true);
        PauseGame(true);
    }

    // Save game button
    public void SaveGameButton()
    {
        SavePlayerData();
    }

    // Save & Quit game button
    public void SaveAndQuitGameButton()
    {
        SavePlayerData();
        SceneManager.LoadScene((int)ScreenEnum.StartingMenu);

        //Application.Quit();

        //#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false; // Exits play mode (will only be executed in the editor)
        //#endif
    }

    // Save game
    public void Save()
    {

    }
    #endregion

    #region Keybinds

    //Deactivates the keybinds panel when the pause button is clicked
    public void DeactivateKeybindsPanel()
    {
        userKeybindsPanel.gameObject.SetActive(false);
        pauseScreen.SetActive(true);
    }
    #endregion

    private void SavePlayerData()
    {
        if (auth != null && databaseReference != null)
        {
            FirebaseUser currentUser = auth.CurrentUser;
            if (currentUser != null)
            {
                // Create a data object to store player data
                PlayerData playerData = new PlayerData(Player.money, Player.favourability);

                // Convert the player data object to JSON
                string json = JsonUtility.ToJson(playerData);

                // Save the JSON data to Firebase under the user's ID
                databaseReference.Child("Playerdata").Child(currentUser.UserId).SetRawJsonValueAsync(json)
                    .ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            Debug.Log("Player data saved successfully!");
                        }
                        else
                        {
                            Debug.LogError("Failed to save player data: " + task.Exception);
                        }
                    });
            }
            else
            {
                Debug.LogError("Current user is null. Make sure the user is authenticated.");
            }
        }
        else
        {
            Debug.LogError("Firebase is not initialized. Make sure Firebase is set up properly.");
        }
    }
    public class PlayerData
    {
        public int money;
        public int favourability;

        public PlayerData(int money, int favourability)
        {
            this.money = money;
            this.favourability = favourability;
        }
    }
}