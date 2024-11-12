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
    public virtual void TakeDamage(int amount,int directionX,float knockBackForce)
    {
        if(!isDead)
        {
            health -= amount;
            if (health <= 0)
            {
                Die(directionX);
                isDead = true;
                animator.SetBool("dead", true);
                gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
            else
            {
                KnockBack(directionX, knockBackForce);
            }
        }
    }
    public void KnockBack(int hitDirectionX, float knockbackForce)
    {
        rb.AddForce(new Vector2(hitDirectionX*knockbackForce,0));
    }

    protected virtual void Die(int directionX)
    {
        rb.AddForce(new Vector2(directionX*deathForceX,deathForceY));
        Destroy(gameObject, 5);
    }
}