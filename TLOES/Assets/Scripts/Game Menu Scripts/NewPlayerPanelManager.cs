using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPlayerPanelManager : MonoBehaviour
{
    [SerializeField] private NewPlayerPanel newPlayerPanel;

    public void Awake() 
    {
        if(newPlayerPanel.enabled)
        {
            newPlayerPanel.newGameButton.onClick.AddListener(NewGameClicked);
            newPlayerPanel.quitGameButton.onClick.AddListener(QuitGameClicked);
        }
    }

    private void NewGameClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.Game);
    }

    private void QuitGameClicked()

    {
        SceneManager.LoadScene((int)ScreenEnum.StartingMenu);
    }
}
