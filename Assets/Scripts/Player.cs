using Map_Elements;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int health = 3;
    [SerializeField] private GameUIManager uiManager;
    [SerializeField] private float repulsiveForce = 10f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;

    private bool isGrounded;
    private int keysCollected = 0;
    private Transform platform;
    private Vector3 platformLastPosition;
    private bool facingRight = true;

    // Хеши для параметров анимации
    private readonly int runHash = Animator.StringToHash("Run");
    private readonly int idleHash = Animator.StringToHash("Idle");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int fallHash = Animator.StringToHash("Fall");

    void Start()
    {
        uiManager.Initialize(health, health);
    }

    void Update()
    {
        HandleInput();
        
        // Проверка на падение
        if (rb.velocity.y <= 0f && !isGrounded)
        {
            animator.SetBool(fallHash, true);
            animator.SetBool(jumpHash, false);
            animator.SetBool(runHash, false);
            animator.SetBool(idleHash, false);
        }
        // Проверка на прыжок
        if (rb.velocity.y > 0f && !isGrounded)
        {
            animator.SetBool(fallHash, false);
            animator.SetBool(jumpHash, true);
            animator.SetBool(runHash, false);
            animator.SetBool(idleHash, false);
        }

        if (platform != null)
        {
            Vector3 platformMovement = platform.position - platformLastPosition;
            transform.position += platformMovement;
            platformLastPosition = platform.position;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        if (move != 0)
        {
            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }

        if (isGrounded)
        {
            animator.SetBool(runHash, move != 0);
            animator.SetBool(idleHash, move == 0);
        }
    }

    void Jump()
    {
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);       
    }

    private void Flip()
    {
        facingRight = !facingRight;
        sr.flipX = !sr.flipX;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            isGrounded = true;
            animator.SetBool(jumpHash, false);
            animator.SetBool(fallHash, false);
        }

        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            Vector2 oppositeDirection = -rb.velocity.normalized;
            Vector2 force = oppositeDirection + Vector2.up * repulsiveForce;
            rb.AddForce(force, ForceMode2D.Impulse);
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
            collision.gameObject.GetComponent<Door>().TryOpenDoor(keysCollected);
        }

        if (collision.gameObject.GetComponent<Finish>() != null && keysCollected >= 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
