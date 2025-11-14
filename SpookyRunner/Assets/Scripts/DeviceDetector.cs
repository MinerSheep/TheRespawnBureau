using System;
using System.Runtime.InteropServices;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;
using System.Reflection;
#endif

public static class DeviceDetector
{
    // Controller presence
#if ENABLE_INPUT_SYSTEM
    static bool _hasGamepad;
    static double _lastUiSwitchTime;
    const double UI_SWITCH_COOLDOWN = 0.25;

    static DeviceDetector()
    {
        RecalcGamepadPresence();

        InputSystem.onDeviceChange += (device, change) =>
        {
            if (!(device is Gamepad)) return;

            bool before = _hasGamepad;
            RecalcGamepadPresence();

            var now = Time.realtimeSinceStartupAsDouble;
            if (before != _hasGamepad && now - _lastUiSwitchTime >= UI_SWITCH_COOLDOWN)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[DeviceDetector] Gamepad presence: {before} -> {_hasGamepad} ({change})");
#endif
                _lastUiSwitchTime = now;
                OnControllerPresenceChanged?.Invoke(_hasGamepad);
            }
        };
    }

    static void RecalcGamepadPresence()
    {
#if UNITY_PS4 || UNITY_PS5 || UNITY_XBOXONE || UNITY_GAMECORE || UNITY_SWITCH
        _hasGamepad = true; 
#else
        _hasGamepad = Gamepad.all.Count > 0;
#endif
    }
#endif

    // ---------------- 1. Controller ------------------------------------------
    public static bool IsController
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _hasGamepad;
#else
#if UNITY_PS4 || UNITY_PS5 || UNITY_XBOXONE || UNITY_GAMECORE || UNITY_SWITCH
            return true;
#else
            var names = Input.GetJoystickNames();
            if (names == null) return false;
            for (int i = 0; i < names.Length; i++)
                if (!string.IsNullOrEmpty(names[i])) return true;
            return false;
#endif
#endif
        }
    }

    public static event Action<bool> OnControllerPresenceChanged;

    public static void UnsubscribeAll() => OnControllerPresenceChanged = null;

    // Controller Brand for UI or smthing else
#if ENABLE_INPUT_SYSTEM

    // PS
    public static bool IsPlayStation
    {
        get
        {
            var pad = Gamepad.current;
            if (pad == null) return false;

            // 1) DualShock
            if (pad is DualShockGamepad) return true;

            // 2) DualSense
            var dualSenseType =
                Type.GetType("UnityEngine.InputSystem.DualSense.DualSenseGamepadHID, Unity.InputSystem", false)
                ?? Type.GetType("UnityEngine.InputSystem.DualShock.DualSenseGamepadHID, Unity.InputSystem", false);
            if (dualSenseType != null && dualSenseType.IsInstanceOfType(pad)) return true;

            // 3) displayName
            var name = pad.displayName ?? string.Empty;
            return name.IndexOf("DualSense", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("PS5", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("PlayStation", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    // Xbox
    public static bool IsXbox
    {
        get
        {
            var pad = Gamepad.current;
            if (pad == null) return false;

            if (pad is XInputController) return true;

            var name = pad.displayName ?? string.Empty;
            return name.IndexOf("Xbox", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
#else
    public static bool IsPlayStation => false;
    public static bool IsXbox => false;
#endif

    // Platform detection for WebGL (Check mobile)
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern bool IsMobileBrowser();
#endif

    //  ---------------- 2. Touch Screen (Android, iOS, WebGL Mobile) --------------
    public static bool IsMobile
    {
        get
        {
#if UNITY_ANDROID || UNITY_IOS
            return true;
#elif UNITY_WEBGL && !UNITY_EDITOR
            try { return IsMobileBrowser(); }
            catch { return false; } //
#else
            return false;
#endif
        }
    }

    // ---------------- 3. Keyboard/Mouse (Windows, WebGL Desktop) ------------------
    public static bool IsDesktop
    {
        get
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return true;
#elif UNITY_WEBGL && !UNITY_EDITOR
            return !IsMobile;
#else
            return false;
#endif
        }
    }
}