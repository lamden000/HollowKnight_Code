using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    enum Direction
    {
        right = 1,
        left = -1
    }

    Rigidbody2D rb;
    public float jumpForce = 5f;
    public float attackCooldown = 1.0f;   // Thời gian cooldown giữa các lần tấn công
    private float lastAttackTime = -1.0f; // Lưu lại thời điểm tấn công gần nhất
    public float moveSpeed = 5f;
    public float attackBounceForce = 20f;
    public Vector2 velocity;
    Animator animator;
    public GameObject slashPrefab;

    private float jumpTimecounter;
    public float jumpTime;
    private bool isJumping;

    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    Direction direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Làm mượt chuyển động
        direction = Direction.right;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleKeyInput();  // Xử lý các input không liên quan đến vật lý trong Update
    }

    private void FixedUpdate()
    {
        HandleMovement(); 
    }

    void HandleKeyInput()
    {
       
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
        if (Input.GetKeyDown(KeyCode.Space) && Ground()) // Nhấn phím Space để nhảy
        {
            isJumping = true;
            jumpTimecounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void HandleMovement()
    {
        // Đặt lại các animation flags
        string[] bools = { "isRunning", "idle", "isWalking" };
        foreach (string s in bools)
        {
            animator.SetBool(s, false);
        }

        // Lấy input từ bàn phím
        float horizontal = Input.GetAxis("Horizontal");

        // Tạo vector di chuyển
        velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        // Kiểm tra xem có di chuyển không
        if (Mathf.Abs(horizontal) > 0.01f)
        {
            animator.SetBool("isWalking", true);

            // Đổi hướng nhân vật dựa trên hướng di chuyển
            if (horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                direction = Direction.right;
            }
            else if (horizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                direction = Direction.left;
            }
        }
        else
        {
            animator.SetBool("idle", true);
        }

        // Cập nhật vận tốc di chuyển (dùng Time.deltaTime cho mượt mà)
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    public bool Ground()
    {
        // Kiểm tra nếu nhân vật đang đứng trên mặt đất
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround);
    }

    private void NormalAttack(int type)
    {
        // Tạo hiệu ứng tấn công và chơi animation tấn công
        GameObject effect = Instantiate(slashPrefab, transform.position + new Vector3(1 * (int)direction, 0, 0), transform.rotation);
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
