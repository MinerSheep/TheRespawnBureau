using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public float batteryMax = 100.0f;
    public float batteryUseRate = 1.0f;
    public float batteryCurrent;

    public GameObject flashLight;
    public bool flashLightOn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        batteryCurrent = batteryMax;
    }

    // Update is called once per frame
    void Update()
    {
        // flashlight using battery
        if (flashLightOn)
        {
            batteryCurrent -= batteryUseRate * Time.deltaTime;
        }

        // flashlight run out of battery
        if (batteryCurrent <= 0)
        {
            flashLightOn = false;
        }

        if (!flashLightOn)
        {
            FlashLightOff();
        }
    }

    void FlashLightOff() 
    {
        flashLight.SetActive(false);
    }
    void FlashLightOn()
    {
        flashLight.SetActive(true);
    }
    public void BatteryChange(float batteryChange = 0f)
    {
        batteryCurrent += batteryChange;
    }

    void flip()
    {

    }
}
