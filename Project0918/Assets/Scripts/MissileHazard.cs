using UnityEngine;

public class MissileHazard : MonoBehaviour
{
    [SerializeField] private float missileVelocity;
    [SerializeField] private float lifespanTimer;
    [SerializeField] private bool passedPlayer;
    [SerializeField] private bool destroyOnGround;
    [SerializeField] private bool damagedPlayer;
    public int projectileHealth;
    private Rigidbody2D rb;
    private Transform tf;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        rb.linearVelocity = new Vector2(-missileVelocity, 0);
    }

    private void OnBecameVisible()
    {
        passedPlayer = true;
    }

    private void OnBecameInvisible()
    {
        if (passedPlayer)
        {
            destroyProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damagedPlayer)
        {
            PlayerController playercontroller = collision.gameObject.GetComponent<PlayerController>();
            playercontroller.LoseHealth();

            Debug.Log("Missle hit landed");
            damagedPlayer = true;
        }

        else if (collision.gameObject.CompareTag("Ground") && destroyOnGround)
        {
            destroyProjectile();
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
