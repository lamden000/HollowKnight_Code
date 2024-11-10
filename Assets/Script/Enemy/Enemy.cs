using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyBase : MonoBehaviour
{
    public int health;
    protected int attackPower;

    public float moveSpeedX;
    public float moveSpeedY;
    public float deathForceX = 250f;
    public float deathForceY = 50f;
    [HideInInspector] public bool isDead=false;

    protected Animator animator; 
    protected Rigidbody2D rb;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>(); // Lấy component Animator
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void TakeDamage(int amount,Vector2 direction)
    {
        if(!isDead)
        {
            Knockback(direction, -3f);
            health -= amount;
            if (health <= 0)
            {
                Die(direction);
                isDead=true;
                animator.SetBool("dead", true);
                gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
        }
    }
    public void Knockback(Vector2 hitDirection, float knockbackForce)
    {
        rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
    }

    protected virtual void Die(Vector2 direction)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(direction.normalized.x*deathForceX,deathForceY));
        Destroy(gameObject, 3);
    }
}