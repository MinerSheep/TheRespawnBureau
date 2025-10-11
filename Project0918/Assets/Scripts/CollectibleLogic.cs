using Unity.VisualScripting;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
    public int scoreValue;
    MovementDemoController playerScript;
    PlayerController playerController;

    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<MovementDemoController>();

        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        if (!playerController)
        {
            Debug.Log("Player Controller null!");
        }
        
        //audioSource = GameObject.Find("Audio Manager").GetComponent<AudioSource>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            // Currently this adds points to the Player object by finding the tag "Player"
            // then adds the value of this collectible to the player's score

            if (CollectionManager.instance != null)
            {
                CollectionManager.instance.score += scoreValue;
            }

            if (playerController != null)
            {
                playerController.pointValue += scoreValue;
                //playerScript.pointValue += scoreValue;
            }

            // Currently calls a game object called "Audio Manager" and sends a play signal
            if (AudioManager.instance != null)
            {
                AudioManager.instance.Play("coin_collect");
            }

            // Destroys this object
            Destroy(this.gameObject);
        }
    }
}
