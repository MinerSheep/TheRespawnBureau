using UnityEngine;
using TMPro;
using System.IO;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }

    [Header("Settings")]
    public int score;
    public int highScore;

    [Header("References")]
    [SerializeField] private TextMeshPro _scoreText;
    [SerializeField] private TextMeshPro _highScoreText;

    [HideInInspector] public int coinsCollected;

    private string saveFolder;
    private string saveFilePath;

    void Start()
    {
        instance = this;

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

        // Load high score from file and display in HUD
        highScore = LoadScore();

        if (_scoreText == null)
            _scoreText = transform.Find("Score")?.GetComponent<TextMeshPro>();
        if (_highScoreText == null)
            _highScoreText = transform.Find("HighScore")?.GetComponent<TextMeshPro>();

        if (_highScoreText != null)
            _highScoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        //Debug.Log("Current Score: " + score);
        if (_scoreText != null)
            _scoreText.text = "Score: " + score;

        // HUD updates if the player reaches a new high score
        if (score > highScore)
        {
            highScore = score;
            if (_highScoreText != null)
                _highScoreText.text = "High Score: " + highScore;
        }

        // Press 'B' to reset high score (for test)
        if (Input.GetKeyDown(KeyCode.B))
        {
            ResetHighScore();
        }

    }

    // Saves high score when player dies
    public void SaveScore()
    {
        Debug.Log("HighScore saved: " + highScore);
        Debug.Log("Coins Collected: " + coinsCollected);
        Debug.Log("Writing high score to: " + saveFilePath);

        // Write the high score to the save file
        File.WriteAllText(saveFilePath, "HighScore: " + highScore);

        //PlayerPrefs.SetInt("score", highScore);
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + coinsCollected);
        PlayerPrefs.Save();

        Debug.Log("total coins: " + PlayerPrefs.GetInt("coins"));

        coinsCollected = 0;
    }

    // Loads high score from file
    public int LoadScore()
    {
        if(File.Exists(saveFilePath))
        {
            string loadedScore = File.ReadAllText(saveFilePath);
            Debug.Log("Loaded from file: " + loadedScore);

            // Split the "HighScore : " from the actual score
            string[] parts = loadedScore.Split(':');
            if(parts.Length == 2)
            {
                string score = parts[1].Trim(); // Get the actual number
                // If the result was actually a number, return it!
                if(int.TryParse(score, out int highScore))
                {
                    return highScore;
                }
            }
        }
        else
        {
            Debug.Log("Save file not found!");
        }
        // If we failed to load the score correctly, just return 0
        return 0;
    }

    // Resets high score (for test)
    public void ResetHighScore()
    {
        highScore = 0;
        score = 0;

        // Write the high score to the save file
        File.WriteAllText(saveFilePath, "HighScore: " + highScore);

        //PlayerPrefs.SetInt("score", 0);
        //PlayerPrefs.Save();

        if (_highScoreText != null)
            _highScoreText.text = "High Score: " + highScore;
        //Debug.Log("High Score has been reset!");
    }
}
