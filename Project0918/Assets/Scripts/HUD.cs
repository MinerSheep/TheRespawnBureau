using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class is responsible for hp and flashlight management
public class HUD : MonoBehaviour
{
    [Header("Progress")]
    public Image ProgressBar;
    public GameObject Player;
    public GameObject Goal;
    public float StartX;
    public float GoalX;
    

    [Header("Health")]
    public TextMeshProUGUI healthText;
    public int maxHp;
    public int hp;

    [Header("Coins")]
    public TextMeshProUGUI CoinsAmount;

    private void Start()
    {
        StartX = Player != null ?Player.transform.position.x : 0.0f;
        GoalX = Goal != null ? Goal.transform.position.x : 1.0f;
    }

    private void Update()
    {
        UpdateProgress();
        UpdateHealthAmount();
    }

    public void UpdateProgress()
    {
        if (Player != null && Goal != null)
            ProgressBar.fillAmount = (Player.transform.position.x - StartX) / (Goal.transform.position.x - StartX);
    }

    public void UpdateHealthAmount()
    {
        hp = Mathf.Clamp(hp, 0, maxHp);
        healthText.text = hp.ToString();
    }
}
