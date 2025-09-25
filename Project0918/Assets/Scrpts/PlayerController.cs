using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    public bool AutoRunner=false;
    public float MoveSpeed = 5f;
    public float MoveForce=1f;
    public float JumpForce = 5f;
    public bool Jumping = false;
    public bool Crouching=false;
    public float CrouchingTime = 2f;

    public Rigidbody2D RB;
    public GroundDetection GD;
    public PlayerModel PM;

    private float crouchingTimer;

    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal > -0.05 && horizontal <= 0.05)
        {
            RB.linearVelocityX =Mathf.Lerp(RB.linearVelocityX, 0, 0.9f);
        }
        else
        {
            RB.AddForce(Vector2.right * horizontal * MoveForce);
            RB.linearVelocity = new Vector2(Mathf.Clamp(RB.linearVelocity.x, -MoveSpeed, MoveSpeed),RB.linearVelocity.y);
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&GD.Grounded)
        {
            RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            PM.PlayerModelStats = 2;
            PM.ChangePlayerModelStats();
            Crouching = false;
            Jumping = true;
        }
    }

    public void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.S)&&Jumping==false)
        {
            if (!Crouching)
            {
                PM.PlayerModelStats = 1;
                PM.ChangePlayerModelStats();
                Crouching = true;
            }
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
        RB= GetComponent<Rigidbody2D>();
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
