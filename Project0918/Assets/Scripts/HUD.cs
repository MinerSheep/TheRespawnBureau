using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class HUDEvents
{
    public delegate void HUDDefaultEvent();
    public static HUDDefaultEvent OnCollectCoin;
}

// This class is responsible for hp and flashlight management
public class HUD : MonoBehaviour
{
    [Header("Progress")]
    public GameObject Player;
    public GameObject Goal;
    public float StartX;
    public float GoalX;

    [Header("Stamina")]
    public float StaminaAmount = 100f;
    public float StaminaMaximum = 100f;
    public float StaminaLossRate = 5f;
    public Image StaminaBarImage;

    [Header("Health")]
    public TextMeshProUGUI healthText;
    public int maxHp;
    public int hp;

    [Header("Stamina")]
    public Slider staminaSlider;
    public float maxStamina;
    public float stamina;

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
        //UpdateProgress();
        UpdateHealthAmount();
        UpdateCoinsAmount();
        UpdatePlayerStamina();
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

    //public void UpdateProgress()
    //{
    //    if (Player != null && Goal != null)
    //        ProgressBar.fillAmount = (Player.transform.position.x - StartX) / (Goal.transform.position.x - StartX);
    //}

    public void UpdateStamina()
    {
        StaminaAmount -= Time.deltaTime * StaminaLossRate;
        StaminaAmount = Mathf.Clamp(StaminaAmount, 0f, StaminaMaximum);
        StaminaBarImage.fillAmount = StaminaAmount / StaminaMaximum;
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

    public void UpdatePlayerStamina()
    {
        if(staminaSlider)
        {
            staminaSlider.value = stamina;
        }
    }

    void OnDestroy()
    {
        HUDEvents.OnCollectCoin -= AddCoin;
    }
}
