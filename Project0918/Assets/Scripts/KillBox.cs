using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectionManager.instance.SaveScore(); // Save high score to PlayerPrefs
            SceneManager.LoadScene("PlayerMovementDemo");
        }
    }
}
