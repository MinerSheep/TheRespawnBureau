using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector2 currentPosition;
    private float currentShakeDuration = 0.0f;

    void Awake()
    {
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            _camera.transform.position = new Vector3(currentPosition.x + Random.insideUnitCircle.x * shakeAmount, currentPosition.y + Random.insideUnitCircle.y * shakeAmount, 0f);

            currentShakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            currentShakeDuration = 0f;
        }
    }

    public void TriggerShake()
    {
        currentShakeDuration = shakeDuration;
    }
}
