using UnityEngine;

public class SwayingLight : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAngle = 30f;   // Maximum angle to sway
    public float swaySpeed = 2f;    // Speed of swaying

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swaySpeed) * swayAngle;
        transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, angle);
    }
}
