using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class HUDEvents
{
    // Currently unused
    public delegate void HUDDefaultEvent();
    public static HUDDefaultEvent OnCollectCoin;
}

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
    public TextMeshProUGUI coinsText;
    public int coins;



    private void Start()
    {
        StartX = Player != null ? Player.transform.position.x : 0.0f;
        GoalX = Goal != null ? Goal.transform.position.x : 1.0f;

        HUDEvents.OnCollectCoin += AddCoin;

        if (DeviceDetector.IsDesktop)
        {
            //desktop hud
            AddRemoveHudElements("Desktop", "Mobile");

        }
        else if (DeviceDetector.IsMobile)
        {
            //mobile hud
            AddRemoveHudElements("Mobile", "Desktop");
        }
    }
    
    public void AddRemoveHudElements(string add, string remove)
    {
        foreach (Transform child in transform)
        {
            if (child.tag == add)
                child.gameObject.SetActive(true);

            if (child.tag == remove)
                child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateProgress();
        UpdateHealthAmount();
        UpdateCoinsAmount();
    }

    void AddCoin()
    {
        coins++;
    }

    public void SaveCoins()
    {
        Debug.Log("Coins Collected: " + coins);
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + coins);
        Debug.Log("Total Coins: " + PlayerPrefs.GetInt("coins"));

        PlayerPrefs.Save();
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

    public void UpdateCoinsAmount()
    {
        coinsText.text = coins.ToString();
    }

    void OnDestroy()
    {
        HUDEvents.OnCollectCoin -= AddCoin;
    }
}
