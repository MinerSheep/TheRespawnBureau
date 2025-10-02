using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public float batteryMax = 100.0f;
    public float batteryUseRate = 1.0f;
    public float batteryCurrent;

    public GameObject flashLight;
    public bool flashLightOn = true;
    public KeyCode flashLightflip = KeyCode.F;

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

        //flipping flashlight by flip the sprite mask
        if (Input.GetKeyDown(flashLightflip))
        {
            flip();
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
        flashLight.transform.Rotate(0f, 180f, 0f, Space.Self);
    }
}
