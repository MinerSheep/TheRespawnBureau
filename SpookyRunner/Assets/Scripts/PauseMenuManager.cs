using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Set to Canvas_Pause in the editor
    private bool isPaused = false;

    private void Start()
    {
        if (pauseMenuUI == null)
            pauseMenuUI = GameObject.FindGameObjectWithTag("Pause");

        // Start with pause menu disabled
        pauseMenuUI.SetActive(false);

        SetUpButtons();
    }

    void SetUpButtons()
    {
        Transform panel = pauseMenuUI.transform.Find("Panel_PauseMenu");

        panel.Find("Button_Resume").GetComponent<Button>().onClick.AddListener(Resume);
        panel.Find("Button_MainMenu").GetComponent<Button>().onClick.AddListener(MainMenu);
        panel.Find("Button_Quit").GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle pause menu and pause state when pressing Escape or P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            Toggle();
    }

    public void Toggle()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    // Switches to Main Menu scene
    public void MainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu_PC");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
