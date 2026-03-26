using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class HazardWarning : MonoBehaviour
{
    public GameObject hazard;
    public float spawnXDistance = 0;
    public bool timed = true;
    public float flashDuration = 3f;
    public float flashSpeed = 5f;
    
    private RectTransform rectTransform;
    private Image image;

    private Transform playerTransform;
    private HazardManager hm;

    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().transform;
        hm = HazardManager.instance;
    }

    void Update()
    {
        // Convert world position to screen position
        Vector3 screenPos = hm.mainCamera.WorldToScreenPoint(hazard.transform.position);

        // Early-out if target is behind the camera
        if (screenPos.z < 0)
        {
            Debug.Log("Target is behind the camera.");
            return;
        }

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Distances to edges
        float distLeft = screenPos.x;
        float distRight = screenWidth - screenPos.x;
        float distBottom = screenPos.y;
        float distTop = screenHeight - screenPos.y;

        // Find the closest edge
        float minDist = Mathf.Min(distLeft, distRight, distTop, distBottom);
        Vector3 spawnScreenPos = screenPos;

        if (minDist == distLeft)
            spawnScreenPos = new Vector3(hm.warningUIOffsetFromEdge, screenPos.y, screenPos.z);
        else if (minDist == distRight)
            spawnScreenPos = new Vector3(screenWidth - hm.warningUIOffsetFromEdge, screenPos.y, screenPos.z);
        else if (minDist == distTop)
            spawnScreenPos = new Vector3(screenPos.x, screenHeight - hm.warningUIOffsetFromEdge, screenPos.z);
        else // bottom
            spawnScreenPos = new Vector3(screenPos.x, hm.warningUIOffsetFromEdge, screenPos.z);

        // Convert back to world space
        rectTransform.position = spawnScreenPos;

        if (spawnXDistance != 0) CheckDistance();
    }

    void CheckDistance()
    {
        if (playerTransform == null)
            return;
        if (Mathf.Abs(hazard.transform.position.x - playerTransform.position.x) <= spawnXDistance)
            Destroy(gameObject);
    }

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        StartCoroutine(FlashAndDestroy());
    }

    IEnumerator FlashAndDestroy()
    {
        float time = 0f;
        Color baseColor = image.color;

        while (time < flashDuration || !timed)
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
        {
            hazard.SetActive(true);
            hazard.transform.parent = hm?.transform;
        }
    }
}
