using Unity.VisualScripting;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
    public int scoreValue;
    PlayerController playerScript;

    void Start()
    { 
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (!playerScript)
        {
            Debug.Log("Script null!");
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            CollectionManager.instance.score += scoreValue;
            Debug.Log("SCORE: " + CollectionManager.instance.score);
            //playerScript.pointValue += scoreValue;
            Destroy(this.gameObject);
        }
    }
}
