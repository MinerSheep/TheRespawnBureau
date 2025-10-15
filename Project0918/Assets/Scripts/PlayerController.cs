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
    public float JumpForce = 5f;
    public float CrouchingTime = 2f;
    public float FallingForce = 3f;
    public float iFrameMax = 0.2f;

    [HideInInspector] public Rigidbody2D RB;
    [HideInInspector] public CapsuleCollider2D cC;
    [HideInInspector] public bool Jumping = false;
    [HideInInspector] public bool Crouching = false;

    [Header("References")]
    public GroundDetection GD;
    public HealthFill health;
    public FlashLight flashlight;

    // Private Variables
    [HideInInspector] public int pointValue;
    [HideInInspector] private float crouchingTimer;
    [HideInInspector] private float iFrames;

    public void LoseHealth()
    {
        if (iFrames > 0)
            return;

        health.LoseHealth();
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
        if (Input.GetKeyDown(KeyCode.Space) && GD.Grounded)
        {
            RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            //PM.PlayerModelStats = 2;
            //PM.ChangePlayerModelStats();
            Crouching = false;
            Jumping = true;
            cC.size = new Vector2(1, 2);

            AudioManager.instance.Play("jump");
        }
    }

    public void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.S) && Jumping == false)
        {
            if (!Crouching)
            {
                //PM.PlayerModelStats = 1;
                //PM.ChangePlayerModelStats();
                Crouching = true;
                cC.size = new Vector2(1, 1);
                AudioManager.instance.Play("crouch");
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) && Jumping == true)
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
                if (!Input.GetKeyDown(KeyCode.S))
                {
                    Crouching = false;
                    cC.size = new Vector2(1, 2);
                }

                //PM.PlayerModelStats = 0;
                //PM.ChangePlayerModelStats();
            }
        }
    }

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        cC = GetComponent<CapsuleCollider2D>();

        if (flashlight == null)
            flashlight = transform.Find("FlashLight").GetComponent<FlashLight>();
    }
    void Update()
    {
        if (!AutoRunner)
        {
            Move();
        }
        Jump();
        Crouch();

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
}
