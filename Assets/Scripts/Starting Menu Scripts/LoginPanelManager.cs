using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;

public class LoginPanelManager : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private Panel loginPanel;
    [SerializeField] public GameObject signUpPanel;
    public TMP_Text warningText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;

    private FirebaseAuth auth;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (loginPanel.enabled)
        {
            loginPanel.loginButton.onClick.AddListener(LoginClicked);
            loginPanel.signUpButton.onClick.AddListener(SignUpClicked);
        }
    }

    private async void LoginClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            warningText.text = "Please enter email and password!";
            return;
        }

        try
        {
            var authTask = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var user = authTask.User;

            if (user != null)
            {
                await user.ReloadAsync(); // Reload user data to get the latest email verification status
                if (user.IsEmailVerified)
                {
                    SceneManager.LoadScene((int)ScreenEnum.MainMenu);
                }
                else
                {
                    warningText.text = "Please verify your email!";
                }
            }
            else
            {
                warningText.text = "User not found. Please check your credentials.";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Firebase authentication failed: " + e.Message);
            warningText.text = "Authentication failed. Please check your credentials and try again.";
        }
    }

    private void SignUpClicked()
    {
        HandleLoginPanel();
        signUpPanel.SetActive(true);
        StartingPanelManager.menuChoice = (int)StartingMenuChoice.Signup;

        // Reset the login panel fields and warning text
        emailInput.text = "";
        passwordInput.text = "";
        warningText.text = "";
    }

    private void HandleLoginPanel()
    {
        loginPanel.gameObject.SetActive(false);
    }
}


