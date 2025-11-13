using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Data structure for submitting scores to the server
[Serializable]
public class ScoreData
{
    public string player_name;
    public int score;
}

// Response structure when submitting scores
[Serializable]
public class ScoreResponse
{
    public bool success;
    public string message;
    public int id;
}

// Data structure for leaderboard entries
[Serializable]
public class LeaderboardEntry
{
    public string player_name;
    public int score;
}

// Wrapper for leaderboard array response
[Serializable]
public class LeaderboardWrapper
{
    public LeaderboardEntry[] entries;
}

// Response structure for player rank queries
[Serializable]
public class RankResponse
{
    public int rank;
}

// Manages database operations for leaderboard system
// Supports both online (Node.js server) and offline modes
// Automatically falls back to offline mode if server is unavailable
public class DatabaseManager : MonoBehaviour
{
    [Header("Server Configuration")]
    [SerializeField] private ServerConfig serverConfig;

    [Header("Testing")]
    [SerializeField] private bool testOnStart = false;
    [SerializeField] private string testUsername = "TestPlayer";
    [SerializeField] private int testScore = 999;

    // Offline mode data storage
    private Dictionary<string, int> offlineScores = new Dictionary<string, int>();
    private bool isOfflineMode = false;

    private static DatabaseManager instance;

    // Singleton instance of DatabaseManager
    public static DatabaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<DatabaseManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("DatabaseManager");
                    instance = go.AddComponent<DatabaseManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (serverConfig == null)
        {
            Debug.LogError("[DatabaseManager] ServerConfig is not assigned! Using offline mode.");
            isOfflineMode = true;
            InitMockData();
            return;
        }

        if (!serverConfig.useOnlineServer)
        {
            isOfflineMode = true;
            Debug.Log("[DatabaseManager] Offline mode enabled (Development)");
            InitMockData();
        }
        else
        {
            Debug.Log("[DatabaseManager] Online mode enabled - Server: " + serverConfig.serverURL);
            StartCoroutine(TestServerHealth());
        }

        if (testOnStart)
        {
            TestServerConnection();
        }
    }

    // Tests server health endpoint
    private IEnumerator TestServerHealth()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverConfig.serverURL + "/api/health");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("[DatabaseManager] Server health check passed: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogWarning("[DatabaseManager] Server health check failed - Switching to offline mode");
            isOfflineMode = true;
            InitMockData();
        }
    }

    // Tests server connection and all API endpoints
    // Can be called from Inspector context menu
    [ContextMenu("Test Server Connection")]
    public void TestServerConnection()
    {
        Debug.Log("========================================");
        Debug.Log("SERVER CONNECTION TEST START");
        Debug.Log("========================================");
        Debug.Log("Mode: " + (isOfflineMode ? "OFFLINE" : "ONLINE"));
        Debug.Log("Server URL: " + (serverConfig != null ? serverConfig.serverURL : "NULL"));
        Debug.Log("Test Username: " + testUsername);
        Debug.Log("Test Score: " + testScore);
        Debug.Log("----------------------------------------");

        StartCoroutine(TestSequence());
    }

    // Runs a sequence of tests for all API endpoints
    private IEnumerator TestSequence()
    {
        Debug.Log("Test 1: Saving score...");
        SaveScore(testUsername, testScore);
        yield return new WaitForSeconds(2f);

        Debug.Log("Test 2: Getting leaderboard...");
        GetLeaderboard(5);
        yield return new WaitForSeconds(2f);

        Debug.Log("Test 3: Getting player rank...");
        GetPlayerRank(testUsername);
        yield return new WaitForSeconds(2f);

        Debug.Log("========================================");
        Debug.Log("SERVER CONNECTION TEST COMPLETE");
        Debug.Log("Check logs above for results");
        Debug.Log("========================================");
    }

    // Initialize mock data for offline testing
    private void InitMockData()
    {
        offlineScores.Clear();
        offlineScores["Player1"] = 1000;
        offlineScores["Player2"] = 850;
        offlineScores["Player3"] = 700;
        offlineScores["DevTest"] = 500;
        Debug.Log("[DatabaseManager] Mock data initialized for offline mode");
    }

    // Save player score to database or offline storage
    // <param name="playerName">Name of the player</param>
    // <param name="score">Score achieved</param>
    // <param name="callback">Optional callback with success status</param>
    public void SaveScore(string playerName, int score, Action<bool> callback = null)
    {
        if (isOfflineMode)
        {
            SaveScoreOffline(playerName, score);
            callback?.Invoke(true);
        }
        else
        {
            StartCoroutine(SaveScoreCoroutine(playerName, score, callback));
        }
    }

    // Save score in offline mode
    private void SaveScoreOffline(string playerName, int score)
    {
        if (offlineScores.ContainsKey(playerName))
        {
            if (score > offlineScores[playerName])
            {
                offlineScores[playerName] = score;
                Debug.Log($"[Offline] New record! {playerName}: {score} points");
            }
            else
            {
                Debug.Log($"[Offline] Record maintained. Best: {offlineScores[playerName]} points");
            }
        }
        else
        {
            offlineScores[playerName] = score;
            Debug.Log($"[Offline] First score! {playerName}: {score} points");
        }
    }

    // Save score coroutine for online mode
    // Automatically switches to offline mode if server fails
    private IEnumerator SaveScoreCoroutine(string playerName, int score, Action<bool> callback)
    {
        ScoreData data = new ScoreData { player_name = playerName, score = score };
        string jsonData = JsonUtility.ToJson(data);

        Debug.Log($"[DatabaseManager] Submitting score: {playerName} - {score} points");

        UnityWebRequest request = UnityWebRequest.Post(
            serverConfig.serverURL + "/api/score",
            jsonData,
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    Debug.Log($"[Online] Score saved successfully: {response.message}");
                    callback?.Invoke(true);
                }
                else
                {
                    Debug.LogWarning($"[Online] Server returned error: {response.message}");
                    callback?.Invoke(false);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[DatabaseManager] Server error - Switching to offline mode: " + e.Message);
                isOfflineMode = true;
                SaveScoreOffline(playerName, score);
                callback?.Invoke(true);
            }
        }
        else
        {
            Debug.LogWarning("[DatabaseManager] Server connection failed - Using offline mode: " + request.error);
            isOfflineMode = true;
            SaveScoreOffline(playerName, score);
            callback?.Invoke(true);
        }
    }

    // Get leaderboard entries
    // <param name="limit">Number of top entries to retrieve</param>
    // <param name="callback">Optional callback with list of entries</param>
    public void GetLeaderboard(int limit = 10, Action<List<LeaderboardEntry>> callback = null)
    {
        if (isOfflineMode)
        {
            List<LeaderboardEntry> entries = GetLeaderboardOffline(limit);
            callback?.Invoke(entries);
        }
        else
        {
            StartCoroutine(GetLeaderboardCoroutine(limit, callback));
        }
    }

    // Get leaderboard in offline mode
    private List<LeaderboardEntry> GetLeaderboardOffline(int limit)
    {
        Debug.Log("[Offline] === Leaderboard ===");

        var sortedScores = new List<KeyValuePair<string, int>>(offlineScores);
        sortedScores.Sort((a, b) => b.Value.CompareTo(a.Value));

        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        int count = Mathf.Min(limit, sortedScores.Count);

        for (int i = 0; i < count; i++)
        {
            entries.Add(new LeaderboardEntry
            {
                player_name = sortedScores[i].Key,
                score = sortedScores[i].Value
            });
            Debug.Log($"{i + 1}. {sortedScores[i].Key} - {sortedScores[i].Value} points");
        }

        return entries;
    }

    // Get leaderboard coroutine for online mode
    private IEnumerator GetLeaderboardCoroutine(int limit, Action<List<LeaderboardEntry>> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(
            serverConfig.serverURL + "/api/leaderboard"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                string json = "{\"entries\":" + request.downloadHandler.text + "}";
                LeaderboardWrapper wrapper = JsonUtility.FromJson<LeaderboardWrapper>(json);

                List<LeaderboardEntry> entries = new List<LeaderboardEntry>(wrapper.entries);

                Debug.Log($"[Online] Leaderboard retrieved: {entries.Count} entries");
                for (int i = 0; i < entries.Count; i++)
                {
                    Debug.Log($"{i + 1}. {entries[i].player_name} - {entries[i].score} points");
                }

                callback?.Invoke(entries);
            }
            catch (Exception e)
            {
                Debug.LogWarning("[DatabaseManager] Parse error - Switching to offline mode: " + e.Message);
                isOfflineMode = true;
                List<LeaderboardEntry> entries = GetLeaderboardOffline(limit);
                callback?.Invoke(entries);
            }
        }
        else
        {
            Debug.LogWarning("[DatabaseManager] Server connection failed - Using offline mode");
            isOfflineMode = true;
            List<LeaderboardEntry> entries = GetLeaderboardOffline(limit);
            callback?.Invoke(entries);
        }
    }

    // Get specific player's rank
    // <param name="playerName">Name of the player</param>
    // <param name="callback">Optional callback with rank number</param>
    public void GetPlayerRank(string playerName, Action<int> callback = null)
    {
        if (isOfflineMode)
        {
            int rank = GetPlayerRankOffline(playerName);
            callback?.Invoke(rank);
        }
        else
        {
            StartCoroutine(GetPlayerRankCoroutine(playerName, callback));
        }
    }

    // Get player rank in offline mode
    private int GetPlayerRankOffline(string playerName)
    {
        if (!offlineScores.ContainsKey(playerName))
        {
            Debug.Log($"[Offline] {playerName} - No record found");
            return -1;
        }

        var sortedScores = new List<KeyValuePair<string, int>>(offlineScores);
        sortedScores.Sort((a, b) => b.Value.CompareTo(a.Value));

        for (int i = 0; i < sortedScores.Count; i++)
        {
            if (sortedScores[i].Key == playerName)
            {
                Debug.Log($"[Offline] {playerName} rank: {i + 1}");
                return i + 1;
            }
        }

        return -1;
    }

    // Get player rank coroutine for online mode
    private IEnumerator GetPlayerRankCoroutine(string playerName, Action<int> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(
            serverConfig.serverURL + "/api/rank/" + UnityWebRequest.EscapeURL(playerName)
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                RankResponse response = JsonUtility.FromJson<RankResponse>(request.downloadHandler.text);
                Debug.Log($"[Online] {playerName} rank: {response.rank}");
                callback?.Invoke(response.rank);
            }
            catch (Exception e)
            {
                Debug.LogWarning("[DatabaseManager] Parse error - Switching to offline mode: " + e.Message);
                isOfflineMode = true;
                int rank = GetPlayerRankOffline(playerName);
                callback?.Invoke(rank);
            }
        }
        else
        {
            Debug.LogWarning("[DatabaseManager] Server connection failed - Using offline mode");
            isOfflineMode = true;
            int rank = GetPlayerRankOffline(playerName);
            callback?.Invoke(rank);
        }
    }

    // Check if currently in offline mode
    public bool IsOfflineMode()
    {
        return isOfflineMode;
    }

    // Manually switch to offline mode
    public void ForceOfflineMode()
    {
        isOfflineMode = true;
        InitMockData();
        Debug.Log("[DatabaseManager] Forced offline mode");
    }

    // Manually retry online connection
    public void RetryOnlineConnection()
    {
        if (serverConfig != null && serverConfig.useOnlineServer)
        {
            isOfflineMode = false;
            Debug.Log("[DatabaseManager] Retrying online connection...");
            StartCoroutine(TestServerHealth());
        }
        else
        {
            Debug.LogWarning("[DatabaseManager] Cannot retry - Online server is disabled in ServerConfig");
        }
    }
}