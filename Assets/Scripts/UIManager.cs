
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour

{
    public static UIManager instance;

    //Screen object variables
    public GameObject LoginPanel;
    public GameObject SignupPanel;
     [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
    }

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        LoginPanel.SetActive(true);
        SignupPanel.SetActive(false);
    }
    public void RegisterScreen() // Regester button
    {
        LoginPanel.SetActive(false);
        SignupPanel.SetActive(true);
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
