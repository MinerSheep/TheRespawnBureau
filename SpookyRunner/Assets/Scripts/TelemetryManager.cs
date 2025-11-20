using System;
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


    [Header("Settings")]
    [SerializeField] public bool timeBasedRecording = true;
    [SerializeField] public List<string> gameDataRecordFormat;

    private float overalltimer = 0;

    private float timer = 0;
    private float recordat = float.MaxValue;

    private bool firstDeath = false;

    private Dictionary<string, uint> integers =
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
        
    }

    public void ActionPerformed()
    {
        
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
    public void RoundEnd()
    {
        // Dump round data
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

        Directory.CreateDirectory("Telemetry");

        gamedatastream = new StreamWriter("Telemetry/gamedata-" + (string)DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
        inputstream = new StreamWriter("Telemetry/inputdata-" + (string)DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
    }

    // Update is called once per frame
    void Update()
    {
        overalltimer += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= recordat)
        {
            // In here would include data that you want to record by second
            // Base it off of gameDataRecordFormat
            gamedatastream.WriteLine(timer + "," + );

            recordat += 1f;
        }
    }
    
    void OnApplicationQuit()
    {
        gamedatastream.WriteLine("Application Quit,,Total Gameplay Time: " + overalltimer + " seconds");

        gamedatastream.Close();
    }
}
