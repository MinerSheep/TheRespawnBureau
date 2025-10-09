
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(FlashLight))]
public class FlickerLight : MonoBehaviour
{
    [HideInInspector] public FlashLight flashlight; // URP 2D light
    public float flickerSpeed = 5f;
    public float flickerAmount = 0.1f;
    public float baseIntensity = 0.6f;

    void Start()
    {
        flashlight = GetComponent<FlashLight>();
    }
    
    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        flashlight.spriteMask.GetComponent<SpriteMask>().alphaCutoff = Mathf.Clamp01(baseIntensity + (noise - 0.5f) * flickerAmount);
    }
}
