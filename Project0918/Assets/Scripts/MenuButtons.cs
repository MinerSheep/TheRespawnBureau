using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [Header("References")]
    public GameObject PlayPanel;
    public GameObject OnlinePanel;

    private bool PlayPanelActive = false;
    private bool OnlinePanelActive = false;


    public void TogglePlayMenu()
    {
        if (PlayPanel != null)
        {
            if (!PlayPanelActive)
            {
                ShowPlayPanel();
                HideOnlinePanel();
            }
            else
            {
                HidePlayPanel();
            }
        }
    }

    public void ToggleOnlineMenu()
    {
        if (OnlinePanel != null)
        {
            if (!OnlinePanelActive)
            {
                ShowOnlinePanel();
                HidePlayPanel();
            }
            else
            {
                HideOnlinePanel();
            }
        }
    }

    public void ShowPlayPanel()
    {
        PlayPanel.SetActive(true);
        PlayPanelActive = true;
    }

    public void HidePlayPanel()
    {
        PlayPanel.SetActive(false);
        PlayPanelActive = false;
    }

    public void ShowOnlinePanel()
    {
        OnlinePanel.SetActive(true);
        OnlinePanelActive = true;
    }

    public void HideOnlinePanel()
    {
        OnlinePanel.SetActive(false);
        OnlinePanelActive = false;
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
