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

    [Header("冲刺参数")]
    public float dashSpeed = 20f;        // 冲刺速度
    public float dashDuration = 0.2f;    // 冲刺持续时间
    public float dashCooldown = 1f;      // 冲刺冷却时间

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    public bool PlayerCanMove = true;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;        // 原始缩放
    private Vector3 spawnPosition;        // 原始生成位置
    private BlockBuilder blockBuilder;

    // Dash 状态变量
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime = -100f;   // 初始化为足够小的值

    [Header("Audio Source")]
    public AudioSource jumpFX;
    public AudioSource deathFX;
    public AudioSource dashFX;

    public DiedMenu diedMenu;

    void Start()
    {
        blockBuilder = GetComponent<BlockBuilder>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        spawnPosition = transform.position; // 存储初始生成位置

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
        HandleDash();
        FlipSprite();

        // 按下 R 鍵重新回到起點
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    void HandleMovement()
    {
        if (isDashing) return; // Dash 期间禁用常规移动

        float move = Input.GetAxis("Horizontal");
        if (!PlayerCanMove) move = 0;
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(move));
        }
    }

    void HandleJump()
    {
        // 按下跳躍鍵立即起跳
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && isGrounded && PlayerCanMove && !isDashing)
        {
            jumpFX.Play(0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (anim != null)
            {
                anim.SetTrigger("Jump");
            }
            Debug.Log("Jump Triggered");
        }

        // 放開跳躍鍵時，如仍在上升，立刻結束上升，開始下落
        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W)) && rb.velocity.y > 0)
        {
            // 削減角色的向上速度，讓角色立即往下落
            rb.velocity = new Vector2(rb.velocity.x, 0f);
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
                anim.ResetTrigger("Jump");
                anim.SetTrigger("LandingTrigger");
            }
        }
    }

    void HandleDash()
    {
        // 检查是否可以开始 Dash（冷却时间已过）
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && PlayerCanMove)
        {
            StartDash();
        }

        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                dashTimeLeft -= Time.deltaTime;
                // 保持 Dash 方向
                float dashDirection = spriteRenderer.flipX ? -1f : 1f;
                rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);
            }
            else
            {
                StopDash();
            }
        }
    }

    void StartDash()
    {
        dashFX.Play();
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        if (anim != null)
        {
            anim.SetTrigger("Dash"); // 触发 Dash 动画
        }

        Debug.Log("Dash Started");
    }

    void StopDash()
    {
        isDashing = false;
        rb.velocity = new Vector2(0, rb.velocity.y); // 恢复常规速度

        if (anim != null)
        {
            anim.ResetTrigger("Dash"); // 重置 Dash 动画触发器
        }

        Debug.Log("Dash Ended");
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

    public void Respawn()
    {
        transform.position = spawnPosition;
        rb.velocity = Vector2.zero; // 重置速度
        deathFX.Play();
        Debug.Log("Player respawned to the original position.");

        diedMenu.Pause();
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
