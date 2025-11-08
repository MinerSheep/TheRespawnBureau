using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        SaveSystem.SaveGlobalData();
    }

    public void LoadGame()
    {
        SaveSystem.LoadGlobalData();
    }
}

