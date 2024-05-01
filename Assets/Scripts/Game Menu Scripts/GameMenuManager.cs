using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    // INSTANCE VARIABLES
    [SerializeField] private UserKeybindsPanel userKeybindsPanel;
    [SerializeField] private GameObject newPlayerPanel;
    [SerializeField] private GameObject oldPlayerPanel;
    private int menuChoice = StartingPanelManager.menuChoice;

    public void Awake() 
    {
       userKeybindsPanel.exitButton.onClick.AddListener(DeactivateKeybindsPanel);
    }

    private void DisplayGameMenuPanel()
    {
        if(menuChoice == (int)StartingMenuChoice.Login) 
        {
            oldPlayerPanel.SetActive(true);
            
        }
        if(menuChoice == (int)StartingMenuChoice.Signup)
        {
            newPlayerPanel.SetActive(true);
        }
    }

    private void DeactivateKeybindsPanel()
    {
        userKeybindsPanel.gameObject.SetActive(false);
        DisplayGameMenuPanel();
    }
}


