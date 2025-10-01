using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerController2 : MonoBehaviour
{
    public bool AutoRunner=false;
    public float MoveSpeed = 5f;
    public float MoveForce=1f;
    public float JumpForce = 5f;
    public bool Jumping = false;
    public bool Crouching=false;
    public float CrouchingTime = 2f;
    public float FallingForce = 3f;

    public Rigidbody2D RB;
    public GroundDetection GD;
    public PlayerModel PM;

    private float crouchingTimer;

    private AudioSource jump;
    private AudioSource crouch;


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
            RB.linearVelocity = new Vector2(Mathf.Clamp(RB.linearVelocity.x, -MoveSpeed, MoveSpeed), RB.linearVelocity.y);
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GD.Grounded)
        {
            RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            PM.PlayerModelStats = 2;
            PM.ChangePlayerModelStats();
            Crouching = false;
            Jumping = true;
            
            jump.Play();
        }
    }

    public void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.S) && Jumping == false)
        {
            if (!Crouching)
            {
                PM.PlayerModelStats = 1;
                PM.ChangePlayerModelStats();
                Crouching = true;

                crouch.Play();
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
                Crouching = false;
                PM.PlayerModelStats = 0;
                PM.ChangePlayerModelStats();
            }
        }
    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();

        jump = gameObject.AddComponent<AudioSource>();

        AudioClip clip = Resources.Load<AudioClip>("Audio/jump-retro-game-jam-fx-1-00-03");
        if (clip == null)
        {
            Debug.LogError("Could not load mp3 from Resources/Audio/jump-retro-game-jam-fx-1-00-03.mp3");
            return;
        }
        jump.clip = clip;

        crouch = gameObject.AddComponent<AudioSource>();
        
        clip = Resources.Load<AudioClip>("Audio/horror-body-drop-152091");
        if (clip == null)
        {
            Debug.LogError("Could not load mp3 from Resources/Audio/horror-body-drop-152091.mp3");
            return;
        }
        crouch.clip = clip;
    }
    void Update()
    {
        if (!AutoRunner)
        {
            Move();
        }
        Jump();
        Crouch();
    }
}
