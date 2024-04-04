using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPanelManager : MonoBehaviour
{
    [SerializeField] private Panel loginPanel;
    [SerializeField] public GameObject signUpPanel;

    public void Awake() 
    {
        loginPanel.loginButton.onClick.AddListener(LoginClicked);
        loginPanel.signUpButton.onClick.AddListener(SignUpClicked);
    }

    private void SignUpClicked()
    {
        HandleLoginPanel();
        signUpPanel.SetActive(true);
    }

    private void LoginClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.MainMenu);
    }

    private void HandleLoginPanel()
    {
        loginPanel.gameObject.SetActive(false);
    }

   
}
