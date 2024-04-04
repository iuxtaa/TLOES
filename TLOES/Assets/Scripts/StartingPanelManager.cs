using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPanelManager : MonoBehaviour
{
    [SerializeField] private Panel startingPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject signupPanel;
    
    public void Awake() // If scene is generated
    {
        startingPanel.loginButton.onClick.AddListener(LogInClicked);
        startingPanel.signUpButton.onClick.AddListener(SignUpClicked);
        
    }

    private void SignUpClicked()
    {
        HandleStartingPanel();
        signupPanel.SetActive(true);
    }

    private void LogInClicked()
    {
        HandleStartingPanel();
        loginPanel.SetActive(true);
    }

    private void HandleStartingPanel()
    {
        startingPanel.gameObject.SetActive(false);
    }
}
