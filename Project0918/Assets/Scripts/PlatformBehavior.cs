using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Behavior
{
    Moving,
    Breaking,
    Launching,
    MoveAndBreak,
    MoveAndLaunch
}

public class PlatformBehavior : MonoBehaviour
{
    [SerializeField] Behavior behavior;   // Determines what kind of behavior the platform will have
    [SerializeField] Transform[] movePoints;    // For moving platforms, these are the points to move between
    [SerializeField] float speed;   // Movement speed
    [SerializeField] float breakTime;   //For platforms that "break", determines how long they take to break
    [SerializeField] float launchStrength;  // Amount of force applied to player
    private int destPoint = 0;  // Index for next movePoint
    private bool moveDir = false;   // Directs the platform to move forward/backward
    private bool touched = false;   // This flag is triggered when the player touches the platform

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Attempt to get first patrol point
        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Move back and forth between movePoints
        if (behavior == Behavior.Moving || behavior == Behavior.MoveAndLaunch || behavior == Behavior.MoveAndBreak)
        {
            // Get the current movePoint
            Transform target = movePoints[destPoint];

            // Move towards the movePoint
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // If the platform is close enough to the point, get the next movePoint
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                GotoNextPoint();
            }
        }
        // Break if the player touches the platform
        if(behavior == Behavior.Breaking || behavior == Behavior.MoveAndBreak)
        {
            // Only decrement the timer once the player touches the platform
            if (touched)
            {
                breakTime -= Time.deltaTime;

                // When the timer runs out the platform is destroyed
                if (breakTime <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    // Attempt to get the next movePoint
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (movePoints.Length == 0)
        {
            return;
        }

        // If the platform has reached the last point (on either end), reverse direction
        if (destPoint + 1 >= movePoints.Length && moveDir)
        {
            moveDir = false;
        }
        else if (destPoint - 1 <= 0 && !moveDir)
        {
            moveDir = true;
        }
        // Increment/Decrement patrol point index accordingly
        if (moveDir)
        {
            ++destPoint;
        }
        else if (!moveDir)
        {
            --destPoint;
        }
    }

    // Collision detection
    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        GameObject player = collision.gameObject; // Store whatever thing hit the platform
        string objName = collision.gameObject.name;

        // If the player collides with the platform, set the "touched" flag
        if (player != null && objName == "GroundTrigger")
        {
            touched = true;
            if(behavior == Behavior.Launching || behavior == Behavior.MoveAndLaunch)
            {
                float platformTop = GetComponent<Collider2D>().bounds.max.y;    // Top of the platform
                float playerBot = collision.bounds.min.y; // Bottom of the player's collider

                // If the player is hitting the top of the platform, launch 'em
                if (Mathf.Abs(playerBot - platformTop) < 0.2f)
                {
                    collision.attachedRigidbody.AddForce(Vector2.up * launchStrength, ForceMode2D.Impulse);
                }
            }
        }
    }
}


