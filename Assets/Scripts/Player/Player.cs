using Map_Elements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float repulsiveForce = 10f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer; // Маска для слоя земли
    [SerializeField] private int health = 3;
    public event System.Action<int> OnHealthChanged;
    public event System.Action<int> OnKeysChanged;
    public event System.Action OnPlayerDeath;

    private bool isGrounded;
    private bool facingRight = true;
    private int keysCollected = 0;

    public int Health { get { return health; } } // Публичное свойство Health с публичным геттером и приватным сеттером
    public int KeysCollected { get { return keysCollected; } }

    // Хеши для параметров анимации
    private readonly int runHash = Animator.StringToHash("Run");
    private readonly int idleHash = Animator.StringToHash("Idle");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int fallHash = Animator.StringToHash("Fall");

    private IPlayerInput _input; // Интерфейсный тип для ввода

    private void Start()
    {
        _input = new PlayerInput();
    }

    private void Update()
    {
        _input.CustomUpdate();
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
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        if (_input.IsJump && isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        float move = _input.Horizontal;
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

    private void Jump()
    {
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        sr.flipX = !sr.flipX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка на слой земли
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
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
            ChangeHealth(-1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Проверка на слой земли
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Key>() != null)
        {
            ChangeKeys(1);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<MedKit>() != null)
        {
            ChangeHealth(1);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<Door>() != null)
        {
            if (keysCollected >= 5)
            {
                collision.gameObject.GetComponent<Door>().TryOpenDoor(keysCollected);
            }
        }

        if (collision.gameObject.GetComponent<Finish>() != null && keysCollected >= 5)
        {
            // Вызываем событие о смерти игрока
            OnPlayerDeath?.Invoke();
        }
    }

    private void ChangeHealth(int amount)
    {
        health += amount;
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            // Вызываем событие о смерти игрока
            OnPlayerDeath?.Invoke();
        }
    }

    private void ChangeKeys(int amount)
    {
        keysCollected += amount;
        OnKeysChanged?.Invoke(keysCollected);
    }
}
