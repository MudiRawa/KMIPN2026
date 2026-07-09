using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Movement")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Underwater")]
    public float swimSpeed = 3f;
    public float waterDrag = 0f;
    public bool isUnderwater = false;
    public Animator animator;

    [Header("Oxygen")]
    public float maxOxygen = 100f;
    public float oxygen = 100f;
    public float oxygenDecreaseRate = 1f;
    public LayerMask waterLayer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    float normalGravityScale;
    bool isGrounded;

    Vector2 moveInput;
    Vector3 originalScale;
    private bool canMove = true;

    public static PlayerMovement instance;
    public bool FacingRight => transform.localScale.x > 0f;

    enum PlayerState { Idle = 0, Walk = 1, Swim = 2 }
    readonly int animStateHash = Animator.StringToHash("State");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalGravityScale = rb.gravityScale;
        originalScale = transform.localScale;
        instance = this;
        LoadOxygenUpgrade();
    }

    void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }

        UpdateOxygen();

        if (isUnderwater)
        {
            Swim();
        }
        else
        {
            GroundMove();
        }

        UpdateFacingDirection();
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        PlayerState state = PlayerState.Idle;

        if (isUnderwater)
        {
            state = PlayerState.Swim;
        }
        else if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            state = PlayerState.Walk;
        }

        animator.SetInteger(animStateHash, (int)state);
    }

    void UpdateOxygen()
    {
        if (isUnderwater)
        {

            oxygen -= oxygenDecreaseRate * Time.deltaTime;
            if (oxygen <= 0f)
            {
                oxygen = 0f;
                Inventory.instance.ClearInventory();
                Mati.instance.mati();
            }
        }

        oxygen = Mathf.Clamp(oxygen, 0f, maxOxygen);
    }


    void Swim()
    {
        rb.gravityScale = 0f;
        rb.linearDamping = waterDrag;

        Vector2 vel = rb.linearVelocity;

        // Horizontal
        vel.x = moveInput.x * swimSpeed;

        // Vertical
        if (moveInput.y != 0)
        {
            float targetY = moveInput.y * swimSpeed;
            vel.y = Mathf.Lerp(vel.y, targetY, Time.deltaTime * 8f);
        }
        else
        {
            vel.y = Mathf.Lerp(vel.y, 0f, Time.deltaTime * 5f);
        }

        rb.linearVelocity = vel;
    }
    void GroundMove()
    {
        rb.gravityScale = normalGravityScale;
        rb.linearDamping = 0f;

        rb.linearVelocity = new Vector2(
            moveInput.x * speed,
            rb.linearVelocity.y
        );
    }

    void UpdateFacingDirection()
    {
        if (moveInput.x > 0.01f)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (moveInput.x < -0.01f)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }

    // =========================
    // NEW INPUT SYSTEM
    // =========================

    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    public void DisableMovement()
    {
        canMove = false;
        moveInput = Vector2.zero;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isUnderwater)
        {
            Jump();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    void Attack()
    {
        if(!isUnderwater) return;
        PlayerShoot shoot = GetComponent<PlayerShoot>();
        if (shoot != null)
        {
            shoot.Shoot();
        }
    }

    // =========================
    // WATER
    // =========================

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            StartCoroutine(EnterWater()); //memasuki air
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isUnderwater = false; //keluar dari air
        }
    }

    IEnumerator EnterWater()
    {
        yield return new WaitForSeconds(0.3f); //effect delay sebelum memasuki mode berenang

        isUnderwater = true;
    }

    void OnDrawGizmosSelected() //mengecek ground atau tanah
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }

    void LoadOxygenUpgrade()
    {
        // Data oxygen tiap level
        float[] oxygenLevels =
        {
            100f,
            120f,
            150f,
            180f,
            250f
        };

        int level = PlayerPrefs.GetInt("OxygenLevel", 0);

        // Biar aman
        level = Mathf.Clamp(level, 0, oxygenLevels.Length - 1);

        maxOxygen = oxygenLevels[level];

        oxygen = maxOxygen;
    }

}