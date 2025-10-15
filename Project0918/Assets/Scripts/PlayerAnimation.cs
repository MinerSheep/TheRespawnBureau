using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Settings")]
    public float Playerscale = 1.5f;
    
    [Header("References")]
    public Animator AT;
    public GameObject PlayerModel;

    // Private variables
    private PlayerController pC;
    private Rigidbody2D rB;
    [HideInInspector] public int direction = 1; //1 means facing right while 0 means facing left

    private void Turn()
    {
        if (pC.AutoRunner)
            return;

        if (rB.linearVelocityX < -0.5f)
        {
            PlayerModel.transform.localScale = new Vector3(Playerscale, Playerscale, 1);
            direction = 0;
        }
        else if (rB.linearVelocityX > 0.5f)
        {
            PlayerModel.transform.localScale = new Vector3(-Playerscale, Playerscale, 1);
            direction = 1;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pC = GetComponent<PlayerController>();
        rB = GetComponent<Rigidbody2D>();

        if (pC.AutoRunner)
        {
            AT.SetBool("AutoRunning", true);

            PlayerEvents.OnFlipFlashlight += FlipDirection;
        }
    }
    
    void FlipDirection(int direction2)
    {
                direction = direction2;
                PlayerModel.transform.localScale = new Vector3(direction == 0 ? Playerscale : -Playerscale, Playerscale, 1);
        
    }

    public void UpdateStatus()
    {
        if(pC.Jumping)
        {
            AT.SetInteger("PlayerState", 3);
        }
        else if(pC.Crouching)
        {
            AT.SetInteger("PlayerState", 2);
        }
        else if (rB.linearVelocityX > 0.5f || rB.linearVelocityX < -0.5f)
        {
            AT.SetInteger("PlayerState", 1);
        }
        else
        {
            AT.SetInteger("PlayerState", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
        UpdateStatus();
    }

    void OnDestroy()
    {
        if (pC.AutoRunner)
        {
            PlayerEvents.OnFlipFlashlight -= FlipDirection;
        }
    }
}
