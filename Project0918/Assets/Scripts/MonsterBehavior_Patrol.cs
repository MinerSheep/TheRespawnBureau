using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum PatrolBehavior
{
    Loop,
    BackAndForth,
    Random
}

public class MonsterBehavior_Patrol : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;              // List of points that the monster moves between
    [SerializeField] private PatrolBehavior patrolBehavior; // Determines the "style" of movement between patrol points
    [SerializeField] float speed = 1.5f;                    // Movement speed
    private int destPoint = 0;                              // Index for next patrol point
    private bool patrolDir = true;                          // Determines whether the monster is going forward/backward through patrol points

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Attempt to get first patrol point
        GotoNextPoint();
    }
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (patrolPoints.Length == 0)
        {
            return;
        }

        // BackAndForth makes the monster reverse direction when hitting the last/first patrol point
        if(patrolBehavior == PatrolBehavior.BackAndForth)
        {
            // If the monster has reached the last point (on either end), reverse direction
            if (destPoint + 1 >= patrolPoints.Length && patrolDir)
            {
                patrolDir = false;
            }
            else if (destPoint - 1 <= 0 && !patrolDir)
            {
                patrolDir = true;
            }
            // Increment/Decrement patrol point index accordingly
            if (patrolDir)
            {
                ++destPoint;
            }
            else if (!patrolDir)
            {
                --destPoint;
            }
        }
        // Loop treats all patrol points as a continuous route (last connects to first)
        else if(patrolBehavior == PatrolBehavior.Loop)
        {
            // Choose the next point in the array as the destination, wrap around if necessary
            destPoint = (destPoint + 1) % patrolPoints.Length;
        }
        // Random is... random, monster picks random points from the list to move to
        else if(patrolBehavior == PatrolBehavior.Random)
        {
            destPoint = Random.Range(0, patrolPoints.Length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Returns if no points have been set up
        if (patrolPoints.Length == 0)
        {
            return;
        }

        // Get the current patrol point
        Transform target = patrolPoints[destPoint];

        // Move towards the patrol point
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // If the monster is close enough to the point, get the next patrol point
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            GotoNextPoint();
        }
    }





    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        string objName = collision.gameObject.name;
        Debug.Log("Triggered with: " + objName);
        // If the monster collides with the player, kill the player (reload level)
        if (objName == "Player_Stand" || objName == "Player_Jump" || objName == "Player_Crouch")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
