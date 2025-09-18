using PrimaF;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using TMPro;
//using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveSpeedCrouch = 3f;
    [SerializeField] private Vector2 crouchHeight;
    [SerializeField] private Vector2 crouchOffset;

    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpIgnoreGroundTime = 0.2f;
    [SerializeField] private float jumpCoyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferPressedTime;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private float lastJumpTime;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isLeaveGround;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int jumpAmount = 1;
    [SerializeField] private int currentJumpAmount;
    private float jumpHeight;
    private bool isGrounded;

    private bool isCrouch;
    private bool isDead;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private Camera cam;

    [SerializeField] private int healthPoint = 3;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioClip _sfxJump;
    [SerializeField] private AudioClip _sfxWin;
    [SerializeField] private AudioClip _sfxHurt;
    [SerializeField] private AudioClip _sfxLose;
    [SerializeField] private float _volumn;
    [SerializeField] private TextMeshProUGUI healthText;

    private float normalHeight;
    private Vector2 normHeight;
    private Vector2 normOffset;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameController gc;
    private Animator animator;
    private CapsuleCollider2D col;
    [SerializeField] private SoundManager soundManager;

    private float currentSpeed;

    void Start()
    {
        UpdateHealthUI();
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;

        gc = FindObjectOfType<GameController>();
        soundManager = FindObjectOfType<SoundManager>();
        animator = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        jumpHeight = (jumpForce * jumpForce) / (9.8f * 2f);

        normalHeight = player.transform.localScale.y;
        isDead = false;
        isCrouch = false;
        normHeight = col.size;
        normOffset = col.offset;
    }

    void Update()
    {
       if(!isDead)
        {
            // Walking
            float horizontalInput = 0;
            if (Input.GetKey(PlayerSetting.playerMoveLeftKeyControl))
            {
                horizontalInput = -1;
            }
            else if (Input.GetKey(PlayerSetting.playerMoveRightKeyControl))
            {
                horizontalInput = 1;
            }
            else if(!Input.GetKey(PlayerSetting.playerMoveLeftKeyControl) && !Input.GetKey(PlayerSetting.playerMoveRightKeyControl))
            {
                horizontalInput = 0;
            }
            Vector2 movement = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);
            rb.velocity = movement;

            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            animator.SetFloat("YSpeed", rb.velocity.y);
            animator.SetBool("isCrouch", isCrouch);



            RaycastHit2D castHit = Physics2D.Raycast(transform.position, -Vector2.up, groundDistance, groundLayer);
            isGrounded = castHit.collider != null;

            RaycastHit2D castHitCoyote = Physics2D.Raycast(transform.position, -Vector2.up, jumpHeight + 0.2f, groundLayer);
            isLeaveGround = castHitCoyote.collider == null;

            //Jump & DoubleJump
            {
                if (isGrounded && (Input.GetKeyDown(PlayerSetting.playerJumpKeyControl)))
                {
                    Jump();
                }
                //CoyoteJump
                if (isLeaveGround && (Input.GetKeyDown(PlayerSetting.playerJumpKeyControl)) && jumpTime < jumpCoyoteTime && currentJumpAmount > 0)
                {
                    Jump();
                }

                if (!isGrounded && (Input.GetKeyDown(PlayerSetting.playerJumpKeyControl)) && currentJumpAmount > 0 && jumpTime > jumpIgnoreGroundTime)
                {
                    Jump();
                }

                //JumpBuffer
                if (!isGrounded && (Input.GetKeyDown(PlayerSetting.playerJumpKeyControl)) && currentJumpAmount == 0)
                {
                    jumpBufferPressedTime = jumpTime;
                }

                if (!isGrounded && Input.GetKeyDown(PlayerSetting.playerDownKeyControl))
                {
                    rb.gravityScale = 10;
                }
                else
                {
                    rb.gravityScale = 1;
                }
            }

            if (isJumping)
            {
                jumpTime += Time.deltaTime;
            }


            //Turning left/right
            if (Input.GetKeyDown(PlayerSetting.playerFlipKeyControl)) 
            {
                transform.localScale = new Vector3(player.transform.localScale.x * -1, player.transform.localScale.y, player.transform.localScale.z);
            }

            //Crouch
            if (Input.GetKeyUp(PlayerSetting.playerCrouchKeyControl))
            {
                col.size = normHeight;
                col.offset = normOffset;
                isCrouch = false;
                currentSpeed = moveSpeed;
            
            }
            if (Input.GetKeyDown(PlayerSetting.playerCrouchKeyControl))
            {
                col.size = crouchHeight;
                col.offset = crouchOffset;
                isCrouch = true;
                currentSpeed = moveSpeedCrouch;
            }
        }
    }

    void Jump()
    {
        soundManager.PlayBGM(_sfxJump, transform.position, GlobalDataStatic.volume);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        --currentJumpAmount;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            healthPoint -= 1;
            StartCoroutine(Hurt());
            soundManager.PlayBGM(_sfxHurt, transform.position, GlobalDataStatic.volume);

            UpdateHealthUI();

            if (healthPoint > 0)
            {
                soundManager.PlayBGM(_sfxHurt, transform.position, GlobalDataStatic.volume);
            }
            else
            {
                Lose();
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentSpeed = 0f;
            healthPoint = 0;
            UpdateHealthUI();
            transform.position = new Vector3(transform.position.x, -3, transform.position.z);
            soundManager.PlayBGM(_sfxLose, transform.position, GlobalDataStatic.volume);
            Lose();
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            currentSpeed = 0f;
            soundManager.PlayBGM(_sfxWin, transform.position, GlobalDataStatic.volume);
            gc.Win();
        }
    }


    private void Lose()
    {
        flashLight.SetActive(false);
        gc.Lose();
        isDead = true;
        animator.SetBool("isDead", true);
    }

    private void UpdateHealthUI()
    {
        healthText.text = ": " + healthPoint.ToString();
    }

    IEnumerator Hurt()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }
}



