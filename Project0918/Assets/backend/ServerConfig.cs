using UnityEngine;

[CreateAssetMenu(fileName = "ServerConfig", menuName = "Game/Server Config")]
public class ServerConfig : ScriptableObject
{
    [Header("Server Settings")]
    [Tooltip("Uncheck for offline mode during development, check when deploying")]
    public bool useOnlineServer = false;

    [Tooltip("School server URL (Only admin should modify this)")]
    public string serverURL = "http://localhost/game_api";

    [Header("Information")]
    [TextArea(3, 5)]
    public string notice = "Team members only need to care about 'Use Online Server' checkbox!\nServer URL is managed by administrator.";
}
