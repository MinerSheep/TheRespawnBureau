using UnityEngine;

public class MonsterBehaviourAutoRunner : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [HideInInspector] float buildUpSpeed = 0.5f;  // Percent of speed build up (0 -> 1) after being flashed
    [HideInInspector] public float currentSpeed;
    [HideInInspector] float distance = 10f;
    [SerializeField] float speed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
            return;

            transform.position = new Vector3(transform.position.x, Target.transform.position.y);

        if (Vector3.Distance(Target.transform.position, transform.position) < distance)
        {
            Vector3 targetPos = Target.transform.position - (Target.transform.position - transform.position).normalized * distance;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }
}
