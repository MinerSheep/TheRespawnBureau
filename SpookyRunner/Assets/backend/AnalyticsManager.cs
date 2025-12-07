using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// ==================== Data Structures ====================

[Serializable]
public class SessionStartData
{
    public string device_id;
    public string player_name;
    public string character_used;
}

[Serializable]
public class SessionStartResponse
{
    public bool success;
    public int session_id;
}

[Serializable]
public class SessionEndData
{
    public int session_id;
    public int session_length;
    public int highest_score;
    public int level_progress;
    public int dash_count;
    public int death_count;
    public int coins_earned;
    public int coins_total;
}

[Serializable]
public class DeathData
{
    public int session_id;
    public string device_id;
    public string death_type;
    public float death_location_x;
    public float death_location_y;
    public int time_since_start;
}

// ==================== Analytics Manager ====================

public class AnalyticsManager : MonoBehaviour
{
    private static AnalyticsManager instance;
    public static AnalyticsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<AnalyticsManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AnalyticsManager");
                    instance = go.AddComponent<AnalyticsManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    [Header("Server Configuration")]
    [SerializeField] private ServerConfig serverConfig;

    [Header("Current Session Info")]
    [SerializeField] private int currentSessionId = -1;
    [SerializeField] private float sessionStartTime;
    [SerializeField] private int dashCount = 0;
    [SerializeField] private int deathCount = 0;
    [SerializeField] private int coinsAtStart = 0;

    // Device ID (unique per device, works on mobile too)
    private string deviceId;

    // Player info
    private string currentPlayerName = "Unknown";
    private string currentCharacter = "Default";

    public int CurrentSessionId => currentSessionId;
    public string DeviceId => deviceId;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Get unique device ID (works on mobile, PC, etc.)
        deviceId = SystemInfo.deviceUniqueIdentifier;
        Debug.Log($"[Analytics] Device ID: {deviceId}");
    }

    void Start()
    {
        if (serverConfig == null)
        {
            serverConfig = Resources.Load<ServerConfig>("ServerConfig");
        }
    }

    // ==================== Session Methods ====================

    /// <summary>
    /// Call this when the game/run starts
    /// </summary>
    public void StartSession(string playerName = null, string characterUsed = null)
    {
        currentPlayerName = playerName ?? "Player_" + deviceId.Substring(0, 6);
        currentCharacter = characterUsed ?? "Default";
        sessionStartTime = Time.time;
        dashCount = 0;
        deathCount = 0;
        coinsAtStart = GetCurrentCoins();

        if (serverConfig != null && serverConfig.useOnlineServer)
        {
            StartCoroutine(StartSessionCoroutine());
        }
        else
        {
            // Offline mode - just track locally
            currentSessionId = -1;
            Debug.Log("[Analytics] Session started (offline mode)");
        }
    }

    private IEnumerator StartSessionCoroutine()
    {
        SessionStartData data = new SessionStartData
        {
            device_id = deviceId,
            player_name = currentPlayerName,
            character_used = currentCharacter
        };

        string jsonData = JsonUtility.ToJson(data);
        UnityWebRequest request = UnityWebRequest.Post(
            serverConfig.serverURL + "/api/session/start",
            jsonData,
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                SessionStartResponse response = JsonUtility.FromJson<SessionStartResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    currentSessionId = response.session_id;
                    Debug.Log($"[Analytics] Session started: ID {currentSessionId}");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[Analytics] Failed to parse session start response: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("[Analytics] Failed to start session: " + request.error);
        }
    }

    /// <summary>
    /// Call this when the game/run ends
    /// </summary>
    public void EndSession(int highestScore, int levelProgress)
    {
        if (serverConfig != null && serverConfig.useOnlineServer && currentSessionId > 0)
        {
            StartCoroutine(EndSessionCoroutine(highestScore, levelProgress));
        }
        else
        {
            Debug.Log($"[Analytics] Session ended (offline) - Score: {highestScore}, Deaths: {deathCount}, Dashes: {dashCount}");
        }
    }

    private IEnumerator EndSessionCoroutine(int highestScore, int levelProgress)
    {
        int sessionLength = Mathf.RoundToInt(Time.time - sessionStartTime);
        int coinsEarned = GetCurrentCoins() - coinsAtStart;

        SessionEndData data = new SessionEndData
        {
            session_id = currentSessionId,
            session_length = sessionLength,
            highest_score = highestScore,
            level_progress = levelProgress,
            dash_count = dashCount,
            death_count = deathCount,
            coins_earned = Mathf.Max(0, coinsEarned),
            coins_total = GetCurrentCoins()
        };

        string jsonData = JsonUtility.ToJson(data);
        UnityWebRequest request = UnityWebRequest.Post(
            serverConfig.serverURL + "/api/session/end",
            jsonData,
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"[Analytics] Session {currentSessionId} ended - Length: {sessionLength}s, Score: {highestScore}");
        }
        else
        {
            Debug.LogWarning("[Analytics] Failed to end session: " + request.error);
        }

        currentSessionId = -1;
    }

    // ==================== Death Tracking ====================

    /// <summary>
    /// Call this when the player dies
    /// </summary>
    /// <param name="deathType">monster, falling, obstacle, other</param>
    /// <param name="position">Death position in world</param>
    public void RecordDeath(string deathType, Vector2 position)
    {
        deathCount++;
        int timeSinceStart = Mathf.RoundToInt(Time.time - sessionStartTime);

        if (serverConfig != null && serverConfig.useOnlineServer && currentSessionId > 0)
        {
            StartCoroutine(RecordDeathCoroutine(deathType, position, timeSinceStart));
        }
        else
        {
            Debug.Log($"[Analytics] Death recorded (offline) - Type: {deathType}, Pos: {position}, Time: {timeSinceStart}s");
        }
    }

    private IEnumerator RecordDeathCoroutine(string deathType, Vector2 position, int timeSinceStart)
    {
        DeathData data = new DeathData
        {
            session_id = currentSessionId,
            device_id = deviceId,
            death_type = deathType,
            death_location_x = position.x,
            death_location_y = position.y,
            time_since_start = timeSinceStart
        };

        string jsonData = JsonUtility.ToJson(data);
        UnityWebRequest request = UnityWebRequest.Post(
            serverConfig.serverURL + "/api/death",
            jsonData,
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"[Analytics] Death recorded - Type: {deathType}, Session: {currentSessionId}");
        }
        else
        {
            Debug.LogWarning("[Analytics] Failed to record death: " + request.error);
        }
    }

    // ==================== Dash Tracking ====================

    /// <summary>
    /// Call this every time the player dashes
    /// </summary>
    public void RecordDash()
    {
        dashCount++;
    }

    /// <summary>
    /// Get current dash count for this session
    /// </summary>
    public int GetDashCount()
    {
        return dashCount;
    }

    // ==================== Helper Methods ====================

    private int GetCurrentCoins()
    {
        // Try to get coins from PlayerPrefs or your coin system
        return PlayerPrefs.GetInt("Coins", 0);
    }

    /// <summary>
    /// Set player name for analytics
    /// </summary>
    public void SetPlayerName(string name)
    {
        currentPlayerName = name;
    }

    /// <summary>
    /// Set character being used
    /// </summary>
    public void SetCharacter(string character)
    {
        currentCharacter = character;
    }

    // ==================== Debug Methods ====================

    [ContextMenu("Test Start Session")]
    public void TestStartSession()
    {
        StartSession("TestPlayer", "TestCharacter");
    }

    [ContextMenu("Test Record Death")]
    public void TestRecordDeath()
    {
        RecordDeath("monster", new Vector2(100, 5));
    }

    [ContextMenu("Test End Session")]
    public void TestEndSession()
    {
        EndSession(1500, 10);
    }
}