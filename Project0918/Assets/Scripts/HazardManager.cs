using UnityEngine;

public class HazardManager : MonoBehaviour
{
    Transform missilePrefab;
    Transform boulderPrefab;

    [SerializeField] private float lowClampRandTimer;
    [SerializeField] private float highClampRandTimer;

    void Start()
    {
        missilePrefab = transform.Find("MissileHazard");
        boulderPrefab = transform.Find("BoulderHazard");

        hazardManager();
    }

    void Update()
    {
        
    }

    public void hazardManager()
    {
        float spawnTimer = Random.Range(lowClampRandTimer, highClampRandTimer);
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
