using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Things to capture for Data:
// Dash press (Measure how often they use it)       inputstream <- InputPressed
// Session Length                                   OnApplicationQuit
// Time they first die/How many times they die.     RoundEnd + OnApplicationQuit
// What do they die to? (Monster? Falling?)         RoundEnd
// Location of death(?)                             RoundEnd
// Level progress                                   RoundEnd
// Highest score                                    OnApplicationQuit
// Character used                                   N/A
// Day return behaviour (Day 1, 7, 30)              External System Needed
// Time between sessions                            External System Needed
// Savings of coins overtime                        

public class TelemetryManager : MonoBehaviour
{
    static public TelemetryManager instance { get; private set; }
    private static StreamWriter gamedatastream;
    private static StreamWriter inputstream;

    public string DeathReason = "";


    [Header("Settings")]
    [SerializeField] public bool timeBasedRecording = true;
    [SerializeField] public List<string> gameDataRecordFormat;

    private float overalltimer = 0;

    private float timer = 0;
    private float recordat = float.MaxValue;

    private bool firstDeath = true;

    private Dictionary<string, uint> integers = new Dictionary<string, uint>
    {
        { "Jumps", 0 },
        { "Crouches", 0 },
        { "CoinCollects", 0 },
        { "CoinMisses", 0 },
        { "StamCollects", 0 },
        { "StamMisses", 0 },
    };

    public void InputPressed(string inputName)
    {
        inputstream.WriteLine(overalltimer + "," + inputName + " pressed");
    }

    public void InputReleased(string inputName)
    {
        inputstream.WriteLine(overalltimer + "," + inputName + " released");
    }

    public void ActionPerformed(string actionName)
    {
        inputstream.WriteLine("Player " + actionName + "ed");
    }

    public void RoundBegin()
    {
        // optional time based recording system
        timer = 0;
        recordat = 1;

        // Put game data header
        gamedatastream.WriteLine(string.Join(",", gameDataRecordFormat));
    }

    // Needs location and reason for death
    public void RoundEnd(bool death)
    {
        GameObject location = FindAnyObjectByType<LevelGenerator>()?.FindPlayerChunk();
        DistanceScoreTracker dst = FindAnyObjectByType<DistanceScoreTracker>();

        float distance = dst ? dst.TotalDistance() : -1;

        // Dump round data
        if (death)
        {
            gamedatastream.WriteLine("Player died," + (firstDeath ? "FIRST DEATH" : "") + ",Reason: " + DeathReason + ",,Location: " + location?.name + ",,Distance: " + distance);
            firstDeath = false;
        }
        else
        {
            gamedatastream.WriteLine("Game ended,,Reason: " + DeathReason + ",,Location: " + location?.name + ",,Distance: " + distance);
        }
        
        timer = 0;
        recordat = float.MaxValue;
    }

    void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        string now = (string)DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        Directory.CreateDirectory(Path.Combine("Telemetry", now));

        gamedatastream = new StreamWriter("Telemetry/" + now + "/gamedata-" + now + ".csv");
        inputstream = new StreamWriter("Telemetry/" + now + "/inputdata-" + now + ".csv");
    }

    void Start()
    {
        inputstream.WriteLine("time,input");
    }

    // Update is called once per frame
    void Update()
    {
        overalltimer += Time.deltaTime;
        timer += Time.deltaTime;

        if (timeBasedRecording && timer >= recordat)
        {
            float frameRate = 1.0f / Time.deltaTime;

            // In here would include data that you want to record by second
            // Base it off of gameDataRecordFormat
            gamedatastream.WriteLine(timer + ",");

            recordat += 1f;
        }
    }
    
    void OnApplicationQuit()
    {
        gamedatastream.WriteLine("Application Quit,,Total Gameplay Time: " + overalltimer + " seconds");
        inputstream.WriteLine("Application Quit,,Total Gameplay Time: " + overalltimer + " seconds");

        gamedatastream.Close();
        inputstream.Close();
    }
}
