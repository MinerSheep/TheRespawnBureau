using Unity.VisualScripting;
using UnityEngine;


// Attach to the runner scene
public class HazardManager : MonoBehaviour
{
    public static HazardManager instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float lowClampRandTimer = 3;
    [SerializeField] private float highClampRandTimer = 5;
    [SerializeField] private float hazardSpawnOffset = 25;  // distance hazard spawns away from the player when warning is given
    [SerializeField] private SpawnMethod boulderMethod = SpawnMethod.WorldSpace;
    [SerializeField] private float boulderOffset = 4;
    public float spawnRate = 1;
    public float warningUIOffsetFromEdge = 50f;    // How far from the edge (in pixels)

    [Header("Optional Settings")]
    [SerializeField] private float spawnTimer = 0;

    [Header("References")]
    public Camera mainCamera;
    public GameObject prefabToSpawn;  // Prefab to spawn on edge
    public GameObject missilePrefab;
    public GameObject boulderPrefab;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

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
            Transform playerTransform = FindAnyObjectByType<PlayerController>().transform;
            Transform groundTrigger = playerTransform.Find("GroundTrigger");
            Transform headTrigger = playerTransform.Find("HeadTrigger");

            switch (Random.Range(0, 2))
            {
                case 0:
                    GameObject boulder = SpawnPrefab(boulderPrefab,
                    playerTransform.position.x + boulderOffset + (boulderMethod == SpawnMethod.WorldSpace ? hazardSpawnOffset : 0),
                    boulderPrefab.transform.position.y,
                    boulderMethod == SpawnMethod.WorldSpace ? transform : null);
                    
                    Warning(boulder, boulderMethod == SpawnMethod.WorldSpace ? boulderOffset : 0);
                    break;
                case 1:
                    GameObject missle = SpawnPrefab(missilePrefab, missilePrefab.transform.position.x, Random.Range(groundTrigger.position.y, headTrigger.position.y));
                    
                    Warning(missle);
                    break;
            }
            spawnTimer = Random.Range(lowClampRandTimer, highClampRandTimer);
        }
    }

    private GameObject SpawnPrefab(GameObject prefab, float x, float y, Transform parent = null)
    {
        GameObject target = Instantiate(prefab, parent);
        target.transform.position = new Vector3(x, y, target.transform.position.z);
        target.SetActive(false);

        return target;
    }

    public void Warning(GameObject target, float spawnXDistance = 0)
    {
        if (!target || !prefabToSpawn || !mainCamera)
        {
            Debug.LogWarning("EdgeSpawner: Missing references!");
            return;
        }

        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (var player in players)
        {
            // Spawn the warning for every player
            HazardWarning hw = Instantiate(prefabToSpawn, player.hud.transform).GetComponent<HazardWarning>();
            hw.hazard = target;
            hw.timed = spawnXDistance == 0;
            hw.spawnXDistance = spawnXDistance;
        }
    }

    [System.Serializable]
    public enum SpawnMethod
    {
        OffScreenHold, // hazard is spawned and held slightly off screen, appears when timer expires
        WorldSpace     // hazard is spawned far out, appears when player gets close
    };
}
