using UnityEngine;

public class MinionAIHandler : MonoBehaviour
{
    MinionStats Stats;
    ProjectileEmitter Emitter;
    public GameObject Opponent;

    float strafeTimer;
    float strafeDirection;

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
        
        //find opponent
    }

    // Update is called once per frame
    void Update()
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
         Emitter.Angle,   // current angle
         targetAngle,     // target angle
         turnSpeed * Time.deltaTime  // max change this frame
     );
    }

    void HandleMovement()
    {
        if (Opponent == null) 
            return;

        Vector2 toOpponent = Opponent.transform.position - transform.position;
        float distance = toOpponent.magnitude;
        Vector2 forward = toOpponent.normalized;

        Vector3 movement = Vector3.zero;

        // Maintain preferred aggression distance
        if (distance > Stats.Agression)
            movement += (Vector3)(forward * Stats.Speed);
        else if (distance < Stats.Agression)
            movement -= (Vector3)(forward * Stats.Speed);

        // Strafe logic
        strafeTimer -= Time.deltaTime;

        if (strafeTimer <= 0f)
        {
            strafeDirection = Random.Range(-1f, 1f);
            strafeTimer = Stats.Variablity; // raw seconds
        }

        Vector2 perpendicular = new Vector2(-forward.y, forward.x);

        movement += (Vector3)(
            perpendicular *
            strafeDirection *
            Stats.Caution
        );

        transform.position += movement * Time.deltaTime;
    }


}
