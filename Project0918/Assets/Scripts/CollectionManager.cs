using UnityEngine;
using TMPro;


public class CollectionManager : MonoBehaviour
{
    public static CollectionManager instance { get; private set; }

    [SerializeField]
    private TextMeshPro _scoreText;

    public int score;
    public int highScore;

    void Start()
    {
        instance = this;

        // Load high score from file and display in HUD
        highScore = PlayerPrefs.GetInt("score");
        _scoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        Debug.Log("Current Score: " + score);

        // HUD updates if the player reaches a new high score
        if (score > highScore)
        {
            highScore = score;
            _scoreText.text = "High Score: " + highScore;
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

        PlayerPrefs.SetInt("score", highScore);
        PlayerPrefs.Save();
    }

    // Resets high score (for test)
    public void ResetHighScore()
    {
        highScore = 0;
        score = 0;
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.Save();
        _scoreText.text = "High Score: " + highScore;
        //Debug.Log("High Score has been reset!");
    }
}
