using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
    private static StreamWriter inputstream;    // this one is mostly irrelevant

    [Header("Settings")]
    [SerializeField] public bool timeBasedRecording = true;

    // PRIVATE variables
    private List<string> gameDataRecordFormat = new List<string>{ "Time", "Jumps", "Crouches", "Dashes", "WallHits", 
    "CoinCollects", "CoinMisses", "StamCollects", "StamMisses" };
    private float overalltimer = 0;
    private float timer = 0;
    private float record_time = float.MaxValue;
    private bool first_death = true;

    public string DeathReason = "";

    private Dictionary<string, uint> integers = new Dictionary<string, uint>
    {
        { "Jumps", 0 },
        { "Crouches", 0 },
        { "Dashes", 0 },
        { "WallHits", 0 },
        { "CoinCollects", 0 },
        { "CoinMisses", 0 },
        { "StamCollects", 0 },
        { "StamMisses", 0 },
    };

    public void IntIncrease(string name, uint value = 1)
    {
        integers[name] += value;
    }

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

        if (actionName == "Dash")
        {
            AnalyticsManager.Instance?.RecordDash();
        }
    }

    bool begin = false;
    public void RoundBegin()
    {
        // optional time based recording system
        begin = true;
        timer = 0;
        record_time = 1;

        // Put game data header
        gamedatastream.WriteLine(string.Join(",", gameDataRecordFormat));

        List<string> keys = new List<string>(integers.Keys);
        foreach (var key in keys)
            integers[key] = 0;

        // Server
        AnalyticsManager.Instance?.StartSession();
    }

    // Needs location and reason for death
    public void RoundEnd(bool death)
    {
        if (!begin)
            return;
        begin = false;

        GameObject location = FindAnyObjectByType<LevelGenerator>()?.FindPlayerChunk();
        DistanceScoreTracker dst = FindAnyObjectByType<DistanceScoreTracker>();

        float distance = dst ? dst.TotalDistance() : -1;

        // Dump round data
        if (death)
        {
            gamedatastream?.WriteLine("Player died," + (first_death ? "FIRST DEATH" : "") + ",Reason: " + DeathReason + ",,Location: " + location?.name + ",,Distance: " + distance);
            first_death = false;

            // Server
            Vector2 deathPos = FindAnyObjectByType<PlayerController>()?.transform.position ?? Vector2.zero;
            string deathType = string.IsNullOrEmpty(DeathReason) ? "other" : DeathReason.ToLower();
            AnalyticsManager.Instance?.RecordDeath(deathType, deathPos);
        }
        else
        {
            gamedatastream?.WriteLine("Game ended,,Reason: " + DeathReason + ",,Location: " + location?.name + ",,Distance: " + distance);
        }

        // Server
        int score = ScoreManager.instance?.score ?? 0;
        AnalyticsManager.Instance?.EndSession(score, Mathf.RoundToInt(distance));

        timer = 0;
        record_time = float.MaxValue;
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

        if (timeBasedRecording && timer >= record_time)
        {
            float frameRate = 1.0f / Time.deltaTime;

            // In here would include data that you want to record by second
            // Base it off of gameDataRecordFormat
            string writeLine = Mathf.Round(timer).ToString();
            foreach (var dataName in gameDataRecordFormat)
                writeLine += (integers.ContainsKey(dataName) ? integers[dataName] : "") + ",";

            gamedatastream.WriteLine(writeLine);

            record_time += 1f;
        }
    }
    
    void OnApplicationQuit()
    {
        DeathReason = "App Quit";
        RoundEnd(false);

        gamedatastream.WriteLine("Application Quit,,Total Gameplay Time: " + overalltimer + " seconds");
        inputstream.WriteLine("Application Quit,,Total Gameplay Time: " + overalltimer + " seconds");

        gamedatastream.Close();
        inputstream.Close();
    }
}
