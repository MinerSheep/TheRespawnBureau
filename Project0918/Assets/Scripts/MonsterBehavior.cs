using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterBehavior : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] float speed = 0.1f;

    float distance = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!Target)
        {
            Target = GameObject.FindWithTag("Player");
            if(!Target)
            {
                Debug.Log("Best part of the moment: you forget to set the player target");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Different scenes have different monster behavior
        string sceneName = SceneManager.GetActiveScene().name;
        // Platformer scene: Monster chases player through walls, moves constantly on both x and y axis (flies)
        if (sceneName == "Platformer")
        {
            if(Target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);
            }
        }
        // Auto Runner scene: Monster moves right constantly, matches player's Y position exactly
        else if(sceneName == "AutoRunnerTester")
        {
            if (Target != null)
            {
                transform.position = new Vector3(transform.position.x, Target.transform.position.y);

                if (Vector3.Distance(Target.transform.position, transform.position) < distance)
                {
                    Vector3 targetPos = Target.transform.position - (Target.transform.position - transform.position).normalized * distance;
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                }

            }
        }
    }





    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        string objName = collision.gameObject.name;
        Debug.Log("Triggered with: " + objName);
        // If the monster collides with the player, kill the player (reload level)
        if(objName == "Player_Stand")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
