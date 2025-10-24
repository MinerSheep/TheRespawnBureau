using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerEvents
{
    public delegate void PlayerIntEvent(int direction);
    public static PlayerIntEvent OnFlipFlashlight;

    // Currently unused
    public delegate void PlayerDefaultEvent();
    public static PlayerDefaultEvent OnPlayerDeath;
    public static PlayerDefaultEvent OnPlayerLightOut;
}

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public bool AutoRunner = false;
    public float MoveSpeed = 5f;
    public float MoveForce = 1f;
    public float JumpForce = 18f;
    private float DefaultJumpForce = 18f;   // Used to reset jump to normal after leaving a "sticky" platform
    public float JumpHoldForce = 3f;
    public float JumpHoldTime = 1f;
    public float CrouchingTime = 2f;
    public float FallingForce = 3f;
    public float iFrameMax = 0.2f;


    [HideInInspector] private InputBuffer inputBuffer;
    [HideInInspector] public Rigidbody2D RB;
    [HideInInspector] public CapsuleCollider2D cC;
    [HideInInspector] public bool Jumping = false;
    [HideInInspector] public bool Crouching = false;

    [Header("References")]
    public GroundDetection GD;
    public HUD hud;
    public FlashLight flashlight;

    // Private Variables
    [HideInInspector] public int pointValue;
    [HideInInspector] private float crouchingTimer;
    [HideInInspector] private float iFrames;
    [HideInInspector] private bool firstJump = false;
    [HideInInspector] private float JumpTimer = 0f;

    public void LoseHealth()
    {
        if (iFrames > 0)
            return;

        hud.hp -= 1;
        iFrames = iFrameMax;
    }

    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal > -0.05 && horizontal <= 0.05)
        {
            RB.linearVelocityX = Mathf.Lerp(RB.linearVelocityX, 0, 0.9f);
        }
        else
        {
            RB.AddForce(Vector2.right * horizontal * MoveForce);
            RB.linearVelocity = new Vector2(Mathf.Clamp(RB.linearVelocityX, -MoveSpeed, MoveSpeed), RB.linearVelocity.y);
        }
    }

    public void Jump()
    {
        if (Jumping == false && GD.Grounded && inputBuffer.Consume("Jump"))
        {
            RB.linearVelocity = new Vector2(RB.linearVelocity.x, 0);
            RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            //PM.PlayerModelStats = 2;
            //PM.ChangePlayerModelStats();
            Crouching = false;
            Jumping = true;
            cC.size = new Vector2(1, 2);
            firstJump = true;
            JumpTimer = JumpHoldTime;
            AudioManager.instance.Play("jump");
        }
        else if (Jumping == true)
        {
            JumpHold();
        }
    }
    // Set/reset functions for modifying JumpForce
    public void SetJumpForce(float NewJumpForce)
    {
        JumpForce = NewJumpForce;
    }
    public void ResetJumpForce()
    {
        JumpForce = DefaultJumpForce;
    }

    public void JumpHold()
    {
        if (firstJump)
        {
            JumpTimer-=Time.deltaTime;
            if(inputBuffer.Consume("Jump")&&JumpTimer>0)
            {
                RB.AddForce(new Vector2(0,JumpHoldForce));
            }
            else
            {
                firstJump = false;
            }
        }
    }

    public void Crouch()
    {
        if (Jumping == false && Crouching == false && inputBuffer.Consume("Crouch"))
        {
            //PM.PlayerModelStats = 1;
            //PM.ChangePlayerModelStats();
            Crouching = true;
            cC.size = new Vector2(1, 1);
            AudioManager.instance.Play("crouch");
        }
        else if (Jumping == true && inputBuffer.Consume("Crouch"))
        {
            RB.AddForce(Vector2.down * FallingForce);
            Debug.Log("SFA");
        }
        if (Crouching)
        {
            crouchingTimer += Time.deltaTime;
            if (crouchingTimer > CrouchingTime)
            {
                crouchingTimer = 0;
                Crouching = false;
                cC.size = new Vector2(1, 2);

                //PM.PlayerModelStats = 0;
                //PM.ChangePlayerModelStats();
            }
        }
    }

    void Start()
    {
        inputBuffer = GetComponent<InputBuffer>();
        RB = GetComponent<Rigidbody2D>();
        cC = GetComponent<CapsuleCollider2D>();

        //if (flashlight == null)
        //    flashlight = transform.Find("FlashLight").GetComponent<FlashLight>();

        PlayerEvents.OnPlayerDeath += PlayerDeath;
    }
    void Update()
    {
        if (!AutoRunner)
        {
            Move();
        }
        Jump();
        Crouch();

        //flipping flashlight by flip the sprite mask
        //if (inputBuffer.Consume("FlipFlashlight"))
        //    flashlight?.flip();

        // iFrame counter
        if (iFrames > 0)
        {
            iFrames -= Time.deltaTime;
        }

        // Return to main menu
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("MainMenu_PC");
        }
    }

    private void PlayerDeath()
    {
        
    }
}
