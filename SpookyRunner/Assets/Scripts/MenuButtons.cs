using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AddressableAssets;

public class MenuButtons : MonoBehaviour
{
    [Header("References")]
    public GameObject PlayPanel;
    public GameObject OptionsPanel;
    public GameObject OnlinePanel;
    public GameObject OptionsPanelSliderContent;
    public GameObject CreditsPanel;
    public GameObject QuitPanel;


    private GameObject ActivePanel = null;

    void OnEnable()
    {
        if (OptionsPanelSliderContent != null && AudioManager.instance != null)
        {
            OptionsPanelSliderContent.transform.Find("Master").GetComponent<Slider>().onValueChanged.AddListener(AudioManager.instance.SetMasterVolume);
            OptionsPanelSliderContent.transform.Find("Music").GetComponent<Slider>().onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
            OptionsPanelSliderContent.transform.Find("Sound").GetComponent<Slider>().onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);

            OptionsPanelSliderContent.transform.Find("Master").GetComponent<Slider>().value = AudioManager.instance.MasterVolume;
            OptionsPanelSliderContent.transform.Find("Music").GetComponent<Slider>().value = AudioManager.instance.MusicVolume;
            OptionsPanelSliderContent.transform.Find("Sound").GetComponent<Slider>().value = AudioManager.instance.SoundVolume;
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

    public void ShowCredits()
    {
        CreditsPanel.SetActive(true);
        ActivePanel = CreditsPanel;
    }

    public void ShowQuits()
    {
        QuitPanel.SetActive(true);
        ActivePanel = QuitPanel;
    }

    public void HideQuits()
    {
        QuitPanel.SetActive(false);
        ActivePanel?.SetActive(false);
        ActivePanel = null;
    }

    public void HideCredits()
    {
        CreditsPanel.SetActive(false);
        ActivePanel?.SetActive(false);
        ActivePanel = null;
    }

    public void AutorunnerPlay()
    {
        string AutoRunnerInfinite = "AutoRunnerInfinite";
        AudioManager.instance.PlaySound("transition");
        StartCoroutine(LoadSceneAsync(AutoRunnerInfinite));
        //SceneManager.LoadScene(AutoRunnerTester);
    }

    public void PlatformerPlay(string Platformer)
    {
        AudioManager.instance.PlaySound("transition");
        SceneManager.LoadScene(Platformer);
    }
    private IEnumerator LoadSceneAsync(string AutoRunnerTester)
    {
        var handle = Addressables.DownloadDependenciesAsync("default");
        while (!handle.IsDone)
        {
            float percentCompleted = handle.PercentComplete;
            Debug.Log(percentCompleted);

            yield return null;
        }
        // Load scene from server
        Addressables.LoadSceneAsync(AutoRunnerTester);
    }

    public void QuitGame()
    {
        Debug.Log("quitting");
        Application.Quit();

    }
}
