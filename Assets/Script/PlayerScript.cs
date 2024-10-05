using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    enum Direction
    {
        right = 1,
        left = -1
    }

    public float attackCooldown = 1.0f;   // Thời gian cooldown giữa các lần tấn công
    private float lastAttackTime = -1.0f;    // Lưu lại thời điểm tấn công gần nhất
    public float moveSpeed = 5f;
    Vector2 velocity;
    Animator animator;
    public GameObject slashPrefab;

    Direction direction;

    private void Start()
    {
        direction = Direction.right;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleKeyInput();
        // Cập nhật vị trí của nhân vật
    }

    void HandleKeyInput()
    {
        string[] bools = { "isRunning", "idle","isWalking" };
        foreach (string s in bools) {
            animator.SetBool(s, false);
        }
        // Lấy input từ bàn phím
        float horizontal = Input.GetAxis("Horizontal");

        // Tạo vector di chuyển
        velocity = new Vector2(horizontal, 0) * moveSpeed * Time.deltaTime;
 
        if (velocity.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
            if (horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                direction = Direction.right;
                velocity.x = -velocity.x;
            }
            else if (horizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                direction = Direction.left;
            }
        }
        else {
            animator.SetBool("idle", true);
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
        
        transform.Translate(velocity);
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
}