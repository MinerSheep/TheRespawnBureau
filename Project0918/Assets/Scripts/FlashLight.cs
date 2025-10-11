using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public float batteryMax = 100.0f;
    public float batteryUseRate = 1.0f;
    public float batteryCurrent;

    public GameObject spriteMask;
    public LightMethod lightMethod = LightMethod.Static;
    public bool flashLightOn = true;
    public KeyCode flashLightflip = KeyCode.F;
    //public FlipCamera flipCamera = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        batteryCurrent = batteryMax;

        if (spriteMask == null)
            spriteMask = transform.Find("Sprite Mask")?.gameObject;
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

        switch (lightMethod)
        {
            case LightMethod.FollowMouse:
                float rotationSpeed = 10f;

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = mousePos - (Vector2)transform.position;

                float currentangle = transform.rotation.eulerAngles.z;
                float targetangle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                float smoothAngle = Mathf.LerpAngle(currentangle, targetangle, Time.deltaTime * rotationSpeed);

                transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
                break;
            case LightMethod.FollowMovement:
                break;
        }

        //flipping flashlight by flip the sprite mask
        if (Input.GetKeyDown(flashLightflip))
        {
            flip();
        }
    }

    void FlashLightOff() 
    {
        spriteMask.SetActive(false);
    }
    void FlashLightOn()
    {
        spriteMask.SetActive(true);
    }
    public void BatteryChange(float batteryChange = 0f)
    {
        batteryCurrent += batteryChange;
    }

    void flip()
    {
        spriteMask.transform.Rotate(0f, 180f, 0f, Space.Self);

        //if (flipCamera != null)
        //    flipCamera.Flip();
    }

    public enum LightMethod
    {
        Static,
        FollowMouse,
        FollowMovement
    }
}
