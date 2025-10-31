using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class HazardWarning : MonoBehaviour
{
    public float flashDuration = 3f;
    public float flashSpeed = 5f;
    public GameObject hazard;

    private Image image;

    void OnEnable()
    {
        image = GetComponent<Image>();
        StartCoroutine(FlashAndDestroy());
    }

    IEnumerator FlashAndDestroy()
    {
        float time = 0f;
        Color baseColor = image.color;

        while (time < flashDuration)
        {
            float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
            image.color = new Color(baseColor.r, baseColor.g, baseColor.b, t);
            time += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (hazard != null)
            hazard.SetActive(true);
    }
}
