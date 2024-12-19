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
    private int currentSoul;
    [SerializeField] private float healHoldTime = 1.5f;
    [SerializeField] private float soulDrainDelay = 0.501f;  // Time before soul starts to drain
    private int soulPerHeal = 33;         // Total soul required for one heal

    [Header("Player setting:")]
    [SerializeField] private float hitStunTime = 1f;
    [SerializeField] private float getDamageImmnueTime = 1f;
    public float jumpForce = 5f;
    public float attackCooldown = 1.0f;   // Thời gian cooldown giữa các lần tấn công
    public float maxSpeedX = 10f;
    public float accelerationX = 5f;

    public float attackBounceForce;
    public float hitBounceForceX;
    public float hitBounceForceY;

    public float jumpTime;
    public bool isFacingRight=true;

    private float jumpTimecounter;
    private bool isJumping;

    private bool isStunned = false;
    private bool isImmune = false;
    private bool isHealing;
    private bool canHeal = true;
    private bool isdead=false;
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
        if (GameManager.Instance.playerStats.Health != 0)
            GetPreviousStats();
        else
        {
            SetSoul(currentSoul);
            SetLives();
        }
        GameManager.Instance.UpdateMoney();
    }


    public void StoreCurrentStats()
    {
        GameManager.Instance.playerStats.Health = currentLife;
        GameManager.Instance.playerStats.Soul =currentSoul;
    }

    public void GetPreviousStats()
    {
        currentLife = GameManager.Instance.playerStats.Health;
        currentSoul = 0;
        SetSoul(GameManager.Instance.playerStats.Soul);
        SetLives();
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
        animator.SetFloat("yVelocity",rb.velocity.y);
    }

    void HandleKeyInput()
    {
        if(animator.GetBool("dead")|| isStunned)
        {
            if (isHealing)
            {
                ResetHealingState(false);
            }
            return;
        }
        if (Input.GetKey(KeyCode.F)  && (currentSoul >= soulPerHeal || isHealing) && currentLife < maxLife && IsOnGround())
        {
            if (canHeal)
            {
                if (holdTimer==0)
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
                    ResetHealingState(true);
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
        }
        else if (isHealing) // Reset only if healing was in progress but the conditions aren't met anymore
        {
            ResetHealingState(false);
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
                Turn();
                animator.SetBool("turn", true);
            }
            if (IsOnGround())
            {
                animator.SetBool("isWalking", true);
            }

            // Determine the target velocity based on the facing direction
            float targetVelocityX = isFacingRight ? maxSpeedX : -maxSpeedX;

            // Smoothly interpolate the current velocity towards the target velocity
            float newVelocityX = Mathf.MoveTowards(rb.velocity.x, targetVelocityX, accelerationX * Time.fixedDeltaTime);

            // Set the Rigidbody's velocity
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);
        }
        else
        {
            //float newVelocityX = Mathf.MoveTowards(rb.velocity.x, 0, accelerationX * Time.fixedDeltaTime);
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
            ResetHealingState(false);
        }
    }

    private void Turn()
    {
        isFacingRight = !isFacingRight;
        float yRotation = !isFacingRight ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        cameraFollowObject.CallTurn();
    }

    private void Heal()
    {
        animator.SetTrigger("heal");
        healingSound.Stop(audioSource);
        audioSource.loop = false;
        audioSource.clip=healCompleteSound;
        audioSource.Play();
        lifeIcons[currentLife].GetComponent<Animator>().SetTrigger("restore");
        currentLife++;
        canHeal = false;
        Hit_CrackController.instance.StopHetmauEffect();
    }

    void ResetHealingState(bool isHealing)
    {
        this.isHealing = isHealing;
        if (!isHealing)
        {
            healingSound.Stop(audioSource);
            healingSound.ResetTime();
        }
        holdTimer = 0;
        drainSoulTimer = 0;
        animator.SetBool("isHealing", false);
        healParticle.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            // Get the position of the other object
            Vector2 otherPosition = collision.transform.position;
            // Calculate the direction vector
            int direction = otherPosition.x - transform.position.x>0?-1:1;
            TakeDamage(1, direction);          
        }

    }

    private IEnumerator Stun(float stunTime)
    {
        isStunned = true;
        yield return new WaitForSeconds(stunTime);
        isStunned=false;
    }

    private IEnumerator Immune(float immuneTime)
    {
        isImmune = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        yield return new WaitForSeconds(immuneTime);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
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
        rb.AddForce( new Vector2(direction.x * attackBounceForce,0));
    }

    private void GetDamageBounceBack(int direction)
    {
        if(direction>0&&isFacingRight|| direction < 0 && !isFacingRight)
        {
            Turn();
        }
        animator.SetTrigger("hit");
        Vector2 bounceForce = new Vector2(direction* hitBounceForceX, hitBounceForceY);
        rb.AddForce(bounceForce);
    }


    private void Die()
    {
        GameManager.Instance.ResetGame();
        isdead = true;
        gameObject.tag = "DeadPlayer";
        NotifyEnemies();
        animator.SetBool("dead",true);
    }

    private void NotifyEnemies()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        foreach (EnemyBase enemy in enemies)
        {
            enemy.UpdatePlayerReference();
        }
    }

    public void TakeDamage(int damage, int hitDirection)
    {
        if (isImmune||isdead) return;
        GetDamageBounceBack(hitDirection);
        StartCoroutine(Stun(hitStunTime));
        if (currentLife > 1)
            Hit_CrackController.instance.OpenEffect();
        else
            Hit_CrackController.instance.LastHealthEffect();
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

        if (currentLife == 1)
        {
            Hit_CrackController.instance.HetmauEffect();
            var icon = lifeIconPrefab.transform.GetChild(0).gameObject;
            var effect = icon.transform.GetChild(0).gameObject;
            effect.SetActive(true);
        }
        StartCoroutine(Immune(getDamageImmnueTime));
    }
    public void SetLives()
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
        for (int i = 0; i < 5; i++)
        {
            GameObject icon = Instantiate(lifeIconPrefab, lifeContainer);
            lifeIcons.Add(icon);
            if (i >= currentLife)
                icon.GetComponent<Animator>().SetTrigger("break");
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
