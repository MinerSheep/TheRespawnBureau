using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject PauseUI;

    // Start is called before the first frame update
    void Start()
    {
        winUI.SetActive(false);
        loseUI.SetActive(false);
        PauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            PauseUI.SetActive(true);
        }
    }

    public void ChangeScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void SettingPopUp (GameObject settingPopUp)
    {
        settingPopUp.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
    }

    public void Win()
    {
        winUI.SetActive(true);
    }

    public void Lose()
    {
        loseUI.SetActive(true);
    }
}
