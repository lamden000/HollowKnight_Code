using System.Collections.Generic;
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
    private float lastAttackTime = -1.0f;    // Lưu lại thời điểm tấn công gần nhất
    public float moveSpeed = 5f;
    public float attackBounceForce=20f;
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
        direction = Direction.right;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleKeyInput();
        // Cập nhật vị trí của nhân vật
    }

    private void FixedUpdate()
    {
        
    }
    void HandleKeyInput()
    {
        string[] bools = { "isRunning", "idle","isWalking" };
        foreach (string s in bools) {
            animator.SetBool(s, false);
        }
        // Lấy input từ bàn phím
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Tạo vector di chuyển
        velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (velocity.magnitude > 0)
        {
            animator.SetBool("isWalking", true);

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
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
        else {
            animator.SetBool("idle", true);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time >= lastAttackTime + attackCooldown+0.1)
            {
                NormalAttack(0);
            }
            else if(Time.time >= lastAttackTime + attackCooldown)
            {
                NormalAttack(1);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Ground()) // Nhấn phím Space để nhảy
        {
            isJumping = true;
            jumpTimecounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if(jumpTimecounter > 0)
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
        //transform.Translate(velocity);
    }
    public bool Ground()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void NormalAttack(int type)
    {
        GameObject effect = Instantiate(slashPrefab, transform.position + new Vector3(1 * (int)direction, 0, 0), transform.rotation);
        effect.transform.Translate(velocity);
        effect.GetComponent<SlashPrefab>().Instantiate("NormalAttack" + type);
        animator.SetTrigger("normalAttack");
        Destroy(effect, 0.1f);
        lastAttackTime = Time.time;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }    
    public void AttackBounceBack(Vector2 direction)
    {
        rb.velocity.Set(direction.x*attackBounceForce, rb.velocity.y);
    }
}