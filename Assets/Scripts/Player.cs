using System;
using System.Collections;
using System.Collections.Generic;
using Map_Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int health = 3;
    [SerializeField] private GameUIManager uiManager;
    [SerializeField] private float repulsiveForce= 10f;
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private bool isGrounded;
    private int keysCollected = 0;
    private Transform platform;
    private Vector3 platformLastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        uiManager.Initialize(health, health);
    }

    void Update()
    {
        Move();
        Jump();
        if (platform != null)
        {
            Vector3 platformMovement = platform.position - platformLastPosition;
            transform.position += platformMovement;
            platformLastPosition = platform.position;
        }
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        sr.flipX = move < 0;

        if (isGrounded)
        {
            animator.SetBool("Run", move != 0);
            animator.SetBool("Idle", move == 0);
        }
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
            animator.SetBool("Idle", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", false);
        }

        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            Vector2 oppositeDirection = -rb.velocity.normalized;
            Vector2 force = oppositeDirection + Vector2.up * repulsiveForce; 
            rb.AddForce(force , ForceMode2D.Impulse); 
            health--;
            uiManager.UpdateHealth(health);
            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
       
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            isGrounded = false;
            animator.SetBool("Fall", true);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Key>() != null)
        {
            keysCollected++;
            uiManager.UpdateKeys(keysCollected);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<MedKit>() != null)
        {
            health++;
            uiManager.UpdateHealth(health);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<Door>() != null)
        {
            Debug.Log("У двери");
            collision.gameObject.GetComponent<Door>().TryOpenDoor(keysCollected);
        }

        if (collision.gameObject.GetComponent<Finish>() != null && keysCollected >= 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        
        }
    }
}
