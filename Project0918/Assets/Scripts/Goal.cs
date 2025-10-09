using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject confetti;

    bool finished = false;

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
        if (other.CompareTag("Player") && !finished)
        {
            // Win condition
            finished = true;

            if (CollectionManager.instance != null)
            {
                CollectionManager.instance.SaveScore();
            }

            for (int i = 0; i < 100; i++)
                Instantiate(confetti, transform.position, Quaternion.identity);
        }
    }
}
