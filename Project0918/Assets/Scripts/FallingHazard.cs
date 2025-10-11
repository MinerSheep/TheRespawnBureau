using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    [SerializeField] private bool isFalling;
    private Rigidbody2D rb;

    PlayerController playerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.Find("Player_Controller").GetComponent<PlayerController>();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.health -= 1;
        }
    }
}
