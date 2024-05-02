using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;



public class LoginPanelManager : MonoBehaviour
{
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
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            SceneManager.LoadScene((int)ScreenEnum.MainMenu);
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
    }

    private void HandleLoginPanel()
    {
        loginPanel.gameObject.SetActive(false);
    }
}
