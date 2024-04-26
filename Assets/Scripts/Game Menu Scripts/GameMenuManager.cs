using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    // Variable objects of the game menu scene
    [SerializeField] private UserKeybindsPanel userKeybindsPanel; 
    [SerializeField] private GameObject newPlayerPanel;
    [SerializeField] private GameObject oldPlayerPanel;

    // Local variable to store the value public static menuChoice of starting panel scene
    private int menuChoice = StartingPanelManager.menuChoice;

    // If scene is loaded
    public void Awake() 
    {
        userKeybindsPanel.exitButton.onClick.AddListener(DeactivateKeyBindsPanel);
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

    // If the player presses the exit button, call the function
    private void DeactivateKeyBindsPanel()
    {
        userKeybindsPanel.gameObject.SetActive(false);
        DisplayGameMenuPanel();
    }
}


