using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPanelManager : MonoBehaviour
{
    [Header("Starting Panel")]
    [SerializeField] private Panel startingPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject signupPanel;
    
    public static int menuChoice = (int)StartingMenuChoice.Unspecified;
    public void Start() // If scene is generated
    {
        startingPanel.loginButton.onClick.AddListener(LogInClicked);
        startingPanel.signUpButton.onClick.AddListener(SignUpClicked);
       

    }

    private void SignUpClicked()
    {
        HandleStartingPanel();
        signupPanel.SetActive(true);
        menuChoice = (int)StartingMenuChoice.Signup;
    }

    private void LogInClicked()
    {
        HandleStartingPanel();
        loginPanel.SetActive(true);
        menuChoice = (int)StartingMenuChoice.Login;
    }

    private void HandleStartingPanel()
    {
        startingPanel.gameObject.SetActive(false);
    }
}

public enum StartingMenuChoice {
    Unspecified,
    Signup,
    Login
}
