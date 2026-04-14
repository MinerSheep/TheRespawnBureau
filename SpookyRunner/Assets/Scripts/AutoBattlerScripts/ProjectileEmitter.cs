using UnityEngine;

public class ProjectileEmitter : MonoBehaviour
{
    public float Angle = 0.0f;
    public GameObject Projectile;
    public float FireRate = 0.25f;    // time between shots
    public int Quantity = 1;          // number of projectiles
    public float Spread = 0.0f;       // the angle of spread between projectiles (used is Quanity is <1)
    public float Speed = 1.0f;        // speed of the projectile
    public float Radius = 0.5f;        // distance from emittion point

    float fireTimer = 0.0f;


    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = FireRate;
        }
    }

    void Fire()
    {
        if (Projectile == null) 
            return;

        // If only 1 projectile, just fire at Angle
        if (Quantity <= 1)
        {
            SpawnProjectile(Angle);
            return;
        }

        // Spread across total Spread angle
        float startAngle = Angle - Spread * 0.5f;
        float angleStep = Spread / (Quantity - 1);

        for (int i = 0; i < Quantity; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            SpawnProjectile(currentAngle);
        }
    }

    void SpawnProjectile(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        Vector3 spawnPosition = transform.position + (Vector3)(direction * Radius);

        GameObject proj = Instantiate(Projectile, spawnPosition, Quaternion.Euler(0, 0, angle));

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * Speed;
        }
    }

}
