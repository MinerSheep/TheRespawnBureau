using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject confetti;

    bool finished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    float transitionTimer = 3.0f;
    // Update is called once per frame
    void Update()
    {
        if (finished)
        {
            transitionTimer -= Time.deltaTime;

            if (transitionTimer < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
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
