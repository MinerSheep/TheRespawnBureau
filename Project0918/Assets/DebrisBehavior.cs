using UnityEngine;

public class DebrisBehavior : MonoBehaviour
{
    [Header("References")]
    //public Animator animator; // Uncomment this when we have an animation for debris!
    private BoxCollider2D debrisCollider;

    [Header("Animation Settings")]
    //public string destroyTriggerName = "Destroy"; // Name of trigger in Animator

    private bool isTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        debrisCollider = GetComponent<BoxCollider2D>();
        //animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only trigger once
        if (isTriggered)
        {
            return;
        }

        // Disable collision so the player can pass through
        if (collision.collider.CompareTag("AttackVolume"))
        {
            debrisCollider.enabled = false;
        }

        //if (animator != null)
        //{
        //    animator.SetTrigger(destroyTriggerName);
        //}
    }

    private void StartDestructionSequence()
    {
        isTriggered = true;

        // Disable collision so the player can pass through
        if (debrisCollider != null)
        {
            debrisCollider.enabled = false;
        }

        //// Start the animation
        //if (animator != null)
        //{
        //    animator.SetTrigger(destroyTriggerName);
        //}
        //else
        //{
        //    Debug.LogWarning("DebrisBehavior: Animator not found!");
        //}

        // Use an Animation Event to call DestroySelf() at the end
        DestroySelf();
    }

    // This can be called by an Animation Event at the end of your animation
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
