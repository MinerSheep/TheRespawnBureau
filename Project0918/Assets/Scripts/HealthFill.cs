using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class is responsible for hp and flashlight management
public class HealthFill : MonoBehaviour
{
    [Header("Battery")]
    public Image battery;
    public float imageFill = 1.0f;
    public float batteryLossSpeed = 30f;
    public float BatteryCurrentAmount = 100f;
    public float BatteryMaxAmount = 100f;
    public FlashLight FL;

    [Header("Health")]
    public int hp = 4;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        UpdateFillAmount();
        UpdateHealthAmount();

        FL.batteryUseRate = batteryLossSpeed;
    }

    private void Update()
    {
        BatteryCurrentAmount -= batteryLossSpeed * Time.deltaTime;
        UpdateFillAmount();

        UpdateHealthAmount();
    }

    public void LoseHealth()
    {
        hp--;
        UpdateHealthAmount();
    }



    public void UpdateFillAmount()
    {
        battery.fillAmount = Mathf.Clamp01(BatteryCurrentAmount / BatteryMaxAmount);

    }

    public void UpdateHealthAmount()
    {
        healthText.text = hp.ToString();
    }


}
