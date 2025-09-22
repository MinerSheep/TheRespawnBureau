/**
* @file MetricsManager.cs
* @author Ryder Claus
* @brief Temporary system for getting statistics from players in game
*/

using System.Collections.Generic;
using UnityEngine;

public class MetricsManager : MonoBehaviour
{
    public static MetricsManager Instance { get; private set; }

    Dictionary<string, uint> inputTriggered = new Dictionary<string, uint>();

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("MetricsManager in play");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RecordInput(string inputname)
    {
        inputTriggered[inputname]++;
    }

    // Example: write to disk when the game closes
    void OnApplicationQuit()
    {
        System.IO.File.WriteAllText(".txt", $"Total Jumps: {0}");
    }
}
