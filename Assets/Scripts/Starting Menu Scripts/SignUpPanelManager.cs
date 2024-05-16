using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Firebase;
using Firebase.Auth;
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

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;

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
            var authTask = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            var user = authTask.User;

            // Send verification email
            if (user != null)
            {
                await user.SendEmailVerificationAsync();
                warningText.text = "Verification email has been sent!";
            }

            // Optionally, you can save the username to a database or user profile
            // Example: SaveUsernameToDatabase(username);
        }
        catch (System.Exception e)
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
