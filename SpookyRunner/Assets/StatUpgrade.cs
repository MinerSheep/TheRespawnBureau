using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class StatUpgrade : MonoBehaviour
{
    public enum StatType
    {
        FireRate,
        BulletAmount,
        BulletSpeed,
        Agression,
        Variablity,
        Focus,
        Caution
    }

    public int Points = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI fireRateText;
    [SerializeField] private TextMeshProUGUI bulletAmountText;
    [SerializeField] private TextMeshProUGUI bulletSpeedText;
    [SerializeField] private TextMeshProUGUI agressionText;
    [SerializeField] private TextMeshProUGUI variablityText;
    [SerializeField] private TextMeshProUGUI focusText;
    [SerializeField] private TextMeshProUGUI cautionText;
    [SerializeField] private TextMeshProUGUI pointsText;

    [Header("References")]
    [SerializeField] private MinionStats minionStats;
    [SerializeField] private ProjectileEmitter emitter;

    private Dictionary<StatType, int> levels = new Dictionary<StatType, int>();

    void Awake()
    {
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            levels[type] = 0;
        }
        UpdateAllUI();
        UpdatePointsUI();
    }

    public void TryUpgrade(StatType type)
    {
        int cost = GetCost(type);

        if (Points < cost)
            return;

        Points -= cost;

        levels[type]++;
        ApplyUpgrade(type);
        UpdateUI(type);
        UpdatePointsUI();
    }

    int GetCost(StatType type)
    {
        int level = levels[type];

        switch (type)
        {
            case StatType.FireRate:
                return 1 + Mathf.FloorToInt(Mathf.Pow(level, 1.2f));

            case StatType.BulletAmount:
                return 2 + level * 2;

            case StatType.BulletSpeed:
                return 1 + Mathf.FloorToInt(level * level * 0.5f);

            case StatType.Agression:
                return 1 + level;

            case StatType.Variablity:
                return 1 + Mathf.FloorToInt(Mathf.Pow(level, 1.1f));

            case StatType.Focus:
                return 2 + Mathf.FloorToInt(Mathf.Pow(level, 1.3f));

            case StatType.Caution:
                return 1 + level * 3;

            default:
                return 1;
        }
    }

    void ApplyUpgrade(StatType type)
    {
        switch (type)
        {
            case StatType.FireRate:
                emitter.FireRate += 0.05f;
                break;

            case StatType.BulletAmount:
                emitter.Quantity += 1;
                emitter.Spread = Mathf.Max(5f, emitter.Spread - 2f);
                break;

            case StatType.BulletSpeed:
                emitter.Speed += 0.2f;
                break;

            case StatType.Agression:
                minionStats.Agression += 0.2f;
                break;

            case StatType.Variablity:
                minionStats.Variablity += 0.1f;
                break;

            case StatType.Focus:
                minionStats.Focus += 0.2f;
                break;

            case StatType.Caution:
                minionStats.Caution += 0.2f;
                break;
        }
    }

    void UpdateUI(StatType type)
    {
        int level = levels[type];
        TextMeshProUGUI text = GetText(type);

        if (text != null)
        {
            text.text = $"{type} - Cost: {GetCost(type)}";
        }
    }

    void UpdateAllUI()
    {
        foreach (var type in levels.Keys)
        {
            UpdateUI(type);
        }
    }

    public void UpdatePointsUI()
    {
        if (pointsText != null)
        {
            pointsText.text = $"Points: {Points}";
        }
    }

    TextMeshProUGUI GetText(StatType type)
    {
        return type switch
        {
            StatType.FireRate => fireRateText,
            StatType.BulletAmount => bulletAmountText,
            StatType.BulletSpeed => bulletSpeedText,
            StatType.Agression => agressionText,
            StatType.Variablity => variablityText,
            StatType.Focus => focusText,
            StatType.Caution => cautionText,
            _ => null
        };
    }

    public void TryUpgradeFromUI(int typeIndex)
    {
        StatType type = (StatType)typeIndex;
        TryUpgrade(type);
    }
}