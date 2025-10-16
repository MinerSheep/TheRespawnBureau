using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    [System.Serializable]
    public class BufferedInput
    {
        public string actionName;
        public float time;
        public BufferedInput(string name, float time) { actionName = name; this.time = time; }
    }

    public float bufferTime = 0.2f;
    private PlayerControls controls;
    private List<BufferedInput> buffer = new List<BufferedInput>();

    // Enable/disable controls
    public void OnEnable() => controls.Enable();
    public void OnDisable() => controls.Disable();

    // On awake, add inputs to buffer
    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.FlipFlashlight.performed += ctx => AddToBuffer("FlipFlashlight");
        controls.Player.Jump.performed += ctx => AddToBuffer("Jump");
        controls.Player.Crouch.performed += ctx => AddToBuffer("Crouch");
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

    // Adding input to buffer
    private void AddToBuffer(string action)
    {
        buffer.Add(new BufferedInput(action, Time.time));
    }

    // Request input of a certain type
    public bool Consume(string action)
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].actionName == action)
            {
                buffer.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}
