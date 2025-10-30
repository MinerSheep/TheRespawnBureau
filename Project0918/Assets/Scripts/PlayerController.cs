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
    public float DoubleJumpForce = 12f;
    public float CrouchingTime = 2f;
    public float FallingForce = 3f;
    public float iFrameMax = 0.2f;
    public float DashSpeed = 8f;
    public float DashTime = 1f;
    public float DashCD = 4f;
    public float Stamina = 1000f; // Stamina is constantly decreasing, player dies if it hits zero
    public float StaminaMax = 1000f;  // Used to reset stamina after death
    public float StaminaDrainRate = 0.1f;   // Amount removed from stamina per update
    public HeadTrigger HT;


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
    [HideInInspector] private bool doublejump = false;
    [HideInInspector] private float DashTimer = 0f;
    [HideInInspector] private float DashCDTimer = 0f;
    [HideInInspector] private bool dashing=false;

    public void LoseHealth()
    {
        if (iFrames > 0)
            return;

        hud.hp -= 1;
        iFrames = iFrameMax;
    }

    public void UpdateStamina()
    {
        Stamina -= StaminaDrainRate;
        hud.stamina = Stamina;

        if (Stamina <= 0.0f)
        {
            // TODO: Remove the LoadScene below once we have PlayerDeath implemented
            PlayerDeath();
            // If the stamina hits zero, restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
        if (Jumping == false && GD.Grounded && inputBuffer.Consume("Jump")&&!HT.IsTriggering)
        {
            RB.linearVelocity = new Vector2(RB.linearVelocity.x, 0);
            RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            //PM.PlayerModelStats = 2;
            //PM.ChangePlayerModelStats();
            Crouching = false;
            Jumping = true;
            cC.size = new Vector2(1, 2);
            firstJump = true;
            doublejump = true;
            JumpTimer = JumpHoldTime;
            AudioManager.instance.Play("jump");
        }
        else if (Jumping == true)
        {
            JumpHold();
            DoubleJump();
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

    public void DoubleJump()
    {
        if (inputBuffer.Consume("Jump") && doublejump && !firstJump)
        {
            RB.linearVelocity = new Vector2(RB.linearVelocity.x, 0);
            RB.AddForce(Vector2.up * DoubleJumpForce, ForceMode2D.Impulse);
            doublejump = false;
            Debug.Log("Doublejump");
        }
    }

    public void JumpHold()
    {
        if (firstJump)
        {
            JumpTimer-=Time.deltaTime;
            if(inputBuffer.Consume("Jump")&&JumpTimer>0)
            {
                RB.AddForce(new Vector2(0,JumpHoldForce));
                Debug.Log("holdjump");
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
            if (crouchingTimer > CrouchingTime&&!HT.IsTriggering)
            {
                crouchingTimer = 0;
                Crouching = false;
                cC.size = new Vector2(1, 2);

                //PM.PlayerModelStats = 0;
                //PM.ChangePlayerModelStats();
            }
        }
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && DashCDTimer <= 0)
        {
            DashCDTimer = DashCD;
            dashing = true;
            DashTimer = DashTime;
        }
        if (dashing)
        {
            DashTimer-= Time.deltaTime;
            RB.AddForce(Vector2.right*DashSpeed);
            if (DashTimer <= 0)
            {
                dashing = false;
            }
        }
        else if(DashCDTimer > 0)
        {
            DashCDTimer -= Time.deltaTime;
            Debug.Log(DashCDTimer);
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

        Stamina = StaminaMax;
    }
    void Update()
    {
        if (!AutoRunner)
        {
            Move();
        }
        else
        {
            UpdateStamina();
        }
        Jump();
        Crouch();
        Dash();
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
