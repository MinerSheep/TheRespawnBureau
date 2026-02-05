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

    public float xoffset;
    public float yoffset;
    public float ymax = -1;
    public float lerpTime = 1.0f;

    private void Start()
    {
        CameraEvents.OnGrounded += UpdateY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followTarget.transform.position.x + xoffset, transform.position.y, -10);
    }

    private bool lerping = false;
    void UpdateY(bool grounded)
    {
        if (!grounded)
            return;

        lerping = false;

        float yoffsetcurr = transform.position.y - followTarget.transform.position.y;

        if (followTarget.transform.position.y + yoffset < ymax && yoffsetcurr < yoffset - 0.5f || yoffset + 0.5f < yoffsetcurr)
            StartCoroutine(LerpY());
    }

    IEnumerator LerpY()
    {
        float time = 0.0f;
        float starty = transform.position.y;
        float endy = followTarget.transform.position.y + yoffset;

        lerping = true;

        while (time < lerpTime && lerping)
        {
            time += Time.deltaTime;

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(starty, endy, time), -10);
            
            yield return null;
        }
    }

    private void OnDestroy()
    {
        CameraEvents.OnGrounded -= UpdateY;
    }
}
