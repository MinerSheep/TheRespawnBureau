/**
* @file MetricsManager.cs
* @author Ryder Claus
* @brief Temporary system for getting statistics from players in game
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class MetricsManager : MonoBehaviour
{
    public static MetricsManager Instance { get; private set; }

    Dictionary<string, uint> inputTriggered = new Dictionary<string, uint>();

    string filename;

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

    void Start()
    {
        filename = "Telemetry/T" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + ".csv";
        System.IO.File.WriteAllText(filename, DateTime.Now.ToString("HH:mm:ss") + "," + "Game Started");
    }

    public void RecordInput(string inputname)
    {
        inputTriggered[inputname]++;
        System.IO.File.AppendAllText(filename, DateTime.Now.ToString("HH:mm:ss") + "," + inputname);
    }

    // Example: write to disk when the game closes
    void OnApplicationQuit()
    {
        System.IO.File.AppendAllText(filename, DateTime.Now.ToString("HH:mm:ss") + "," + "Game Ended");

        // Output all statistics
    }
}
