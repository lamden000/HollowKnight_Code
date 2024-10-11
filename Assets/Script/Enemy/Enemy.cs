using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Move();
    void Attack();
    void TakeDamage(int amount,Vector2 direction);
    void SetState(int state);
    void Render();
}
public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    protected int health;
    protected int attackPower;
    protected int state;

    protected float vx;
    protected float vy;

    protected int nx;
    protected int ny;
    protected Animator animator; // Tham chiếu đến Animator
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Lấy component Animator
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void SetState(int state)
    {
        this.state = state;
    }
    public int GetState()
    {
        return state;
    }
    public abstract void Move();

    public abstract void Attack();
    public abstract void Render();
    public virtual void TakeDamage(int amount,Vector2 direction)
    {
        Knockback(direction, -5f);
        health -= amount;
        if (health <= 0)
        {
            Die(direction);
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