using UnityEngine;

public class FlyToPoint : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public float startSpeed = 1.0f;
    public float startAngle = 0.0f;
    public float startupinvul = 0.1f;
    public float constantforce = 2.0f;
    public float maxSpeed = 3.0f;
    public float bufferRange = 1.0f;
    public float lifetime = 10.0f;
    public GameObject finalPosition; 

    float time = 0.0f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        if (rigidbody == null)
        {
            Debug.LogError("Missing Rigidbody2D");
            return;
        }

        Vector2 startDirect = Quaternion.Euler(0, 0, startAngle) * Vector2.right;
        rigidbody.linearVelocity = startDirect * startSpeed;
    }

    void FixedUpdate()
    {
        if (finalPosition == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 targetPos = finalPosition.transform.position;
        Vector2 constDirect = targetPos - rigidbody.position;

        // destroy when close enough
        if (constDirect.magnitude < bufferRange && time > startupinvul || time > lifetime)
        {
            Destroy(gameObject);
            return;
        }

        // apply force toward moving target
        Vector2 force = constDirect.normalized * constantforce;
        rigidbody.linearVelocity += force * Time.deltaTime;

        // clamp speed
        if (rigidbody.linearVelocity.magnitude > maxSpeed)
        {
            rigidbody.linearVelocity =
                rigidbody.linearVelocity.normalized * maxSpeed;
        }

        time += Time.deltaTime;
    }
}
