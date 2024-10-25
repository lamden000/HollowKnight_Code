using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyBase : MonoBehaviour
{
    public int health;
    protected int attackPower;

    public float moveSpeedX;
    public float moveSpeedY;

    protected Animator animator; 
    protected Rigidbody2D rb;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>(); // Lấy component Animator
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void TakeDamage(int amount,Vector2 direction)
    {
        if(!animator.GetBool("dead"))
        {
            Knockback(direction, -5f);
            health -= amount;
            if (health <= 0)
            {
                Die(direction);
                animator.SetBool("dead", true);
            }
        }
    }
    public void Knockback(Vector2 hitDirection, float knockbackForce)
    {
        rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
    }

    protected virtual void Die(Vector2 direction)
    {
        Debug.Log($"{this.GetType().Name} has died.");
    }
}