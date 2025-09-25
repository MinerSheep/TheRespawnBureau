using Unity.VisualScripting;
using UnityEditor.Build;
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
            playerScript.pointValue += scoreValue;
            Destroy(this.gameObject);
        }
    }
}
