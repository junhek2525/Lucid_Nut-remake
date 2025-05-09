using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] public float moveSpeed = 5f; // 이동 속도
    [SerializeField] public float jumpForce = 10f; // 점프 힘

    [SerializeField] public float coyoteTime = 0.2f;
    [SerializeField] public float jumpBufferTime = 0.2f;

    private float gravityScale = 3.5f;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded; // 바닥에 있는지 여부
    [SerializeField] public float groundCheckDistance;
    [SerializeField] public Transform groundCheck; // 바닥 체크 위치
    [SerializeField] public Vector2 groundCheckSize = new Vector2(1f, 0.1f); // 바닥 체크 박스 크기
    [SerializeField] public LayerMask groundLayer; // 바닥 레이어 마스크

    [Header("Component")]
    public GameObject wingBong;
    public ParticleSystem dust;
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rb { get; private set; }
    private PlayerAnimator_M animator;
    private PlayerSkill playerSkill;

    [Header("IsAtcitoning")]
    [SerializeField] public bool isPlatform = false;
    [SerializeField] private bool isJumping; // 점프 중인지 여부
    [SerializeField] public bool isFacingRight = false; // 플레이어가 오른쪽을 보고 있는지 여부

    private int facingDir;
    private int moveInput = 0;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public bool isDashing = false;
    public bool isAttack = false;
    private bool isJumpCut = false;

    // 추가된 변수들
    [Header("Double Jump")]
    [SerializeField] private bool canDoubleJump = true; // 더블 점프 기능을 켜고 끄는 변수
    private bool doubleJumpAvailable = false; // 더블 점프 가능 여부

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSkill = GetComponent<PlayerSkill>();
        animator = GetComponent<PlayerAnimator_M>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        gravityScale = rb.gravityScale;
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        // 좌우 이동
        MoveInput();
        Jump();

        // 캐릭터 방향 설정
        Flip();

        // 바닥 체크
        GroundCheck();


        GravitySetting();

        AnimationController();
    }

    private void GravitySetting()
    {
        if (playerSkill.isUmbrellaOpen && rb.velocity.y <= 0)
        {
            rb.gravityScale = playerSkill.umbrellaFallMultiplier;
        }
        else
        {

            rb.gravityScale = gravityScale;
        
        }
    }

    void MoveInput()
    {
        moveInput = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1;
        }
        rb.velocity = new Vector2(moveInput * moveSpeed / (isAttack ? 2 : 1), rb.velocity.y);
    }

    private void Flip()
    {
        if (isAttack)
            return;

        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            if (isGrounded)
                CreateDust();

            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;  // 스프라이트의 x축을 뒤집기
        }
    }

    public void FlipAttack(int directionX)
    {
        // 캐릭터의 방향을 바꾸는 로직
        if (isFacingRight && directionX < 0 || !isFacingRight && directionX > 0)
        {
            if (isGrounded)  // 캐릭터가 땅에 있을 때만 먼지 생성
                CreateDust();

            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;  // 스프라이트의 x축을 뒤집기
        }
    }


    private void Jump()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            doubleJumpAvailable = true; // 바닥에 닿으면 더블 점프 가능
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            if(jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            CreateDust();
            PerformJump();
            isJumpCut = true;
        }
        else if (canDoubleJump && doubleJumpAvailable && !isGrounded && Input.GetButtonDown("Jump"))
        {
            PerformJump();
            StartCoroutine(WingEffectStart());
            isJumpCut = true;
            doubleJumpAvailable = false; // 더블 점프 사용 후에는 더블 점프 불가
        }

        if (isJumpCut && Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
            isJumpCut = false;
        }
    }

    IEnumerator WingEffectStart()
    {
        wingBong.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        wingBong.SetActive(false);
    }

    private void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpBufferCounter = 0f;
        StartCoroutine(JumpCooldown());
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    public bool canUmbrella()
    {
        return !isDashing && !isAttack;
    }

    private void GroundCheck()
    {
        if (!isPlatform)
        {
            Collider2D collider = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
            isGrounded = collider != null;
        }
        else
        {
            isGrounded = false;
        }
    }

    void CreateDust()
    {
        dust.Play();
    }

    private void AnimationController()
    {
        if (isAttack)
        {
            animator.PlayAnimation("Attack");
        }
        else if (!isGrounded && rb.velocity.y < 0)
        {
            animator.PlayAnimation("Fall");
        }
        else if (!isGrounded && rb.velocity.y > 0)
        {
            animator.PlayAnimation("Jump");
        }
        else if (isGrounded && moveInput != 0)
        {
            animator.PlayAnimation("Move");
        }
        else if (isGrounded && moveInput == 0)
        {
            animator.PlayAnimation("Idle");
        }
    }


    void OnDrawGizmos()
    {
        // 바닥 체크 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
