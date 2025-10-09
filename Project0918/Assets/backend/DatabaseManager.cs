using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ScoreData
{
    public string username;
    public int score;
}

[Serializable]
public class ScoreResponse
{
    public bool success;
    public string message;
    public int best_score;
}

[Serializable]
public class LeaderboardEntry
{
    public string username;
    public int best_score;
}

[Serializable]
public class LeaderboardResponse
{
    public bool success;
    public LeaderboardEntry[] leaderboard;
}

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

    void Start()
    {
        if (serverConfig == null)
        {
            Debug.LogError("ServerConfig is not assigned! Please assign it in the Inspector.");
            isOfflineMode = true;
            return;
        }

        if (!serverConfig.useOnlineServer)
        {
            isOfflineMode = true;
            Debug.Log("Offline mode enabled (Development)");
            InitMockData();
        }
        else
        {
            Debug.Log("Online mode enabled - Server: " + serverConfig.serverURL);
        }

        // Run test if enabled
        if (testOnStart)
        {
            TestServerConnection();
        }
    }

    // Test server connection and functionality
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

    // Test sequence coroutine
    private IEnumerator TestSequence()
    {
        // Test 1: Save score
        Debug.Log("Test 1: Saving score...");
        SaveScore(testUsername, testScore);
        yield return new WaitForSeconds(2f);

        // Test 2: Get individual score
        Debug.Log("Test 2: Getting individual score...");
        GetScore(testUsername);
        yield return new WaitForSeconds(2f);

        // Test 3: Get leaderboard
        Debug.Log("Test 3: Getting leaderboard...");
        GetLeaderboard(5);
        yield return new WaitForSeconds(2f);

        Debug.Log("========================================");
        Debug.Log("SERVER CONNECTION TEST COMPLETE");
        Debug.Log("Check logs above for results");
        Debug.Log("========================================");
    }

    // Initialize mock data for offline testing
    private void InitMockData()
    {
        offlineScores["Player1"] = 1000;
        offlineScores["Player2"] = 850;
        offlineScores["Player3"] = 700;
        offlineScores["DevTest"] = 500;
    }

    // Save score to database or offline storage
    public void SaveScore(string username, int score)
    {
        if (isOfflineMode)
        {
            SaveScoreOffline(username, score);
        }
        else
        {
            StartCoroutine(SaveScoreCoroutine(username, score));
        }
    }

    // Save score in offline mode
    private void SaveScoreOffline(string username, int score)
    {
        if (offlineScores.ContainsKey(username))
        {
            if (score > offlineScores[username])
            {
                offlineScores[username] = score;
                Debug.Log($"[Offline] New record! {username}: {score} points");
            }
            else
            {
                Debug.Log($"[Offline] Record maintained. Best: {offlineScores[username]} points");
            }
        }
        else
        {
            offlineScores[username] = score;
            Debug.Log($"[Offline] First score! {username}: {score} points");
        }
    }

    // Save score coroutine for online mode
    private IEnumerator SaveScoreCoroutine(string username, int score)
    {
        ScoreData data = new ScoreData { username = username, score = score };
        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(serverConfig.serverURL + "/save_score.php", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Check if response is HTML instead of JSON
            if (request.downloadHandler.text.Contains("<html>"))
            {
                Debug.LogWarning("Server returned HTML - Switching to offline mode");
                isOfflineMode = true;
                SaveScoreOffline(username, score);
                yield break;
            }

            try
            {
                ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(request.downloadHandler.text);
                Debug.Log($"[Online] {response.message} - Best: {response.best_score} points");
            }
            catch (Exception e)
            {
                Debug.LogWarning("Server error - Switching to offline mode: " + e.Message);
                isOfflineMode = true;
                SaveScoreOffline(username, score);
            }
        }
        else
        {
            Debug.LogWarning("Server connection failed - Using offline mode");
            isOfflineMode = true;
            SaveScoreOffline(username, score);
        }
    }

    // Get individual player score
    public void GetScore(string username)
    {
        if (isOfflineMode)
        {
            GetScoreOffline(username);
        }
        else
        {
            StartCoroutine(GetScoreCoroutine(username));
        }
    }

    // Get score in offline mode
    private void GetScoreOffline(string username)
    {
        if (offlineScores.ContainsKey(username))
        {
            Debug.Log($"[Offline] {username}: {offlineScores[username]} points");
        }
        else
        {
            Debug.Log($"[Offline] {username} - No record found");
        }
    }

    // Get score coroutine for online mode
    private IEnumerator GetScoreCoroutine(string username)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverConfig.serverURL + "/get_score.php?username=" + username);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler.text.Contains("<html>"))
            {
                Debug.LogWarning("Server returned HTML - Switching to offline mode");
                isOfflineMode = true;
                GetScoreOffline(username);
                yield break;
            }

            try
            {
                ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    Debug.Log($"[Online] {username}: {response.best_score} points");
                }
                else
                {
                    Debug.Log($"[Online] {response.message}");
                }
            }
            catch
            {
                isOfflineMode = true;
                GetScoreOffline(username);
            }
        }
        else
        {
            isOfflineMode = true;
            GetScoreOffline(username);
        }
    }

    // Get leaderboard
    public void GetLeaderboard(int limit = 10)
    {
        if (isOfflineMode)
        {
            GetLeaderboardOffline(limit);
        }
        else
        {
            StartCoroutine(GetLeaderboardCoroutine(limit));
        }
    }

    // Get leaderboard in offline mode
    private void GetLeaderboardOffline(int limit)
    {
        Debug.Log("[Offline] === Leaderboard ===");

        var sortedScores = new List<KeyValuePair<string, int>>(offlineScores);
        sortedScores.Sort((a, b) => b.Value.CompareTo(a.Value));

        int count = Mathf.Min(limit, sortedScores.Count);
        for (int i = 0; i < count; i++)
        {
            Debug.Log($"{i + 1}. {sortedScores[i].Key} - {sortedScores[i].Value} points");
        }
    }

    // Get leaderboard coroutine for online mode
    private IEnumerator GetLeaderboardCoroutine(int limit)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverConfig.serverURL + "/get_leaderboard.php?limit=" + limit);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler.text.Contains("<html>"))
            {
                Debug.LogWarning("Server returned HTML - Switching to offline mode");
                isOfflineMode = true;
                GetLeaderboardOffline(limit);
                yield break;
            }

            try
            {
                LeaderboardResponse response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    Debug.Log("[Online] === Leaderboard ===");
                    for (int i = 0; i < response.leaderboard.Length; i++)
                    {
                        var entry = response.leaderboard[i];
                        Debug.Log($"{i + 1}. {entry.username} - {entry.best_score} points");
                    }
                }
            }
            catch
            {
                isOfflineMode = true;
                GetLeaderboardOffline(limit);
            }
        }
        else
        {
            isOfflineMode = true;
            GetLeaderboardOffline(limit);
        }
    }
}