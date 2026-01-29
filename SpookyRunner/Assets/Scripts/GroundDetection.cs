using UnityEngine;
using System;

public class GroundDetection : MonoBehaviour
{

    [HideInInspector] public bool Grounded;
    [HideInInspector] private int GroundCount = 0;

    [Header("References")]
    public PlayerController PC;

    void Update()
    {

        if (Grounded && GroundCount == 0)
        {
            CameraEvents.TriggerGrounded(false);
            ParticleManager.instance.RunningEffectCall(transform.position);
            Grounded = false;
        }
        else if (!Grounded && GroundCount > 0)
        {
            CameraEvents.TriggerGrounded(true);
            ParticleManager.instance.RunningEffectDestory();
            Grounded = true;
        }

        if (PC.Jumping)
        {
            PC.Jumping = !Grounded;

            if (!PC.Jumping)
            {
                PC.RB.linearVelocityY = 0f;
            }
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
