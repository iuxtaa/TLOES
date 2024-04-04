using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpPanelManager : MonoBehaviour
{
    [SerializeField] private Panel signUpPanel;
    [SerializeField] private GameObject loginPanel;
    
    public void Awake() 
    {
        signUpPanel.loginButton.onClick.AddListener(LoginClicked);
        signUpPanel.signUpButton.onClick.AddListener(SignUpClicked);
    }

    public void SignUpClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.MainMenu);
    }

    private void LoginClicked()
    {
        HandleSignUpPanel();
        loginPanel.SetActive(true);
    }

    private void HandleSignUpPanel(){
        signUpPanel.gameObject.SetActive(false);
    }
}
