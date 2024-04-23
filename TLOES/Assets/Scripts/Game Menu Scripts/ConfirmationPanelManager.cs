using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmationPanelManager : MonoBehaviour
{
    [SerializeField] private ConfirmationPanel confirmationPanel;
    [SerializeField] private GameObject oldPlayerPanel;

     public void Awake()
    {
        if(confirmationPanel.enabled)
        {
            confirmationPanel.yesButton.onClick.AddListener(YesClicked);
            confirmationPanel.noButton.onClick.AddListener(NoClicked);
        }
    }

    private void YesClicked()
    {
        SceneManager.LoadScene((int)ScreenEnum.Game);
    }

    private void NoClicked()
    {
        confirmationPanel.gameObject.SetActive(false);
        oldPlayerPanel.SetActive(true);
    }
}
