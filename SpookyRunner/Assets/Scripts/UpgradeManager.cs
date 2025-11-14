using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("CSV File")]
    [SerializeField] private TextAsset upgradeCSV;

    [Header("Debug Option")]
    [SerializeField] private bool printDataOnStart = true;

    // Store upgrade data by type and level
    // Dictionary<UpgradeType, Dictionary<Level, UpgradeData>>
    private Dictionary<UpgradeType, Dictionary<int, UpgradeData>> upgradeDatabase =
        new Dictionary<UpgradeType, Dictionary<int, UpgradeData>>();

    void Awake()
    {
        // Setup singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadUpgradeData();

        if (printDataOnStart)
        {
            PrintAllUpgradeData();
        }
    }

    public void LoadUpgradeData()
    {
        if (upgradeCSV == null)
        {
            Debug.LogError("CSV file is not allocated!");
            return;
        }

        upgradeDatabase.Clear();

        // Split CSV text by line
        string[] lines = upgradeCSV.text.Split('\n');

        // Parse upgrade type names
        string[] statHeaders = lines[0].Split(',');
        UpgradeType[] upgradeTypes = ParseUpgradeTypes(statHeaders);

        // Parse Stat data (lines 2-6)
        Dictionary<UpgradeType, Dictionary<int, int>> statData = ParseSection(lines, 1, 6, upgradeTypes);

        // Parse Cost data (lines 9-13)
        Dictionary<UpgradeType, Dictionary<int, int>> costData = ParseSection(lines, 8, 13, upgradeTypes);

        // Merge data
        foreach (UpgradeType type in upgradeTypes)
        {
            upgradeDatabase[type] = new Dictionary<int, UpgradeData>();

            if (statData.ContainsKey(type) && costData.ContainsKey(type))
            {
                foreach (var level in statData[type].Keys)
                {
                    int stat = statData[type][level];
                    int cost = costData[type].ContainsKey(level) ? costData[type][level] : 0;

                    upgradeDatabase[type][level] = new UpgradeData(level, stat, cost);
                }
            }
        }

        Debug.Log($"Upgrade Data loaded Successfully! Num of Type:  {upgradeDatabase.Count}, " +
                  $"Num of Level: {upgradeDatabase[UpgradeType.MaxStamina].Count}");
    }

    private UpgradeType[] ParseUpgradeTypes(string[] headers)
    {
        List<UpgradeType> types = new List<UpgradeType>();

        for (int i = 1; i < headers.Length; i++)
        {
            string header = headers[i].Trim();

            if (string.IsNullOrEmpty(header)) break;

            if (header.Contains("Max Stamina"))
                types.Add(UpgradeType.MaxStamina);
            else if (header.Contains("Health"))
                types.Add(UpgradeType.Health);
            else if (header.Contains("Stamina Drain"))
                types.Add(UpgradeType.StaminaDrain);
            else if (header.Contains("Score Orb"))
                types.Add(UpgradeType.ScoreOrbValue);
            else if (header.Contains("Coin"))
                types.Add(UpgradeType.CoinValue);
            else if (header.Contains("Magnet"))
                types.Add(UpgradeType.SmallMagnet);
        }

        return types.ToArray();
    }

    private Dictionary<UpgradeType, Dictionary<int, int>> ParseSection(
        string[] lines, int startLine, int endLine, UpgradeType[] types)
    {
        Dictionary<UpgradeType, Dictionary<int, int>> sectionData =
            new Dictionary<UpgradeType, Dictionary<int, int>>();

        // Initialize dictionary for each upgrade type
        foreach (UpgradeType type in types)
        {
            sectionData[type] = new Dictionary<int, int>();
        }

        // Parse data lines
        for (int lineIndex = startLine; lineIndex <= endLine; lineIndex++)
        {
            if (lineIndex >= lines.Length) break;

            string[] values = lines[lineIndex].Split(',');

            if (values.Length < 2) continue;

            // Parse level
            if (!int.TryParse(values[0].Trim(), out int level)) continue;

            // Parse value for each upgrade type
            for (int i = 0; i < types.Length && i + 1 < values.Length; i++)
            {
                if (int.TryParse(values[i + 1].Trim(), out int value))
                {
                    sectionData[types[i]][level] = value;
                }
            }
        }

        return sectionData;
    }

    // Get upgrade data for specific type and level
    public UpgradeData GetUpgradeData(UpgradeType type, int level)
    {
        if (upgradeDatabase.ContainsKey(type) && upgradeDatabase[type].ContainsKey(level))
        {
            return upgradeDatabase[type][level];
        }

        Debug.LogWarning($"Type: {type} Level: {level}, Can't find the data!");
        return null;
    }

    // Get upgrade stat value
    public int GetUpgradeStat(UpgradeType type, int level)
    {
        UpgradeData data = GetUpgradeData(type, level);
        return data != null ? data.stat : 0;
    }

    // Get upgrade cost value
    public int GetUpgradeCost(UpgradeType type, int level)
    {
        UpgradeData data = GetUpgradeData(type, level);
        return data != null ? data.cost : 0;
    }

    // Print all upgrade data (for debugging)
    public void PrintAllUpgradeData()
    {
        Debug.Log("========================================");
        Debug.Log("UPGRADE DATA LOADED SUCCESSFULLY!");
        Debug.Log("========================================");

        foreach (var typeEntry in upgradeDatabase)
        {
            Debug.Log($"\n+--- <color=cyan><b>{typeEntry.Key}</b></color> -------------------");
            Debug.Log("|  Level |  Stat  |  Cost  |");
            Debug.Log("+--------+--------+--------+");

            foreach (var levelEntry in typeEntry.Value)
            {
                UpgradeData data = levelEntry.Value;
                Debug.Log($"|   {data.level,2}   |  {data.stat,4}  |  {data.cost,4}  |");
            }

            Debug.Log("+--------+--------+--------+");
        }

        Debug.Log("\n========================================");
        Debug.Log($"Total Types: {upgradeDatabase.Count}");
        Debug.Log($"Total Levels per Type: {upgradeDatabase[UpgradeType.MaxStamina].Count}");
        Debug.Log("========================================\n");
    }
}

