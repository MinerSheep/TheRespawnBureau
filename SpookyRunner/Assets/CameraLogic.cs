using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraEvents
{
    public static event Action<bool> OnGrounded;

    public static void TriggerGrounded(bool flag) { OnGrounded?.Invoke(flag); }
}

public class CameraLogic : MonoBehaviour
{
    public GameObject followTarget;

    float xoffset;
    float yoffset;
    float lerpTime = 1.0f;

    private void Start()
    {
        CameraEvents.OnGrounded += UpdateY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(followTarget.transform.position.x + xoffset, transform.position.y);
    }

    private bool lerping = false;
    void UpdateY(bool grounded)
    {
        if (!grounded)
            return;

        lerping = false;

        float yoffsetcurr = transform.position.y - followTarget.transform.position.y;

        if (yoffsetcurr < yoffset || yoffset < yoffsetcurr)
            StartCoroutine(LerpY());
    }

    IEnumerator LerpY()
    {
        float time = 0.0f;
        float starty = transform.position.y;

        lerping = true;

        while (time < lerpTime && lerping)
        {
            time += Time.deltaTime;

            transform.position = new Vector2(transform.position.x, Mathf.Lerp(starty, followTarget.transform.position.y + yoffset, time));
            
            yield return null;
        }
    }

    private void OnDestroy()
    {
        CameraEvents.OnGrounded -= UpdateY;
    }
}
