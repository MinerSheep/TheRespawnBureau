using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private int levelNum = 0;
    // Start is called before the first frame update
    void Start()
    {

       button.GetComponent<Button>();

        Debug.Log(GlobalDataStatic.unlockedLevel);
        if (GlobalDataStatic.unlockedLevel < levelNum)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void ChangeScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        GlobalDataStatic.unlockedLevel = 1;
        Debug.Log(GlobalDataStatic.unlockedLevel);
    }
    public void SettingPopUp(GameObject settingPopUp)
    {
        settingPopUp.SetActive(true);
    }
}
