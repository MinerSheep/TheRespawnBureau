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
        if(PC.Jumping)
        {
            PC.Jumping = !Grounded;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
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
