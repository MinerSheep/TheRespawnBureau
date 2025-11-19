using UnityEngine;

public class GroundDetection : MonoBehaviour
{

    [HideInInspector] public bool Grounded;
    [HideInInspector] private int GroundCount = 0;

    [Header("References")]
    public PlayerController PC;

    void Update()
    {
        Grounded = GroundCount > 0;

        if (Grounded)
            ParticleManager.instance.RunningEffectCall(transform.position);
        else
            ParticleManager.instance.RunningEffectDestory();
          
        if(PC.Jumping)
        {
            PC.Jumping = !Grounded;

            //if (!PC.Jumping)
            //    AudioManager.instance.PlaySound("ground_landing");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Collide with " + collision.name);
            GroundCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GroundCount--;
        }
    }
}
