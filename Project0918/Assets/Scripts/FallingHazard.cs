using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    [SerializeField] private bool isFalling;
    [SerializeField] private bool damagedPlayer;
    private Rigidbody2D rb;

    PlayerController playerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.FindAnyObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        
        if(hit.collider.gameObject.CompareTag("Player"))
        {
            rb.simulated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damagedPlayer)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);

            Debug.Log("Falling hit landed");
            destroyHazard();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            destroyHazard();
        }
    }

    private void destroyHazard()
    {
        Destroy(gameObject);
    }
}
