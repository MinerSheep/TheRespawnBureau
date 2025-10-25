using System.Numerics;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string saveFolder;  // Directory for save file
    private string saveFilePath;    // File path

    void Start()
    {
        // Declare the folder name in AppData that will store saved data
        saveFolder = Path.Combine(Application.persistentDataPath, "SpookyRunner");

        // Check if the folder already exists
        if (!Directory.Exists(saveFolder))
        {
            Debug.Log("Folder " + saveFolder + " does not exist, creating folder");
            Directory.CreateDirectory(saveFolder);
        }

        // Declare the save filepath
        saveFilePath = Path.Combine(saveFolder, "highscore.dat");
    }

    // Save high score to file
    public void SaveScore(int score)
    {
        // Write the high score to the save file
        File.WriteAllText(saveFilePath, "HighScore: " + score);
    }

    // Loads all data from file
    public string Load()
    {
        if (File.Exists(saveFilePath))
        {
            string data = File.ReadAllText(saveFilePath);
            Debug.Log("Loaded from " + saveFilePath);

            return data;
        }

        // If we failed to load, return null
        Debug.Log("Save file not found!");
        return null;
    }
}
