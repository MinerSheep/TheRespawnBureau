using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool AutoRunner = false;
    public float MoveSpeed = 5f;
    public float MoveForce = 1f;
    public float JumpForce = 5f;
    public bool Jumping = false;
    public bool Crouching = false;
    public float CrouchingTime = 2f;
    public float FallingForce = 3f;

    public Rigidbody2D RB;
    public GroundDetection GD;
    //public PlayerModel PM;

    private float crouchingTimer;

    private AudioSource audioSource;
    private CapsuleCollider2D cC;
    public AudioClip jumpAudio;
    public AudioClip crouchAudio;

    
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

            PlayJumpAudio();
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
                cC.size=new Vector2(1,1);
                PlayCrouchAudio();
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
                cC.size = new Vector2(1, 2);
                //PM.PlayerModelStats = 0;
                //PM.ChangePlayerModelStats();
            }
        }
    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        cC=GetComponent<CapsuleCollider2D>();

        audioSource = gameObject.AddComponent<AudioSource>();

        jumpAudio = Resources.Load<AudioClip>("Audio/jump-retro-game-jam-fx-1-00-03");
        if (jumpAudio == null)
        {
            Debug.LogError("Could not load mp3 from Resources/Audio/jump-retro-game-jam-fx-1-00-03.mp3");
            return;
        }

        crouchAudio = Resources.Load<AudioClip>("Audio/horror-body-drop-152091");
        if (crouchAudio == null)
        {
            Debug.LogError("Could not load mp3 from Resources/Audio/horror-body-drop-152091.mp3");
            return;
        }
    }
    void Update()
    {
        if (!AutoRunner)
        {
            Move();
        }
        Jump();
        Crouch();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("MainMenu_PC");
        }
    }


    //Audio handlers, load and play clips into audioSource
    public void PlayJumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void PlayCrouchAudio()
    {
        audioSource.clip = crouchAudio;
        audioSource.Play();
    }
}
