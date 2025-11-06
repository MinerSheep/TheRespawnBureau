using System.Collections;
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

    public void Start()
    {
        if (OptionsPanel != null && AudioManager.instance != null)
        {
            OptionsPanel.transform.Find("Music").GetComponent<Slider>().onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
            OptionsPanel.transform.Find("Sound").GetComponent<Slider>().onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        }
        PlayPanel.transform.position = OffScreenPosition;
        OnlinePanel.transform.position = OffScreenPosition;
    }
    
    [Header("Move in from the side")]
    public Vector3 OffScreenPosition;
    public Vector3 LandingPosition;

    public void TogglePlayMenu()
    {
        if (PlayPanel != null)
        {
            if (ActivePanel != PlayPanel)
            {
                StartCoroutine(SlideInPanel(ActivePanel));

                ActivePanel = PlayPanel;
                ShowPlayPanel();
            }
            else
            {
                StartCoroutine(SlideOutPanel(ActivePanel));
                HidePlayPanel();
            }
        }
    }
    public void ToggleOptionsMenu()
    {
        if (OptionsPanel != null)
        {
            if (ActivePanel != OptionsPanel)
            {
                StartCoroutine(SlideInPanel(ActivePanel));

                ActivePanel?.SetActive(false);
                ActivePanel = OptionsPanel;
                ShowOptionsPanel();
            }
            else
            {
                StartCoroutine(SlideOutPanel(ActivePanel));
                ActivePanel?.SetActive(true);
                HidePlayPanel();
            }
        }
    }
    public void ToggleOnlineMenu()
    {
        if (OnlinePanel != null)
        {
            if (ActivePanel != OnlinePanel)
            {
                StartCoroutine(SlideInPanel(ActivePanel));

                ActivePanel?.SetActive(false);
                ActivePanel = OnlinePanel;
                ShowOnlinePanel();
            }
            else
            {
                StartCoroutine(SlideOutPanel(ActivePanel));
                ActivePanel?.SetActive(true);
                HideOnlinePanel();
            }
        }
    }
    public void ShowPlayPanel()
    {
        PlayPanel.SetActive(true);
        ActivePanel = PlayPanel;
        StartCoroutine(SlideInPanel(PlayPanel));
    }
    public void HidePlayPanel()
    {
        StartCoroutine(SlideOutPanel(PlayPanel));
        ActivePanel = null;
    }
    public void ShowOptionsPanel()
    {
        OptionsPanel.SetActive(true);
        ActivePanel = OptionsPanel;
    }
    public void HideOptionsPanel()
    {
        ActivePanel = null;
    }
    public void ShowOnlinePanel()
    {
        OnlinePanel.SetActive(true);
        ActivePanel = OnlinePanel;
        StartCoroutine(SlideInPanel(OnlinePanel));
    }
    public void HideOnlinePanel()
    {
        StartCoroutine(SlideOutPanel(OnlinePanel));
        ActivePanel = null;
    }
    private IEnumerator SlideInPanel(GameObject panel)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            panel.transform.position = Vector3.Lerp(OffScreenPosition, LandingPosition, t);
            yield return null;
        }
    }
    private IEnumerator SlideOutPanel(GameObject panel)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            panel.transform.position = Vector3.Lerp(LandingPosition, OffScreenPosition, t);
            yield return null;
        }
        panel.SetActive(false);
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
