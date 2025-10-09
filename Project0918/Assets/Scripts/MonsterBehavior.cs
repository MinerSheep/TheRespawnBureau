using UnityEngine;

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
        if (Target != null)
        {
            transform.position = new Vector3(transform.position.x, Target.transform.position.y);

            if(Vector3.Distance(Target.transform.position, transform.position) < distance)
            {
                Vector3 targetPos = Target.transform.position - (Target.transform.position - transform.position).normalized * distance;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }

        }

    }





    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("Triggered with: " + collision.gameObject.name);
    }


}
