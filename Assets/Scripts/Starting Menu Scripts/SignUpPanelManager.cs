using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
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
            // Check if the username is already taken
            var usernameCheck = await databaseReference.Child("usernames").Child(username).GetValueAsync();
            if (usernameCheck.Exists)
            {
                warningText.text = "Username already in use!";
                return;
            }

            // Create user with email and password
            var authTask = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            var user = authTask.User;

            if (user != null)
            {
                // Save the username to the database
                await databaseReference.Child("usernames").Child(username).SetValueAsync(user.UserId);

                // Save additional user information if needed
                // Example: await databaseReference.Child("users").Child(user.UserId).SetValueAsync(new { username = username, email = email });

                // Send verification email
                await user.SendEmailVerificationAsync();
                warningText.text = "Verification email has been sent!";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Firebase authentication failed: " + e.Message);
            warningText.text = "Sign up failed. Please try again later.";
        }
    }

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