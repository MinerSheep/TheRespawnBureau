using UnityEngine;

public class HazardManager : MonoBehaviour
{
    Transform missilePrefab;
    Transform boulderPrefab;

    [SerializeField] private float lowClampRandTimer;
    [SerializeField] private float highClampRandTimer;
    [SerializeField] private float spawnTimer;

    public float spawnRate;

    void Start()
    {
        missilePrefab = transform.Find("MissileHazard");
        boulderPrefab = transform.Find("BoulderHazard");

        hazardManager();
    }

    void Update()
    {
        spawnTimer -= spawnRate;
    }

    public void hazardManager()
    {
        spawnTimer = Random.Range(lowClampRandTimer, highClampRandTimer);
    }

    private void spawnMissile()
    {
        Instantiate(missilePrefab);
    }

    private void spawnBoulder()
    {
        Instantiate(boulderPrefab);
    }
}
