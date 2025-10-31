using Unity.VisualScripting;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lowClampRandTimer;
    [SerializeField] private float highClampRandTimer;
    [SerializeField] private float spawnTimer = 0;
    public float spawnRate;
    public float warningOffsetFromEdge = 50f;    // How far from the edge (in pixels)

    [Header("References")]
    public Camera mainCamera;
    public GameObject prefabToSpawn;  // Prefab to spawn on edge
    public GameObject missilePrefab;
    public GameObject boulderPrefab;



    void Start()
    {
        if (spawnTimer == 0)
            spawnTimer = Random.Range(lowClampRandTimer, highClampRandTimer);
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime * spawnRate;

        if (spawnTimer < 0)
        {
            Debug.Log("spawntimer");
            switch (Random.Range(0, 1))
            {
                case 1:
                    SpawnPrefab(missilePrefab);
                    break;
                case 0:
                    SpawnPrefab(boulderPrefab);
                    break;
            }
            spawnTimer = Random.Range(lowClampRandTimer, highClampRandTimer);
        }
    }

    private void SpawnPrefab(GameObject prefab)
    {
        Debug.Log("spawning " + prefab.name);

        Transform playerTransform = FindAnyObjectByType<PlayerController>().transform;
        Transform groundTrigger = playerTransform.Find("GroundTrigger");
        Transform headTrigger = playerTransform.Find("HeadTrigger");

        GameObject target = Instantiate(prefab);
        float y = Random.Range(groundTrigger.position.y, headTrigger.position.y);
        target.transform.position = new Vector3(target.transform.position.x, y, target.transform.position.z);
        target.SetActive(false);
        Warning(target);
    }

    public void Warning(GameObject target)
    {
        if (!target || !prefabToSpawn || !mainCamera)
        {
            Debug.LogWarning("EdgeSpawner: Missing references!");
            return;
        }

        // Convert world position to screen position
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.transform.position);

        // Early-out if target is behind the camera
        if (screenPos.z < 0)
        {
            Debug.Log("Target is behind the camera.");
            return;
        }

        // Screen dimensions
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
            spawnScreenPos = new Vector3(warningOffsetFromEdge, screenPos.y, screenPos.z);
        else if (minDist == distRight)
            spawnScreenPos = new Vector3(screenWidth - warningOffsetFromEdge, screenPos.y, screenPos.z);
        else if (minDist == distTop)
            spawnScreenPos = new Vector3(screenPos.x, screenHeight - warningOffsetFromEdge, screenPos.z);
        else // bottom
            spawnScreenPos = new Vector3(screenPos.x, warningOffsetFromEdge, screenPos.z);

        // Convert back to world space
        Vector3 spawnWorldPos = mainCamera.ScreenToWorldPoint(spawnScreenPos);

        // Spawn the prefab
        Instantiate(prefabToSpawn, spawnWorldPos, Quaternion.identity).GetComponent<HazardWarning>().hazard = target;
    }
}
