using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    [SerializeField] private bool isFalling;
    private Rigidbody2D rb;
    public LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 300, playerLayer);

        if (raycastHit.collider.gameObject.CompareTag("Player"))
        {
            isFalling = true;
        }

        if(!isFalling)
        {
            rb.linearVelocityY = 0f;
        }
    }
}
