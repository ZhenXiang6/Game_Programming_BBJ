using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("地面检测")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("动画参数")]
    public Animator anim;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private Vector3 spawnPosition; // Original spawn position
    private BlockBuilder blockBuilder;

    void Start()
    {
        blockBuilder = GetComponent<BlockBuilder>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        spawnPosition = transform.position; // Store the initial spawn position

        if (anim == null)
        {
            Debug.LogError("PlayerController: Animator 组件未找到，请确保 Player 对象上添加了 Animator 组件。");
        }

        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck 对象未分配！");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFalling();
        FlipSprite();
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(move));
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (anim != null)
            {
                anim.SetTrigger("Jump");
            }
            Debug.Log("Jump Triggered");
        }
    }

    void HandleFalling()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (anim != null)
        {
            anim.SetBool("isGrounded", isGrounded);
        }

        if (rb.velocity.y < 0 && !isGrounded)
        {
            if (anim != null)
            {
                anim.SetBool("isFalling", true);
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetBool("isFalling", false);
            }
        }

        if (!wasGrounded && isGrounded)
        {
            if (anim != null)
            {
                anim.SetTrigger("LandingTrigger");
            }
        }
    }

    void FlipSprite()
    {
        float move = Input.GetAxis("Horizontal");

        if (move > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (move < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = spawnPosition;
        rb.velocity = Vector2.zero; // Reset velocity
        Debug.Log("Player respawned to the original position.");

        if (blockBuilder != null)
        {
            blockBuilder.ResetBlocks(); // Reset blocks upon respawn
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
