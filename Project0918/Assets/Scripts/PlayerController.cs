using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
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
    public bool HasStamina = true;
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
    public float StaminaDrainRate = -0.1f;   // Amount removed from stamina per update
    public HeadTrigger HT;


    [HideInInspector] private InputBuffer inputBuffer;
    [HideInInspector] public Rigidbody2D RB;
    [HideInInspector] public CapsuleCollider2D cC;
    [HideInInspector] public bool Jumping = false;
    [HideInInspector] public bool Crouching = false;
    [HideInInspector] public bool Attacking = false; // If this is on, the sword is swinging!

    [Header("References")]
    public GroundDetection GD;
    public HUD hud;
    public FlashLight flashlight;
    public Volume attackVol;  // The "damage zone" used when attacking

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
    [HideInInspector] private float AttackTimer = 0f;   // Counts up while attacking
    [HideInInspector] private float AttackTimerEnd = 0.5f;   // How long should the attack volume/animation be active?

    public void LoseHealth()
    {
        if (iFrames > 0)
            return;

        hud.hp -= 1;
        iFrames = iFrameMax;

        if (hud.hp <= 0)
            PlayerEvents.OnPlayerDeath?.Invoke();
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
            AudioManager.instance.PlaySound("jump");
        }
        else if (Jumping == true)
        {
            JumpHold();
            DoubleJump();
       }
    }
    // Activates a damage volume in front of the player that will deactivate after a set amount of time
    public void Attack()
    {
        // Check if the volume exists/was set correctly
        if(attackVol)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Attacking = true;
                attackVol.enabled = true;
                Debug.Log("Attack!");
            }
            if (Attacking)
            {
                AttackTimer += Time.deltaTime;
                if (AttackTimer > AttackTimerEnd)
                {
                    Debug.Log("Attack Ended");
                    Attacking = false;
                    attackVol.enabled = false;
                    AttackTimer = 0f;
                }
            }
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
            AudioManager.instance.PlaySound("crouch");
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

        hud.AssignLeftButton(inputBuffer, "Jump", true);
        hud.AssignRightButton(inputBuffer, "Crouch", false);

        //if (flashlight == null)
        //    flashlight = transform.Find("FlashLight").GetComponent<FlashLight>();

        PlayerEvents.OnPlayerDeath += PlayerDeath;
        
        attackVol.enabled = false;  // Attack volume is inactive unless attacking
    }
    void Update()
    {
        if (!AutoRunner)
        {
            Move();
        }
        Jump();
        Crouch();
        Dash();
        Attack();
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
        RunnerScene[] scenes = FindObjectsByType<RunnerScene>(FindObjectsSortMode.None);

        foreach (var scene in scenes)
        {
            scene.StartMovingSpeed = scene.EndMovingSpeed = 0;
        }

        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        float time = 0;

        while (time < 1.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // Optional
        ScoreManager.instance?.SaveScore(); // Save high score to PlayerPrefs

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
