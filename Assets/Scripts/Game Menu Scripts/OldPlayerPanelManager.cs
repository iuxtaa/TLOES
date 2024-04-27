using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OldPlayerPanelManager : MonoBehaviour
{
    [SerializeField] private OldPlayerPanel oldPlayerPanel;
    [SerializeField] private GameObject confirmationPanel;

    public void Awake()
    {
        if(oldPlayerPanel.enabled)
        {
            oldPlayerPanel.continueGameButton.onClick.AddListener(ContinueGameClicked);
            oldPlayerPanel.restartGameButton.onClick.AddListener(RestartGameClicked);
            oldPlayerPanel.quitGameButton.onClick.AddListener(QuitGameClicked); 
        }
    }

    private void ContinueGameClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.Farm); // Should change to saved currentLocation from database
    }

    private void RestartGameClicked()
    {
        oldPlayerPanel.gameObject.SetActive(false);
        confirmationPanel.SetActive(true);
    }

    private void QuitGameClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.StartingMenu);
    }
}
