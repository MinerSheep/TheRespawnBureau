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
    [Header("Mobile")]
    public MobileButtonManager mobileButtonManager;

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
    public Health PlayerHP;

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
            AddRemoveHudElements("MobileLayout", "Desktop");
        }
        UpdateHealthAmount();
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
        UpdateCoinsAmount();
        UpdateStamina();
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

    public void ChangeStamina(float adjust)
    {
        StaminaAmount = Mathf.Clamp(StaminaAmount + adjust, 0f, StaminaMaximum);
    }

    public void UpdateStamina()
    {
        StaminaAmount -= Time.deltaTime * StaminaLossRate;
        StaminaAmount = Mathf.Clamp(StaminaAmount, 0f, StaminaMaximum);
        StaminaBarImage.fillAmount = StaminaAmount / StaminaMaximum;

        if (StaminaAmount <= 0.0f)
        {
            // TODO: Remove the LoadScene below once we have PlayerDeath implemented
            PlayerEvents.OnPlayerDeath?.Invoke();
        }
    
    }
        

    public void UpdateHealthAmount()
    {
        healthText.text = PlayerHP.CurrentHP.ToString();
    }

    public void UpdateCoinsAmount()
    {
        coinsText.text = coins.ToString();
    }

    // public void UpdateStamina()
    // {
    //     if(staminaSlider)
    //     {
    //         staminaSlider.value = stamina;
    //     }
    // }

    public void AssignLeftButton(InputBuffer buffer, string action, bool hold)
    {
        if (mobileButtonManager == null)
        {
            Debug.LogWarning("No mobile button manager");
            return;
        }

        mobileButtonManager.LButton.GetComponent<MobileButton>().holdable = hold;
        mobileButtonManager.LButton.GetComponent<MobileButton>().onClick = null;
        mobileButtonManager.LButton.GetComponent<MobileButton>().onRelease = null;

        if (hold)
        {
            mobileButtonManager.LButton.GetComponent<MobileButton>().onClick += () => buffer.StartHold(action);
            mobileButtonManager.LButton.GetComponent<MobileButton>().onRelease += () => buffer.EndHold(action);
        }

        mobileButtonManager.LButton.GetComponent<MobileButton>().onClick += () => buffer.AddToBuffer(action);
    }
    public void AssignRightButton(InputBuffer buffer, string action, bool hold)
    {
        if (mobileButtonManager == null)
        {
            Debug.LogWarning("No mobile button manager");
            return;
        }

        mobileButtonManager.RButton.GetComponent<MobileButton>().holdable = hold;
        mobileButtonManager.RButton.GetComponent<MobileButton>().onClick = null;
        mobileButtonManager.RButton.GetComponent<MobileButton>().onRelease = null;

        if (hold)
        {
            mobileButtonManager.RButton.GetComponent<MobileButton>().onClick += () => buffer.StartHold(action);
            mobileButtonManager.RButton.GetComponent<MobileButton>().onRelease += () => buffer.EndHold(action);
        }

        mobileButtonManager.RButton.GetComponent<MobileButton>().onClick += () => buffer.AddToBuffer(action);
    }

    void OnDestroy()
    {
        HUDEvents.OnCollectCoin -= AddCoin;
    }
}
