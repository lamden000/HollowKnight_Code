using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerScript : MonoBehaviour
{

    Rigidbody2D rb;
    public float jumpForce = 5f;
    public float attackCooldown = 1.0f;   // Thời gian cooldown giữa các lần tấn công
    private float lastAttackTime = -1.0f; // Lưu lại thời điểm tấn công gần nhất
    public float moveSpeed = 5f;
    public float attackBounceForce = 20f;
    Animator animator;
    public GameObject slashPrefab;

    private float jumpTimecounter;
    public float jumpTime;
    private bool isJumping;

    public bool isFacingRight;

    private float _fallSpeedYDampingChangeThreshold;


    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private GameObject _cameraFollowGO;
    private Coroutine resetTrigerCoroutine;
    private CameraFollow cameraFollowObject;

    private void Start()
    {
        isFacingRight=true;
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Làm mượt chuyển động
        animator = GetComponent<Animator>();
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;

        cameraFollowObject=_cameraFollowGO.GetComponent<CameraFollow>();

    }

    void Update()
    {
        HandleKeyInput();  // Xử lý các input không liên quan đến vật lý trong Update
        if (rb.velocity.y <_fallSpeedYDampingChangeThreshold&&!CameraManager.instance.IsLerpingYDamping&&!CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if (rb.velocity.y >=0 && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling=false;
            CameraManager.instance.LerpYDamping(false);
        }

        animator.SetBool("isGrounded",IsOnGround());
        if (!IsOnGround())
        {
            animator.SetFloat("yVelocity",rb.velocity.y);
        }
    }

    void HandleKeyInput()
    {
        // Đặt lại các animation flags
        string[] bools = { "isRunning", "idle", "isWalking","isJumping" };
        foreach (string s in bools)
        {
            animator.SetBool(s, false);
        }

        // Xử lý tấn công bằng chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= lastAttackTime + attackCooldown + 0.1)
            {
                NormalAttack(0);
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                NormalAttack(1);
            }
        }

        // Xử lý nhảy
        if (Input.GetKeyDown(KeyCode.Space) && IsOnGround()) // Nhấn phím Space để nhảy
        {
            isJumping = true;
            jumpTimecounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimecounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimecounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) )
        {
            if (Input.GetKey(KeyCode.A)&& isFacingRight|| Input.GetKey(KeyCode.D) && !isFacingRight)
            {
                float yRotation = isFacingRight ? 0f : 180f;
                transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

                isFacingRight = !isFacingRight;
                cameraFollowObject.CallTurn();
            }
            if (IsOnGround())
            {
                animator.SetBool("isWalking", true);
            }

            float direction = isFacingRight ? moveSpeed : -moveSpeed;
            rb.velocity = new Vector2(direction, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("isWalking", false);
        }

    }


    public bool IsOnGround()
    {
        // Kiểm tra nếu nhân vật đang đứng trên mặt đất
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround);
    }

    private void NormalAttack(int type)
    {
        int diretionIndex= isFacingRight? 1:-1;
        // Tạo hiệu ứng tấn công và chơi animation tấn công
        GameObject effect = Instantiate(slashPrefab, transform.position + new Vector3(1 * diretionIndex, 0, 0), transform.rotation);
        effect.GetComponent<SlashPrefab>().Instantiate("NormalAttack" + type);
        animator.SetTrigger("normalAttack");
        Destroy(effect, 0.1f); // Hủy hiệu ứng sau 0.1 giây
        lastAttackTime = Time.time;
    }

    public void AttackBounceBack(Vector2 direction)
    {
        // Tạo lực phản hồi khi tấn công
        rb.velocity = new Vector2(direction.x * attackBounceForce, rb.velocity.y);
    }
}
