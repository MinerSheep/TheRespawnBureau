using System.Collections;
using UnityEngine;

public class MissileHazard : MonoBehaviour
{
    [SerializeField] private float missileVelocity;
    [SerializeField] private float lifespanTimer;
    [SerializeField] private bool passedPlayer;
    [SerializeField] private bool damagedPlayer;
    public int projectileHealth;
    public int DamageAmount;
    private Rigidbody2D rb;
    private Transform tf;

    public GameObject ExplosionPSPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        rb.linearVelocity = new Vector2(-missileVelocity, 0);
        tf.rotation = Quaternion.identity;
    }

    private void OnBecameVisible()
    {
        passedPlayer = true;
    }

    private void OnBecameInvisible()
    {
        if (passedPlayer && isActiveAndEnabled)
        {
            StartCoroutine(WaitThreeSeconds());
        }
    }

    public IEnumerator WaitThreeSeconds()
    {
        yield return new WaitForSeconds(3.0f);
        destroyProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{other.tag}");
        if (other.tag == "Player" && !damagedPlayer)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(DamageAmount);

            Vector2 hitLocation = new Vector2(transform.position.x, transform.position.y);

            GameObject explosion = Instantiate(ExplosionPSPrefab, hitLocation, Quaternion.identity);

            explosion.GetComponent<ParticleSystem>().Play();

            Debug.Log("Missile hit landed");
            damagedPlayer = true;
            destroyProjectile();
        }
        else if (other.CompareTag("Ground"))
        {
            //destroyProjectile();
        }
    }

    public void projectileDamaged(int incomingDamage)
    {
        projectileHealth -= incomingDamage;
        if (projectileHealth <= 0)
        {
            destroyProjectile();
        }
    }

    private void destroyProjectile()
    {
        Destroy(gameObject);
    }
}
