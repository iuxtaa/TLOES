using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmationPanelManager : MonoBehaviour
{
    // Variable objects to manage the confirmation panel
    [SerializeField] private ConfirmationPanel confirmationPanel;
    [SerializeField] private GameObject oldPlayerPanel;

    // If the scene is loaded
     public void Awake()
    {
        if(confirmationPanel.enabled)
        {
            confirmationPanel.yesButton.onClick.AddListener(YesClicked);
            confirmationPanel.noButton.onClick.AddListener(NoClicked);
        }
    }

    // Move to the game scene
    private void YesClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.Farm);
    }

    private void NoClicked()
    {
        confirmationPanel.gameObject.SetActive(false);
        oldPlayerPanel.SetActive(true);
    }
}
