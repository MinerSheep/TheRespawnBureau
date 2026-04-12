using UnityEngine;

public class MinionAIHandler : MonoBehaviour
{
    MinionStats Stats;
    ProjectileEmitter Emitter;
    public GameObject Opponent;
    public Rigidbody2D rb;

    float strafeTimer;
    float strafeDirection;
    Vector2 externalVelocity;

    [SerializeField] SpriteRenderer sr;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    int Points = 0;


    void Awake()
    {
        Stats = GetComponent<MinionStats>();
        Emitter = GetComponent<ProjectileEmitter>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Stats == null)
            Debug.LogError("MinionAIHandler Could not find the MinionStats Component");
        if (Emitter == null)
            Debug.LogError("MinionAIHandler Could not find the ProjectileEmitter Component");
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Opponent == null) 
            return;

        ControlFacing();
        HandleMovement();

    }

    void ControlFacing()
    {
        Vector2 direction = Opponent.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float turnSpeed = Stats.Focus;

        Emitter.Angle = Mathf.MoveTowardsAngle(
            Emitter.Angle,
            targetAngle,
            turnSpeed * Time.deltaTime
        );

        if (sr != null)
            SetSpriteFromAngle(Emitter.Angle);
    }

    void SetSpriteFromAngle(float angle)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;

        if (angle >= -45f && angle < 45f)
        {
            sr.sprite = rightSprite;
        }
        else if (angle >= 45f && angle < 135f)
        {
            sr.sprite = upSprite;
        }
        else if (angle >= -135f && angle < -45f)
        {
            sr.sprite = downSprite;
        }
        else
        {
            sr.sprite = leftSprite;
        }
    }

    public void AddExternalForce(Vector2 force)
    {
        externalVelocity += force;
    }

    void HandleMovement()
    {
        if (Opponent == null)
            return;

        Vector2 toOpponent = Opponent.transform.position - transform.position;
        float distance = toOpponent.magnitude;
        Vector2 forward = toOpponent.normalized;

        Vector2 movement = Vector2.zero;

        // Maintain preferred aggression distance
        if (distance > Stats.Agression)
            movement += forward * Stats.Speed;
        else if (distance < Stats.Agression)
            movement -= forward * Stats.Speed;

        // Strafe logic
        strafeTimer -= Time.fixedDeltaTime;

        if (strafeTimer <= 0f)
        {
            strafeDirection = Random.Range(-1f, 1f);
            strafeTimer = Stats.Variablity;
        }

        Vector2 perpendicular = new Vector2(-forward.y, forward.x);

        movement += perpendicular * strafeDirection * Stats.Caution;

        rb.linearVelocity = movement + externalVelocity;

        externalVelocity = Vector2.Lerp(externalVelocity, Vector2.zero, 5f * Time.fixedDeltaTime);
    }
}
