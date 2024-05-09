
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
    [SerializeField] private GameObject questPopup;
    [SerializeField] private GameObject questOverlay;
    [SerializeField] private GameObject questCompletePopup;

    public void Awake()
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        questPopup.SetActive(false);
        questOverlay.SetActive(false);
        questCompletePopup.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // If pause screen is already active, unpause
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            // If pause screen not active, pause.
            else
                PauseGame(true);
        }
        if (Player.currentQuest != null && Player.currentQuest.isActive)
        {
            questOverlay.SetActive(true);
        }
    }

    #region Pause
    public void PauseGame(bool status)
    {
        if(userKeybindsPanel.gameObject.activeInHierarchy)
        {
            pauseScreen.SetActive(!status);
        }
        else
        {
            pauseScreen.SetActive(status);
        }

        pauseButton.SetActive(!status);

        if (status) // If status is true, pause the game
            Time.timeScale = 0;
        else // If status is false, unpause the game
            Time.timeScale = 1;
    }

    // PAUSE MENU FUNCTIONS

    // Pause button
    public void PauseButton()
    {
        PauseGame(true);
    }

    // Resume button
    public void ResumeButton()
    {
        PauseGame(false);
    }

    // Show Keybinds button
    public void ShowKeybindsButton()
    {
        userKeybindsPanel.exitButton.onClick.AddListener(DeactivateKeybindsPanel);
        userKeybindsPanel.gameObject.SetActive(true);
        PauseGame(true);
    }

    // Save game button
    public void SaveGameButton()
    {
        Save();
    }

    // Save & Quit game button
    public void SaveAndQuitGameButton()
    {
        Save();
        SceneManager.LoadScene((int)ScreenEnum.StartingMenu);

        //Application.Quit();

        //#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false; // Exits play mode (will only be executed in the editor)
        //#endif
    }

    // Save game
    public void Save()
    {

    }
    #endregion

    #region Keybinds
    
    //Deactivates the keybinds panel when the pause button is clicked
    public void DeactivateKeybindsPanel()
    {
        userKeybindsPanel.gameObject.SetActive(false);
        pauseScreen.SetActive(true);
    }
    #endregion
}
