using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    public float bufferTime = 0.2f;
    private PlayerControls controls;
    private List<BufferedInput> buffer = new List<BufferedInput>();
    private Dictionary<string, HeldInput> heldBuffer = new Dictionary<string, HeldInput>();

    public void OnEnable() => controls.Enable();
    public void OnDisable() => controls.Disable();

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.FlipFlashlight.performed += ctx => AddToBuffer("FlipFlashlight");
        controls.Player.Crouch.performed += ctx => AddToBuffer("Crouch");

        // Holding keys
        heldBuffer["Jump"] = new HeldInput();
        controls.Player.Jump.performed += ctx => StartHold("Jump");
        controls.Player.Jump.canceled += ctx => EndHold("Jump");
    }

    void Update()
    {
        // Clear any expired inputs
        for (int i = buffer.Count - 1; i >= 0; i--)
        {
            if (Time.time - buffer[i].time > bufferTime)
                buffer.RemoveAt(i);
        }
    }

    private void AddToBuffer(string action)
    {
        buffer.Add(new BufferedInput(action, Time.time));
    }

    private void StartHold(string action)
    {
        AddToBuffer(action);

        heldBuffer[action].held = true;
        heldBuffer[action].time = Time.time;
    }

    private void EndHold(string action)
    {
        heldBuffer[action].held = false;
    }

    // Request input of a certain type
    public bool Consume(string action)
    {
        bool ret = false;

        // If action is holdable, return true while it is held
        if (heldBuffer.ContainsKey(action))
            ret = heldBuffer[action].held;

        // If input was triggered within bufferTime, consume it and return true
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].actionName == action)
            {
                buffer.RemoveAt(i);
                ret = true;
            }
        }

        return ret;
    }

    [System.Serializable]
    public class BufferedInput
    {
        public string actionName;
        public float time;
        public BufferedInput(string name, float time) { actionName = name; this.time = time; }
    }
    [System.Serializable]
    public class HeldInput
    {
        public bool held;
        public float time;
        public HeldInput() { this.held = false; this.time = 0.0f; }
    }
}
