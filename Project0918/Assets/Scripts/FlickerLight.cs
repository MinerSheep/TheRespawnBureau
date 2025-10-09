
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(FlashLight))]
public class FlickerLight : MonoBehaviour
{
    [HideInInspector] public FlashLight flashlight; // URP 2D light
    [SerializeField] public GameObject spriteMask2;
    public float flickerSpeed = 5f;
    public float flickerAmount = 0.1f;
    public float baseIntensity = 0.6f;
    public float rotateSpeed = 5;

    void Start()
    {
        flashlight = GetComponent<FlashLight>();
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        flashlight.spriteMask.GetComponent<SpriteMask>().alphaCutoff = Mathf.Clamp01(baseIntensity + (noise - 0.5f) * flickerAmount);

        if (spriteMask2 != null)
        {
            spriteMask2.GetComponent<SpriteMask>().alphaCutoff = Mathf.Clamp01(baseIntensity + (noise - 0.5f) * flickerAmount);

            spriteMask2.transform.Rotate(new Vector3(Time.deltaTime * rotateSpeed, 0f, 0f));
        }
    }
}
