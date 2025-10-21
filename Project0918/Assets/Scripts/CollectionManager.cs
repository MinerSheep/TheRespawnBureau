using UnityEngine;
using TMPro;


public class CollectionManager : MonoBehaviour
{
    public static CollectionManager instance { get; private set; }

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

        // Load high score from file and display in HUD
        highScore = PlayerPrefs.GetInt("score");

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
        Debug.Log("Score saved: " + highScore);
        Debug.Log("Coins Collected: " + coinsCollected);

        PlayerPrefs.SetInt("score", highScore);
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
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.Save();

        if (_highScoreText != null)
            _highScoreText.text = "High Score: " + highScore;
        //Debug.Log("High Score has been reset!");
    }
}
