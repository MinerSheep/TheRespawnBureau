using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player;
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private bool Moving = true;
    private Animator animator;
    private GameController gc;


    void Start()
    {
        col = GetComponent<Collider2D>();
        gc = FindObjectOfType<GameController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.Log("Animator is null");
        }
        if (player == null)
        {
            Debug.Log("Player is null naja");
        }
    }

    void Update()
    {
        if (Moving)
        {
            if (player != null)
            {
                Vector2 direction = new Vector2(player.position.x - transform.position.x, 0);
                direction.Normalize();
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }


        if (!Moving)
        {
            rb.velocity = Vector2.zero;
        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("FlashLight"))
        {
            Moving = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RoomLight"))
        {
            Destroy(col);
            animator.SetBool("lightHit", true);
            gc.Win();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("FlashLight"))
        {
            Moving = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isHitting", true);
            Moving = false;
        }
    }
}
