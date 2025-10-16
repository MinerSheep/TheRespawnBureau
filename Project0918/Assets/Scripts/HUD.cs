using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class is responsible for hp and flashlight management
public class HUD : MonoBehaviour
{
    [Header("Battery")]
    public Image battery;
    public FlashLight FL;

    [Header("Health")]
    public TextMeshProUGUI healthText;
    public int maxHp;
    public int hp;

    public void InitializeHUD(FlashLight flashlight)
    {
        FL = flashlight;
    }

    private void Update()
    {
        UpdateFillAmount();
        UpdateHealthAmount();
    }

    public void UpdateFillAmount()
    {
        battery.fillAmount = FL != null ? Mathf.Clamp01(FL.batteryCurrent / FL.batteryMax) : 0;
    }

    public void UpdateHealthAmount()
    {
        hp = Mathf.Clamp(hp, 0, maxHp);
        healthText.text = hp.ToString();
    }
}
