using UnityEngine;

public class CreditsMan : MonoBehaviour
{
    [Header("Settings")]
    public float Playerscale = 1.5f;
    public bool autorun = true;
    
    [Header("References")]
    public Animator AT;
    public GameObject PlayerModel;

    // Private variables
    [HideInInspector] private float actionTimer = 0f;   // Counts down while performing

    private bool Jumping = false;
    private bool Crouching = false;
    private bool Attacking = false;
    private float timer = 5;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AT.SetInteger("PlayerState", 0);
    }

    void SimJump()
    {
            Crouching = false;
            Jumping = true;

            actionTimer = 1.5f;

            AudioManager.instance.PlaySound("jump");
    }

    void SimCrouch()
    {
        Crouching = true;

        actionTimer = 1.5f;

            AudioManager.instance.PlaySound("crouch");
        
    }

    void SimAttack()
    {
        Attacking = true;

        actionTimer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf)
            return;
            
        int result = AT.GetInteger("PlayerState");

        if(Jumping)
        {
            AT.SetInteger("PlayerState", 3);
        }
        else if(Crouching)
        {
            AT.SetInteger("PlayerState", 2);
        }
        else if (Attacking)
        {
            AT.SetInteger("PlayerState", 4);
        }
        else if (autorun)
        {
            AT.SetInteger("PlayerState", autorun ? 1 : 0);
        }

        // a timer runs, if no action is performed, a random action will be performed
        if (autorun)
            timer -= Time.deltaTime;
        if (timer < 0)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    SimJump();
                    break;
                case 1:
                    SimCrouch();
                    break;
                case 2:
                    SimAttack();
                    break;
            }
            timer = 5;
        }


        if (Jumping || Crouching || Attacking)
            {
                actionTimer -= Time.deltaTime;
                if (actionTimer < 0.0f)
                {
                    Jumping = false;
                    Crouching = false;
                    Attacking = false;
                    AT.SetInteger("PlayerState", 0);
                    actionTimer = 0f;
                }
            }
    }
}
