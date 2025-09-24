using UnityEngine;

public class Goal : MonoBehaviour
{
    GameObject confetti; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Win condition
            Instantiate(confetti, transform.position, Quaternion.identity).Play();
        }
    }
}
