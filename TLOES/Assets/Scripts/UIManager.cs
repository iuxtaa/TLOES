
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;

    [Header("Quest")]
    [SerializeField] private GameObject questScreen;

    public void Awake()
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        questScreen.SetActive(false);
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
    }

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
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

    // Save game button
    public void SaveGameButton()
    {
        Save();
    }

    // Save & Quit game button
    public void SaveAndQuitGameButton()
    {
        Save();
        SceneManager.LoadScene(((int)Screen.MainMenu));

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

    public enum Screen
    {
        StartingMenu,
        MainMenu,
        Game
    }
}
