using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [Header("References")]
    public GameObject PlayPanel;
    public GameObject OptionsPanel;
    public GameObject OnlinePanel;

    private GameObject ActivePanel = null;

    void Start()
    {
        if (OptionsPanel != null && AudioManager.instance != null)
        {
            OptionsPanel.transform.Find("Music").GetComponent<Slider>().onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
            OptionsPanel.transform.Find("Sound").GetComponent<Slider>().onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        }
    }
    
    public void TogglePlayMenu()
    {
        if (PlayPanel != null)
        {
            ActivePanel?.SetActive(false);
            ShowPlayPanel();
        }
    }

    public void ToggleOptionsMenu()
    {
        if (OptionsPanel != null)
        {
            ActivePanel?.SetActive(false);
            ShowOptionsPanel();
        }
    }

    public void ToggleOnlineMenu()
    {
        if (OnlinePanel != null)
        {
            ActivePanel?.SetActive(false);
            ShowOnlinePanel();
        }
    }

    public void ShowPlayPanel()
    {
        PlayPanel.SetActive(true);
        ActivePanel = PlayPanel;
    }

    public void ShowOptionsPanel()
    {
        OptionsPanel.SetActive(true);
        ActivePanel = OptionsPanel;
    }

    public void ShowOnlinePanel()
    {
        OnlinePanel.SetActive(true);
        ActivePanel = OnlinePanel;
    }

    public void AutorunnerPlay(string AutoRunnerTester)
    {
        AudioManager.instance.PlaySound("transition");
        SceneManager.LoadScene(AutoRunnerTester);
    }

    public void PlatformerPlay(string Platformer)
    {
        AudioManager.instance.PlaySound("transition");
        SceneManager.LoadScene(Platformer);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
