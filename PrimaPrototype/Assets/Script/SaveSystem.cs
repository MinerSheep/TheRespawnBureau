using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string saveFilePath = Path.Combine(Application.persistentDataPath, "globaldata.json");

    public static void SaveGlobalData()
    {
        GlobalData data = new GlobalData(GlobalDataStatic.unlockedLevel, GlobalDataStatic.volume);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    public static void LoadGlobalData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GlobalData data = JsonUtility.FromJson<GlobalData>(json);
            GlobalDataStatic.unlockedLevel = data.unlockedLevel;
            GlobalDataStatic.volume = data.volume;
        }
        else
        {
            
            GlobalDataStatic.unlockedLevel = 1;
            GlobalDataStatic.volume = 0.5f;
            Debug.Log(GlobalDataStatic.unlockedLevel);
        }
    }
}
