using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("地面检测")]
    public Transform groundCheck;
    public float checkRadius = 0.2f; // 增大检测范围
    public LayerMask groundLayer;

    [Header("动画参数")]
    public Animator anim;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded; // 记录上一帧是否在地面上

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("PlayerController: Animator 组件未找到，请确保 Player 对象上添加了 Animator 组件。");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

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

    // 处理角色的水平移动
    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // 更新 Animator 的 Speed 参数
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(move));
        }
    }

    // 处理角色的跳跃
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

    // 处理角色的下落状态和 Landing 动画
    void HandleFalling()
    {
        wasGrounded = isGrounded;
        // 更新地面检测状态
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (anim != null)
        {
            anim.SetBool("isGrounded", isGrounded);
        }
        Debug.Log("isGrounded: " + isGrounded);

        // 判断角色是否开始下落
        if (rb.velocity.y < 0 && !isGrounded)
        {
            if (anim != null)
            {
                anim.SetBool("isFalling", true);
            }
            Debug.Log("Falling");
        }
        else
        {
            if (anim != null)
            {
                anim.SetBool("isFalling", false);
            }
        }

        // 如果刚从下落状态回到地面，触发 Landing 动画
        if (!wasGrounded && isGrounded)
        {
            if (anim != null)
            {
                anim.SetTrigger("LandingTrigger");
            }
            Debug.Log("Landing Triggered");
        }
    }

    // 翻转角色精灵以匹配移动方向
    void FlipSprite()
    {
        float move = Input.GetAxis("Horizontal");

        if (move > 0)
        {
            spriteRenderer.flipX = false; // 朝右
        }
        else if (move < 0)
        {
            spriteRenderer.flipX = true; // 朝左
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 可视化 groundCheck 范围
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
