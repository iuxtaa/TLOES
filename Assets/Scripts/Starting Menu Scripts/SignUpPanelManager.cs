using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SignUpPanelManager : MonoBehaviour
{
    [Header("Sign Up")]
    [SerializeField] private Panel signUpPanel;
    [SerializeField] private GameObject loginPanel;
    public TMP_Text warningText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;
    public Button signUpButton;

    private FirebaseAuth auth;
    private DatabaseReference databaseReference;
    private FirebaseUser user;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (signUpPanel.enabled)
        {
            signUpPanel.loginButton.onClick.AddListener(LoginClicked);
            signUpPanel.signUpButton.onClick.AddListener(SignUpClicked);
        }
    }

    private async void SignUpClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string username = usernameInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
        {
            warningText.text = "Please enter email, password, and username!";
            return;
        }

        try
        {
            if (user == null)
            {
                var authTask = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
                user = authTask.User;

                if (user != null)
                {
                    await user.SendEmailVerificationAsync();
                    await SaveUsernameToDatabase(user.UserId, username, email);
                   
                    warningText.text = "Verification email has been sent. Please verify your email and click signup again";
                }
            }
            else
            {
                await user.ReloadAsync(); // Reload user data to get the latest email verification status
                if (user.IsEmailVerified)
                {
                    SceneManager.LoadScene((int)ScreenEnum.MainMenu);
                }
                else
                {
                    warningText.text = "Please verify your email and click signup again.";
                }
            }
        }
        catch (AggregateException ae)
        {
            foreach (var innerException in ae.InnerExceptions)
            {
                Debug.LogError("Firebase authentication failed: " + innerException.Message);
                warningText.text = "Sign up failed. Please try again later.";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Firebase authentication failed: " + e.Message);
            warningText.text = "Sign up failed. Please try again later.";
        }
    }

    private async Task SaveUsernameToDatabase(string userId, string username, string email)
    {
        try
        {
            await databaseReference.Child("users").Child(userId).Child("username").SetValueAsync(username);
            await databaseReference.Child("users").Child(userId).Child("email").SetValueAsync(email);
        }
        catch (AggregateException ae)
        {
            foreach (var innerException in ae.InnerExceptions)
            {
                Debug.LogError("Saving username to database failed: " + innerException.Message);
                warningText.text = "Failed to save username to database.";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Saving username to database failed: " + e.Message);
            warningText.text = "Failed to save username to database.";
        }
    }

<<<<<<< Updated upstream
    private async Task InitializePlayerData(string userId)
    {
        try
        {
            PlayerData initialPlayerData = new PlayerData
            {
                favourability = 0,
                inventory = new List<Inventory.Slot>(), // Initialize inventory as a List<Inventory.Slot>
                currentQuest = null,
                startingPosition = new PositionData(Vector3.zero, Vector3.zero),
                isQuestActive = false,
                isQuestComplete = false
            };

            string json = JsonUtility.ToJson(initialPlayerData);
            await databaseReference.Child("players").Child(userId).SetRawJsonValueAsync(json);
        }
        catch (AggregateException ae)
        {
            foreach (var innerException in ae.InnerExceptions)
            {
                Debug.LogError("Initializing player data failed: " + innerException.Message);
                warningText.text = "Failed to initialize player data.";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Initializing player data failed: " + e.Message);
            warningText.text = "Failed to initialize player data.";
        }
    }
=======
  
>>>>>>> Stashed changes

    private void LoginClicked()
    {
        HandleSignUpPanel();
        loginPanel.SetActive(true);
        StartingPanelManager.menuChoice = (int)StartingMenuChoice.Login;
    }

    private void HandleSignUpPanel()
    {
        signUpPanel.gameObject.SetActive(false);
    }
}
