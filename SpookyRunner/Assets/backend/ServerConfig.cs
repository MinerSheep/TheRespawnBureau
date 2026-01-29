using UnityEngine;

// THIS HOLDS THE DATA FOR THE SERVERCONFIG OBJECT THAT ANALYTICS AND DATABASE REFERENCE
// CHANGE THIS IF MORE SERVER INFORMATION IS NEEDED

[CreateAssetMenu(fileName = "ServerConfig", menuName = "Game/Server Config")]
public class ServerConfig : ScriptableObject
{
    [Header("Server Settings")]
    [Tooltip("Check this to use online server. Uncheck for offline development mode.")]
    public bool useOnlineServer = false;

    [Tooltip("Production server URL - DO NOT MODIFY (Admin only)")]
    public string serverURL = "http://129.159.39.145:5000";

    [Header("Information")]
    [TextArea(3, 5)]
    public string notice = "Team Members:\n" +
                           "- Check 'Use Online Server' = Connect to real server\n" +
                           "- Uncheck = Offline mode (fake data for testing)\n\n" +
                           "Server URL should NOT be changed by team members!";
}
