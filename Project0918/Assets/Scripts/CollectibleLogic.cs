using Unity.VisualScripting;
using UnityEngine;

public enum CollectibleType
{
    Coin,
    Battery,
    Stamina
}

public class CollectibleLogic : MonoBehaviour
{
    [Header("Settings")]
    public int value = 100; // Default value to 100
    public CollectibleType type = CollectibleType.Coin; // Default to coin

    // Private variables
    MovementDemoController playerScript;
    PlayerController playerController;

    void Start()
    {
        playerScript = GameObject.FindWithTag("Player")?.GetComponent<MovementDemoController>();

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
            if (type == CollectibleType.Coin && value > 0)
            {
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.score += value;
                    HUDEvents.OnCollectCoin?.Invoke();
                }

                if (playerController != null)
                {
                    playerController.pointValue += value;
                }

                // Currently calls a game object called "Audio Manager" and sends a play signal
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySound("coin_collect");
                }

            }

            // Give battery if battery value
            if (type == CollectibleType.Battery && value > 0)
            {
                playerController.flashlight.BatteryChange(value);
            }

            // Add stamina
            if(type == CollectibleType.Stamina && value > 0)
            {
                playerController.UpdateStamina(value);
            }

            // Destroys this object
            Destroy(this.gameObject);
        }
    }
}
