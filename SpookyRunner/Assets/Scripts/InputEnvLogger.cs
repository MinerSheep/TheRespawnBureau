using System.Collections;
using UnityEngine;

public class InputEnvLogger : MonoBehaviour
{
    private static InputEnvLogger _instance;

    [Header("Legacy Fallback Polling")]
    [Tooltip("New Input System Check if no event for legacy / Watch the changes for sec")]
    public float legacyPollInterval = 1.0f;

    private bool _lastController;

    void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        DeviceDetector.OnControllerPresenceChanged += OnControllerPresenceChanged;
    }

    void Start()
    {
        LogSnapshot(prefix: "[Init] ");

        _lastController = DeviceDetector.IsController;
        StartCoroutine(LegacyPollLoop());
    }

    void OnDisable()
    {
        DeviceDetector.OnControllerPresenceChanged -= OnControllerPresenceChanged;
    }

    void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }

    private void OnControllerPresenceChanged(bool hasPad)
    {
        Debug.Log($"[InputEnvLogger] Controller presence changed: {hasPad}");
        if (hasPad)
        {
            Debug.Log($"[InputEnvLogger] Brand guess -> PS:{DeviceDetector.IsPlayStation}, Xbox:{DeviceDetector.IsXbox}");
        }
    }

    private IEnumerator LegacyPollLoop()
    {
        var wait = new WaitForSeconds(legacyPollInterval > 0 ? legacyPollInterval : 1f);
        while (true)
        {
            yield return wait;

            bool now = DeviceDetector.IsController;
            if (now != _lastController)
            {
                _lastController = now;
                Debug.Log($"[InputEnvLogger] (Polling) Controller presence changed: {now}");
                if (now)
                {
                    Debug.Log($"[InputEnvLogger] (Polling) Brand guess -> PS:{DeviceDetector.IsPlayStation}, Xbox:{DeviceDetector.IsXbox}");
                }
            }
        }
    }

    private void LogSnapshot(string prefix = "")
    {
        string platform =
#if UNITY_EDITOR
            "Editor";
#elif UNITY_STANDALONE_WIN
            "Windows Standalone";
#elif UNITY_STANDALONE_OSX
            "macOS Standalone";
#elif UNITY_STANDALONE_LINUX
            "Linux Standalone";
#elif UNITY_ANDROID
            "Android";
#elif UNITY_IOS
            "iOS";
#elif UNITY_WEBGL
            "WebGL";
#else
            Application.platform.ToString();
#endif

        Debug.Log($"{prefix}[InputEnvLogger] Platform: {platform}");
        Debug.Log($"{prefix}[InputEnvLogger] IsMobile: {DeviceDetector.IsMobile}");
        Debug.Log($"{prefix}[InputEnvLogger] IsDesktop: {DeviceDetector.IsDesktop}");
        Debug.Log($"{prefix}[InputEnvLogger] IsController: {DeviceDetector.IsController}");

#if ENABLE_INPUT_SYSTEM
        Debug.Log($"{prefix}[InputEnvLogger] Input System: New (ENABLE_INPUT_SYSTEM=ON)");
#else
        Debug.Log($"{prefix}[InputEnvLogger] Input System: Legacy (ENABLE_INPUT_SYSTEM=OFF)");
#endif

        if (DeviceDetector.IsController)
        {
            Debug.Log($"{prefix}[InputEnvLogger] Brand guess -> PS:{DeviceDetector.IsPlayStation}, Xbox:{DeviceDetector.IsXbox}");
        }
    }
}
