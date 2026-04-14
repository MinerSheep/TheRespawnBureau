using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject ParticlePrefab;
    public int quantity = 0;
    public float speed = 1.0f;
    public float range = 0; //range of of the angle it will emit at

    public void Spawn()
    {
        if (ParticlePrefab == null)
        {
            Debug.Log("Error: failed to find Prefab");
            return;
        }
        
        float angleStep = 360.0f / quantity;

        for (int i = 0; i < quantity; i++)
        {
            float angle = i * angleStep;

            GameObject particle = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);

            Rigidbody2D rigidbody = particle.GetComponent<Rigidbody2D>();

            Vector2 startDirect = Quaternion.Euler(0, 0, angle) * Vector2.right;
            rigidbody.linearVelocity = startDirect * speed;

        }
    }
}   
