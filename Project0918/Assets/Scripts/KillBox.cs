using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
    public GameObject followTarget;

    void Update()
    {
        // Match the x
        if (followTarget != null)
            transform.position = new Vector2(followTarget.transform.position.x, transform.position.y);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectionManager.instance.SaveScore(); // Save high score to PlayerPrefs
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
