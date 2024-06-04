using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;

    [Header("Keybinds Panel")]
    [SerializeField] private UserKeybindsPanel userKeybindsPanel;

    [Header("Quest")]
    [SerializeField] private GameObject questOverlay;
    [SerializeField] private GameObject questCompletePopup;

    private QuestCompletePopup questCompletePopupScript;

    public void Awake()
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        questOverlay.SetActive(false);
        questCompletePopup.SetActive(false);

        questCompletePopupScript = questCompletePopup.GetComponent<QuestCompletePopup>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }

        if (Player.currentQuest != null && Player.currentQuest.isActive)
        {
            questOverlay.SetActive(true);
        }
        else
        {
            questOverlay.SetActive(false);
        }
    }

    public void ShowQuestCompletePopup(Quest quest)
    {
        if (questCompletePopupScript != null)
        {
            questCompletePopupScript.Initialize(quest);
            questCompletePopup.SetActive(true);
        }
    }

    public void PauseGame(bool status)
    {
        if (userKeybindsPanel.gameObject.activeInHierarchy)
        {
            pauseScreen.SetActive(!status);
        }
        else
        {
            pauseScreen.SetActive(status);
        }

        pauseButton.SetActive(!status);

        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void PauseButton()
    {
        PauseGame(true);
    }

    public void ResumeButton()
    {
        PauseGame(false);
    }

    public void ShowKeybindsButton()
    {
        userKeybindsPanel.exitButton.onClick.AddListener(DeactivateKeybindsPanel);
        userKeybindsPanel.gameObject.SetActive(true);
        PauseGame(true);
    }

    public void SaveGameButton()
    {
        Save();
    }

    public void SaveAndQuitGameButton()
    {
        Save();
        SceneManager.LoadScene((int)ScreenEnum.StartingMenu);
    }

    public void Save()
    {
        // Implement save functionality
    }

    public void DeactivateKeybindsPanel()
    {
        userKeybindsPanel.gameObject.SetActive(false);
        pauseScreen.SetActive(true);
    }
}
