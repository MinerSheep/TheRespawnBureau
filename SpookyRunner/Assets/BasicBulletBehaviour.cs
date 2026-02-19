using UnityEngine;

public class BasicBulletBehaviour : MonoBehaviour
{
    [SerializeField] float Damage = 5.0f;
    [SerializeField] float LifeTime = 3.0f;
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] public bool isEnemy = false;

    float Timer = 0.0f;

    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > LifeTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MinionStats stats = other.GetComponent<MinionStats>();
        BasicBulletBehaviour bullet = other.GetComponent<BasicBulletBehaviour>();

        if (stats != null)
        {
            if (stats.IsEnemy == isEnemy)
                return;

            stats.TakeDamage(Damage);

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 knockDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            }
        }

        if (bullet != null)
        {
            if (bullet.isEnemy == isEnemy)
                return;
        }

        Destroy(gameObject);
    }

}
