using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject OnlinePanel;
    private bool OnlinePanelActive = false;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowOnline()
    {
        if (OnlinePanel != null)
        {
            if(!OnlinePanelActive)
            {
                OnlinePanel.SetActive(true);
                OnlinePanelActive = true;
            }
            else
            {
                OnlinePanel.SetActive(false);
                OnlinePanelActive = false;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
