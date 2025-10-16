using UnityEngine;

public class GroundDetection : MonoBehaviour
{

    [HideInInspector] public bool Grounded;
    [HideInInspector] private int GroundCount = 0;

    [Header("References")]
    public PlayerController PC;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GroundCount++;
            if (GroundCount >= 1)
            {
                Grounded = true;
                if (PC.Jumping)
                {
                    PC.Jumping = false;
                    //PM.PlayerModelStats = 0;
                    //PM.ChangePlayerModelStats();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GroundCount--;
            if (GroundCount < 1)
            {
                Grounded = false;
            }
        }
    }
}
