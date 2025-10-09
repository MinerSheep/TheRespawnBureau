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
        //_scoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        // HUD updates if the player reaches a new high score
        if(score > highScore)
        {
            highScore = score;
            //_scoreText.text = "High Score: " + highScore;
        }
    }

    // Saves high score when player dies
    public void SaveScore()
    {
        PlayerPrefs.SetInt("score", highScore);
    }
}
