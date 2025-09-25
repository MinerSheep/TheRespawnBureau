using UnityEngine;

public class MenuFunctions : MonoBehaviour
{
    public GameObject PausePanel;
    public bool Paused=false;

    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Paused=false;
                Time.timeScale=1;
                PausePanel.SetActive(false);
            }
            else
            {
                Paused=true;
                Time.timeScale=0;
                PausePanel.SetActive(true);
            }
        }
    }
    private void Update()
    {
        Pause();
    }
}
