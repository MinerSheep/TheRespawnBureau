using Unity.VisualScripting;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
    public int scoreValue;
    PlayerController playerScript;
    AudioSource audioSource;

    void Start()
    { 
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //audioSource = GameObject.Find("Audio Manager").GetComponent<AudioSource>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            // Currently this adds points to the Player object by finding the tag "Player"
            // then adds the value of this collectible to the player's score
            CollectionManager.instance.score += scoreValue;
            //playerScript.pointValue += scoreValue;

            // Currently calls a game object called "Audio Manager" and sends a play signal
            //audioSource.Play();

            // Destroys this object
            Destroy(this.gameObject);
        }
    }
}
