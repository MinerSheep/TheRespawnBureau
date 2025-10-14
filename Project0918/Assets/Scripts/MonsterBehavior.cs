using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MonsterBehavior : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] float speed = 0.1f;
    public float currentSpeed;
    public float flashedSpeed = 0f;
    public bool isflashed = false;
    public MonsterState state;

    float distance = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        currentSpeed = speed;
        if (!Target)
        {
            Target = GameObject.FindWithTag("Player");
            if (!Target)
            {
                Debug.Log("Best part of the moment: you forget to set the player target");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isflashed)
        {
            currentSpeed = flashedSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
        
        // Different scenes have different monster behavior

        switch (state)
        {
            // Platformer scene: Monster chases player through walls, moves constantly on both x and y axis (flies)
            case MonsterState.Platformer:
                if(Target != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, currentSpeed * Time.deltaTime);
                }
                break;
            // Auto Runner scene: Monster moves right constantly, matches player's Y position exactly
            case MonsterState.AutoRunner:
                if (Target != null)
                {
                    transform.position = new Vector3(transform.position.x, Target.transform.position.y);

                    if (Vector3.Distance(Target.transform.position, transform.position) < distance)
                    {
                        Vector3 targetPos = Target.transform.position - (Target.transform.position - transform.position).normalized * distance;
                        transform.position = Vector3.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);
                    }

                }
                break;
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

        if (objName == "LightBlock")
        {
            isflashed = true;
            Debug.Log("Flashed");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string objName = collision.gameObject.name;

        if (objName == "LightBlock")
        {
            isflashed = false;
            Debug.Log("Not flashed");
        }
    }

    public enum MonsterState
    {
        Platformer,
        AutoRunner
    };
}
