using UnityEngine;

public class MissileHazard : MonoBehaviour
{
    public float missileVelocity;
    public float lifespanTimer;
    private bool passedPlayer;
    private Rigidbody2D rb;
    private Transform tf;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        rb.linearVelocity = new Vector2(-missileVelocity, 0);
    }

    void Update()
    {
        
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
}
