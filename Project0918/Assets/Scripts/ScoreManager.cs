using UnityEngine;
using TMPro;


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

    void Start()
    {
        instance = this;

        // Load high score from file
        string loadData = SaveManager.Load();
        if (loadData != null)
        {
            // Split the "HighScore : " from the actual score
            string[] parts = loadData.Split(':');
            if (parts.Length == 2)
            {
                string score = parts[1].Trim(); // Get the actual number
                                                // If the result was actually a number, return it!\
                if (!int.TryParse(score, out highScore))
                {
                    Debug.Log("Failed to parse loaded data");
                }
            }
        }
        else
        {
            Debug.Log("Failed to load data");
            highScore = 0;
        }

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

        // Write the high score to the save file
        SaveManager.SaveScore(highScore);
        // Write the current score to PlayerPrefs
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + coinsCollected);
        PlayerPrefs.Save();

        Debug.Log("total coins: " + PlayerPrefs.GetInt("coins"));

        coinsCollected = 0;
    }

    // Resets high score (for test)
    public void ResetHighScore()
    {
        highScore = 0;
        score = 0;

        // Write the high score to the save file
        SaveManager.SaveScore(highScore);

        if (_highScoreText != null)
            _highScoreText.text = "High Score: " + highScore;
        //Debug.Log("High Score has been reset!");
    }
}
