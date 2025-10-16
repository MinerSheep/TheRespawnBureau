using UnityEngine;

public class MissileHazard : MonoBehaviour
{
    [SerializeField] private float missileVelocity;
    [SerializeField] private float lifespanTimer;
    [SerializeField] private bool passedPlayer;
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
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playercontroller = collision.gameObject.GetComponent<PlayerController>();
            playercontroller.LoseHealth();

            Debug.Log("Missle hit landed");
        }
    }
}
