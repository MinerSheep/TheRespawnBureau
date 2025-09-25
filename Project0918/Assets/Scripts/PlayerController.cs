using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Modifiable player physics
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpMaxTime;

    //Player variables
    float moveDirection;
    float jumpTimer;
    bool isJumping;
    public int pointValue;
    Vector2 moveVelocity = new Vector2(0,0);
    [SerializeField] bool isGrounded;

    //Component Call
    Rigidbody2D rb;


    private void Start()
    {
        //Component Call
        rb = GetComponent<Rigidbody2D>();
    }

    //
    void Update()
    {


        //Horizontal inputs
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            moveDirection = 0;
        }

        //Vertical Input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            jumpTimer += .05f;
            isGrounded = false;
            isJumping = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space) || jumpTimer >= jumpMaxTime)
        {
            jumpTimer = jumpMaxTime;
            isJumping = false;
        }

        if (isJumping && Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTimer += .2f;
        }
            
        //define moveVelocity
        moveVelocity.x = moveDirection * moveSpeed;


        rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y) ;
    }

    //Ground detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") == true)
        {
            isGrounded = true;
            jumpTimer = 0;
        }
    }
}
