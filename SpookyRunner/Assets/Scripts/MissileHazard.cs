using UnityEngine;

public class MissileHazard : MonoBehaviour
{
    public HUD staminaHUD;

    [SerializeField] private float missileVelocity;
    [SerializeField] private float lifespanTimer;
    [SerializeField] private bool passedPlayer;
    [SerializeField] private bool damagedPlayer;
    public int projectileHealth;
    public int DamageAmount;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !damagedPlayer)
        {
            staminaHUD.ChangeStamina(-DamageAmount);

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
