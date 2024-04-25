using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPlayerPanelManager : MonoBehaviour
{
    // Variable object of the new player panel in the game menu scene
    [SerializeField] private NewPlayerPanel newPlayerPanel;

    // If the scene is loaded and new player panel is enabled, add the following listeners
    public void Awake() 
    {
        if(newPlayerPanel.enabled)
        {
            newPlayerPanel.newGameButton.onClick.AddListener(NewGameClicked);
            newPlayerPanel.quitGameButton.onClick.AddListener(QuitGameClicked);
        }
    }

    // If the new game button is clicked, move onto the next scene
    private void NewGameClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.Farm);
    }

    // If the quit game button is clicked, go back to the starting menu scene
    private void QuitGameClicked()

    {
        SceneManager.LoadScene((int)ScreenEnum.StartingMenu);
    }
}
