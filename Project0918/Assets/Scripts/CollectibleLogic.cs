using Unity.VisualScripting;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
    public int scoreValue;
    PlayerController2 playerScript;

    void Start()
    {
        playerScript = GameObject.Find("Player_Controller").GetComponent<PlayerController2>();
        if (!playerScript)
        {
            Debug.Log("Script null!");
        }

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (!playerObject)
        {
            Debug.Log("Player not found!");
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
