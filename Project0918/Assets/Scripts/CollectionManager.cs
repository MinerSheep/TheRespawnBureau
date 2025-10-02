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
        highScore = PlayerPrefs.GetInt("score");
        _scoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        if(score > highScore)
        {
            highScore = score;
            _scoreText.text = "High Score: " + highScore;
            Debug.Log("NEW HIGH SCORE: " + highScore);
        }
    }

    public void SaveScore()
    {
        Debug.Log("SAVING SCORE");
        PlayerPrefs.SetInt("score", highScore);
    }
}
