using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject healParticle;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private GameObject _cameraFollowGO;

    public GameObject slashPrefab;
    public GameObject lifeIconPrefab;
    public Transform lifeContainer; // Parent GameObject with a Horizontal Layout Group


    private int maxLife = 5;
    private int currentLife = 5;
    private List<GameObject> lifeIcons = new List<GameObject>();

    [Header("Soul Settings:")]
    [SerializeField] private GameObject soulSprite; // Reference to the soul sprite's transform
    [SerializeField] private GameObject eye; // eye objects
    [SerializeField] private GameObject soulFullSprite; //Soul full sprite
    [SerializeField] private float soulMinY; // 0% height (y position)
    [SerializeField] private float soulMaxY; // 99% height (y position)
    [SerializeField] private int maxSoul = 99; // Max soul value
    [SerializeField] private Color lowSoulColor;
    [SerializeField] private int currentSoul;
    [SerializeField] private float healHoldTime = 1.5f;
    [SerializeField] private float soulDrainDelay = 0.501f;  // Time before soul starts to drain
    private int soulPerHeal = 33;         // Total soul required for one heal

    [Header("Player setting:")]
    [SerializeField] private float fallStunTime = 0.5f;
    [SerializeField] private float getDamageImmnueTime = 1f;
    public float jumpForce = 5f;
    public float attackCooldown = 1.0f;   // Thời gian cooldown giữa các lần tấn công
    public float moveSpeed = 5f;
    public float attackBounceForce = 20f;
    public float jumpTime;
    public float fallStunThreshold;
    public bool isFacingRight=true;

    private float jumpTimecounter;
    private bool isJumping;

    private bool isStunned = false;
    private bool isImmune = false;
    private float previousYVelocity;
    private bool isHealing;
    private bool canHeal = true;
    private float drainInterval;
    private float drainSoulTimer = 0;
    private float holdTimer;
    private float lastAttackTime = -1.0f; // Lưu lại thời điểm tấn công gần nhất

    private float _fallSpeedYDampingChangeThreshold;


    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    private Coroutine resetTriggerCoroutine;
    private CameraFollow cameraFollowObject;

    [Header("Audio Setting:")]
    private AudioSource audioSource;
    public Sound healingSound;
    public AudioClip healCompleteSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Làm mượt chuyển động
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;

        cameraFollowObject=_cameraFollowGO.GetComponent<CameraFollow>();

        drainInterval = (healHoldTime - soulDrainDelay) / soulPerHeal;

        SetSoul(currentSoul);
        SetLives(maxLife);
        TakeDamage(3);

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
        if(animator.GetBool("dead")|| isStunned)
            return;
        if (Input.GetKey(KeyCode.F) && canHeal && (currentSoul >= soulPerHeal || isHealing) && currentLife < maxLife && IsOnGround())
        {
            if (!isHealing)
            {
                StartHealing();
            }
            else
            {
                animator.SetBool("isHealing", true);
            }

            holdTimer += Time.deltaTime;

            if (holdTimer >= healHoldTime)
            {
                Heal();
                ResetHealingState();
            }
            else if (holdTimer >= soulDrainDelay)
            {
                drainSoulTimer += Time.deltaTime;

                if (drainSoulTimer >= drainInterval)
                {
                    DrainSoul();
                    drainSoulTimer -= drainInterval; // Keep remainder for precision
                }
            }
        }
        else if (isHealing) // Reset only if healing was in progress but the conditions aren't met anymore
        {
            ResetHealingState();
            healingSound.Stop(audioSource);
            healingSound.ResetTime();
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

        if (Input.GetKey(KeyCode.A)&& !Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.A)&& isFacingRight|| Input.GetKey(KeyCode.D) && !isFacingRight)
            {
                isFacingRight = !isFacingRight;
                float yRotation = !isFacingRight ? 0f : 180f;
                transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
                animator.SetBool("turn",true);
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

    void StartHealing()
    {
        isHealing = true;
        holdTimer = 0;
        drainSoulTimer = 0;
        animator.SetTrigger("startHeal");
        healingSound.Play(audioSource);
        healParticle.SetActive(true);
    }

    private void DrainSoul()
    {
        if (currentSoul > 0)
        {
            SetSoul(-1);
        }
        else
        {
            ResetHealingState();
        }
    }

    private void Heal()
    {
        animator.SetTrigger("heal");
        healingSound.Stop(audioSource);
        ResetHealingState();
        audioSource.PlayOneShot(healCompleteSound);
        lifeIcons[currentLife].GetComponent<Animator>().SetTrigger("restore");
        currentLife++;
        canHeal = false;
    }

    void ResetHealingState()
    {
        isHealing = false;
        holdTimer = 0;
        drainSoulTimer = 0;
        animator.SetBool("isHealing", false);
        healParticle.SetActive(false);
    }


    void FixedUpdate()
    {
        previousYVelocity = rb.velocity.y;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (previousYVelocity < fallStunThreshold)
            {
                isStunned = true;
                StartCoroutine(Stun(fallStunTime));
            }
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            StartCoroutine(Stun(0.5f));
        }

    }

    private IEnumerator Stun(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isStunned=false;
    }

    private IEnumerator Immune(float immuneTime)
    {
        yield return new WaitForSeconds(immuneTime);
        isImmune = false;
    }

    public void AllowHeal()
    {
        canHeal = true;
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

    private void Die()
    {
        animator.SetBool("dead",true);
    }


    public void TakeDamage(int damage)
    {
        if(isImmune) return;
        for (int i = damage; i > 0; i--)
        {
            currentLife--;
            lifeIcons[currentLife].GetComponent<Animator>().SetTrigger("break");
            if (currentLife == 0)
            {
                Die();
                return;
            }
        }
        isImmune = true;
        StartCoroutine(Immune(getDamageImmnueTime));
    }
    public void SetLives(int lifeCount)
    {
        // Clear old icons
        foreach (var icon in lifeIcons)
        {
            Destroy(icon);
        }
        foreach (Transform child in lifeContainer)
        {
            Destroy(child.gameObject);
        }
        lifeIcons.Clear();

        // Instantiate icons based on lifeCount
        for (int i = 0; i < lifeCount; i++)
        {
            GameObject icon = Instantiate(lifeIconPrefab, lifeContainer);
            lifeIcons.Add(icon);
        }
    }
    public void SetSoul(int soul)
    {
        // Add the soul value, clamping it to the max soul limit
        currentSoul = Mathf.Clamp(currentSoul + soul, 0, maxSoul);

        float newYPosition = Mathf.Lerp(soulMinY, soulMaxY, (float)currentSoul / maxSoul);

        // Apply the new position to the soul sprite
        Vector3 spritePosition = soulSprite.transform.localPosition;
        spritePosition.y = newYPosition;
        soulSprite.transform.localPosition = spritePosition;

        // Show eyes when soul percentage reaches 50% (or another threshold)
        if (currentSoul>=50)
        {
            if(currentSoul==99)
            {
                eye.SetActive(false);
                soulSprite.SetActive(false);
                soulFullSprite.SetActive(true);
            }
            else
            {              
                eye.SetActive(true);
                soulSprite.SetActive(true);
                soulFullSprite.SetActive(false);
            }
        }
        else
        {
            if (currentSoul < 33)
            {
                soulSprite.GetComponent<Image>().color = lowSoulColor;
            }
            eye.SetActive(false);
        }
        if(currentSoul>33)
        {
            soulSprite.GetComponent<Image>().color=Color.white;
        }
    }
}
