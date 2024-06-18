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
    // [SerializeField] private Text healthText;
    // [SerializeField] private Text keysText;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private int keysCollected = 0;
    private Transform platform;
    private Vector3 platformLastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        // UpdateUI();
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
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            isGrounded = true;
        }

        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            health--;
            // UpdateUI();
            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (collision.gameObject.GetComponent<MovingPlatform>() != null)
        {
            platform = collision.transform;
            platformLastPosition = platform.position;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            isGrounded = false;
        }

        if (collision.gameObject.GetComponent<MovingPlatform>() != null)
        {
            platform = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Key>() != null)
        {
            keysCollected++;
            // UpdateUI();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<MedKit>() != null)
        {
            health++;
            // UpdateUI();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<Finish>() != null && keysCollected >= 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Можно заменить на переход на следующий уровень
        }
    }

    /* void UpdateUI()
    {
        healthText.text = "Health: " + health;
        keysText.text = "Keys: " + keysCollected + "/5";
    }*/
}
    