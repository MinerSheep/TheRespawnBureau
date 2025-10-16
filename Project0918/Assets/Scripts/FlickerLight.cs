
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    [Header("Settings")]
    public float flickerSpeed = 5f;
    public float flickerAmount = 0.1f;
    public float baseIntensity = 0.6f;
    public bool removeSprite = true;
    public float rotateSpeed = 5;

    [Header("References")]
    [SerializeField] public GameObject spriteMask;
    [SerializeField] public GameObject spriteMask2;



    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null && removeSprite)
            sr.sprite = null;
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        spriteMask.GetComponent<SpriteMask>().alphaCutoff = Mathf.Clamp01(baseIntensity + (noise - 0.5f) * flickerAmount);

        if (spriteMask2 != null)
        {
            spriteMask2.GetComponent<SpriteMask>().alphaCutoff = Mathf.Clamp01(baseIntensity + (noise - 0.5f) * flickerAmount);

            spriteMask2.transform.Rotate(new Vector3(Time.deltaTime * rotateSpeed, 0f, 0f));
        }
    }
}
